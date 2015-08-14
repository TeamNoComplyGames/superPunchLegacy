using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
	//Our enemy sprite
	public Rigidbody2D enemy;
	//Our enemy movepseed
	public float emoveSpeed = 0f;
	//Our enemy mass(Done in inspector)
	private float defaultMass;

	//Our enemy skills
	public float speed;
	public float healthMultiplier;
	public float attack;
	
	//Our player stats
	public int ehealth;
	//Static makes it available to other classes
	public int elevel;
	//Boolean if our enemy is dead
	private bool dead;
	//Boolean if we are exploding
	private bool exploding;
	//Explosion Sound
	private AudioSource explosionSound;
	//Our explode position
	private Vector3 explodePos;
	//is our enemy a boss?
	private bool isBoss;

	//Our target to fight
	private Player player;

	private SpriteRenderer render; //Our sprite renderer to change our sprite color
	private bool showFlash;
	private Color defaultColor;
	private Animator animator;   //Used to store a reference to the Player's animator component.

	//Frames until the enemy will atack
	public int attackFrames;
	private int totalFrames;

	//boolean if colliding with player
	private bool playerCollide;

	//Boolean if colliding with a walll/object
	private bool wallCollide;
	private float wallCollideTime;

	//Knockback value
	private float knockForce;
	public int knockFrames;
	private float knockTime;
	private float knockStart;
	private int totalKnockFrames;
	private bool knockBool;

	//Our sounds
	private AudioSource hurt;

	//Our game manager
	GameManager gameManager;

	//our camera Script
	Bounded2DCamera actionCamera;

	// Use this for initialization
	void Start () 
	{

		//Get a component reference to the Player's animator component
		render = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
		showFlash = false;
		defaultColor = Color.white;

		//Get our gammaneger
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

		//Enemy is not dead
		dead = false;

		//Enemy is not colliding with player
		playerCollide = false;

		//Get our sounds
		explosionSound = GameObject.Find ("Explosion").GetComponent<AudioSource> ();


		int playerLevel = Player.playerLevel;
		//Create our enemy based off our the player's current level
		elevel = playerLevel / 2;
		if (elevel <= 0) {
			elevel = 1;
		}

		//gET IF OUR ENEMY IS A BOSS OR NOT, bosses will always explode
		if (gameManager.getBossMode()) {
			//Since it is a boss, increase it's scale thier stats
			//I really gotta clean up this code, divide by two, than multiply by 2 :p
			//Oh well, I'm trying to push this out so we can get people playin! :D
			elevel = elevel * 3;
			healthMultiplier = healthMultiplier * 4;
			speed = speed / 2;
			attack = attack / 2;

			//Also make this enemy larger
			transform.localScale = new Vector3(transform.localScale.x * 2.25f , transform.localScale.y * 2.25f, 0);

			//Also, set the bosses color
			Color[] bossColors = {Color.white, Color.magenta, Color.yellow,Color.green,Color.blue,Color.cyan,Color.grey};
			defaultColor = bossColors[(gameManager.getBossesSpawned() - 1) / gameManager.bosses.Length];
			render.material.color = defaultColor;

			//set that this enemy is boss
			isBoss = true;
		} 


		//Get health using enemy skill
		ehealth = (int) (elevel * 2.75f * healthMultiplier);

		//Set the mass of the rigid body to be really high so they dont go flying
		defaultMass = enemy.mass;
		//Our knock force so we do go flying haha, and knock time for how long we can be in knockback
		knockTime = 0.75f;
		knockForce = 100000;
		if (isBoss) {
			//If we are a boss, dont go flying as much
			knockForce = knockForce / 2;
		}

		//Save the total amount of frames before we attack 
		totalFrames = attackFrames;
		totalKnockFrames = knockFrames;
		knockBool = false;

		//INit explosion
		explodePos = Vector3.zero;

		//Get our sounds
		hurt = GameObject.Find ("Hurt").GetComponent<AudioSource> ();

		//Get our camera script
		actionCamera = Camera.main.GetComponent<Bounded2DCamera>();

		//Increase our number of enemies
		gameManager.plusEnemy();


		//Go after our player!
		player = GameObject.Find("Person").GetComponent<Player>();
	}
	
	//Called every frame
	void Update ()
	{
		//check if exploding
		if (exploding) 
		{
			//reset our knowckback frames
			knockFrames = totalKnockFrames;
			//and make us kinematic again
			knockBool = false;
			//And remove the force we added
			enemy.angularVelocity = 0f;
			enemy.velocity = Vector2.zero;

			transform.localPosition = explodePos;
			
			//set mass back to default value
			enemy.mass = defaultMass;
		}
		//If we are being knockbacked, knock back for so many frames
		else if (knockBool) 
		{
			if (knockFrames > 0) 
			{
				--knockFrames;

				Debug.Log(knockFrames);

				//Also check to make sure we havent been in knockback for too long
				if(knockStart + knockTime < Time.realtimeSinceStartup)
				{
					//End the knock back

					//reset our knowckback frames
					knockFrames = totalKnockFrames;
					//and make us kinematic again
					knockBool = false;
					//And remove the force we added
					enemy.angularVelocity = 0f;
					enemy.velocity = Vector2.zero;
					
					//set mass back to default value
					enemy.mass = defaultMass;
				}
			} 
			else 
			{
				//reset our knowckback frames
				knockFrames = totalKnockFrames;
				//and make us kinematic again
				knockBool = false;
				//And remove the force we added
				enemy.angularVelocity = 0f;
				enemy.velocity = Vector2.zero;

				//set mass back to default value
				enemy.mass = defaultMass;
			}
		} 
		//MOve only if game is not over, enemy is not dead, and they are not collidign with the player
		else if(!gameManager.getGameStatus() && !dead && !playerCollide)
		{
			//Move our player
			Move ();
		}

		//Check if enemy is dead
		if (ehealth <= 0 && !dead) 
		{
			//Add experience to the player
			player.addEXP(elevel);
			//Decrease the number of enemies we have
			gameManager.minusEnemy();

			//Destroy this enemy, possible display some animation first
			//Destroy(gameObject);

			//Get an explosion chance
			int exChance = Random.Range(0, 20);

			//Now check explosion chance, or bosses always explode
			if(exChance > 17 || isBoss)
			{
				//EXPLODEEEE

				//Call our explosion coroutine
				StartCoroutine("explode");
			}
			else
			{
				//Move our enemy out of the way and play the death animation
				animator.SetTrigger("DeathTrigger");
				animator.SetBool("Death", true);

				//Set our sorting layer as a corpse so we step on top of it
				render.sortingLayerName = "Permenance";

				//and remove box collider
				GetComponent<BoxCollider2D>().isTrigger = true;
			}

			//Set death boolean to true
			dead = true;

		}
	}
	
	//Function to move our player
	void Move ()
	{
		//Get our speed according to our current level
		//Using enemy skill
		float superSpeed = emoveSpeed + (elevel * speed / 15);

		//Get our movement vector
		Vector2 move = Vector2.MoveTowards(transform.position, player.transform.position, superSpeed * Time.deltaTime);

		//Get our angle stuff
		float h = transform.position.x - player.transform.position.x;
		float v = transform.position.y - player.transform.position.y;

		//Set enemy Direction
		//Also check to make sure we stay that direction when not moving, so check that we are
		if(h != 0 || v != 0)
		{
			
			//animate to the direction we are going to move
			//Find the greatest absolute value to get most promenint direction
			/*
		 * 		2
		 * 3		1
		 * 		0
		 * 
		 * Greater than less than flipped for enemyw
		 * */
			if (Mathf.Abs (h * 100) > Mathf.Abs (v * 100)) 
			{
				if(h < 0)
				{
					animator.SetInteger("Direction", 1);
				}
				else
				{
					animator.SetInteger("Direction", 3);
				}
			} 
			else
			{
				if(v < 0)
				{
					animator.SetInteger("Direction", 2);
				}
				else
				{
					animator.SetInteger("Direction", 0);
				}
			}
		}
		
		//Get the position we want to move to, and go to it using move towards
		transform.position =  move;
	}

	//Knockback function for enemies, with direction
	public void knockBack(int direction, int amount)
	{
		//Knockback according to player direction
		if (direction == 0) 
		{
			//Down
			enemy.AddForce(new Vector2(0, -1.0f * knockForce));
		} 
		else if (direction == 1) 
		{
				//Right
			enemy.AddForce(new Vector2( 1.0f * knockForce, 0));
		} 
		else if (direction == 2) 
		{
				//Up
			enemy.AddForce(new Vector2(0, 1.0f * knockForce));
		} 
		else 
		{
				//KLeft
			enemy.AddForce(new Vector2( -1.0f * knockForce, 0));
		}

		//now set the knockframes to the amount
		knockFrames = knockFrames * amount/2;
		//Set our knock force to a high amount
		knockForce = knockForce + (amount * 2000);
		if (knockForce > 1750000) 
		{
			knockForce = 1750000;
		}
		//Set knockback to true
		knockBool = true;

		//Set a time for knockback, so they cant be in knockback after a certain amount of time
		knockStart = Time.realtimeSinceStartup;

		//increase enemy mass so it'll runthrough other enemies
		enemy.mass = defaultMass/2;
	}

	//Our explosion coroutine
	IEnumerator explode()
	{
		//First set exploding to true
		exploding = true;

		//Set our explosion posisiton to our position
		explodePos = transform.localPosition;

		//Set our sorting layer
		render.sortingLayerName = "Explosions";

		//Set the explosion trigger
		animator.SetBool ("Explode", true);

		//Play the sound, stop if already playing
		if (explosionSound.isPlaying) 
		{
			explosionSound.Stop();
		}
		explosionSound.Play ();

		//Shake the screen
		actionCamera.startShake();
		
		//Add slight impact pause
		actionCamera.startImpact();

		//Expand our scale by 25%!
		transform.localScale += new Vector3(transform.localScale.x * 1.25f, transform.localScale.y * 1.25f, 0);

		//Stay exploding for a while
		yield return new WaitForSeconds (0.5f);

		//Set explosing to false
		exploding = false;

		//Check if it was a boss, if it was, set boss mode to false
		if (isBoss) {
			gameManager.setBossMode (false);
		}

		//Delete the enemy
		Destroy(gameObject);
	}

	//Catch when we collide with something
	void OnCollisionEnter2D(Collision2D collision) 
	{
			//Check if it is awall
		if(collision.gameObject.tag == "Wall" && knockBool)
			{
				//Lose some health
			--ehealth;


			//And stop our knowback if we are in knockback
			//reset our knowckback frames
			knockFrames = totalKnockFrames;
			//and make us kinematic again
			knockBool = false;
			//And remove the force we added
			enemy.angularVelocity = 0f;
			enemy.velocity = Vector2.zero;

			//set our wallCollision time
			wallCollideTime = Time.realtimeSinceStartup;
			}
		//Check if it is another enemy, mass will lower for pushing one another
		else if(collision.gameObject.tag == "Enemy" && knockBool)
		{
			//Lose some health, this is only called if they were being knock backed
			--ehealth;

			//Knockback the enemy

			//Get the enemy
			Enemy e = (Enemy) collision.gameObject.GetComponent("Enemy");

			//lower enemy mass
			e.enemy.mass = defaultMass / 3;
		}

	}

	//Catch when we collide with enemy
	void OnCollisionStay2D(Collision2D collision) 
	{
			//Check if it is the player
			if (collision.gameObject.tag == "Player") {
			//Set player collide to true
			playerCollide = true;


			//Decrease the number of frames until we attack
			if (attackFrames > 0) {
				--attackFrames;
			} else if (dead) {
				//Do nothing ig dead
			}
			//attack the player
			else {
				//Set the attack trigger of the player's animation controller in order to play the player's attack animation.
				animator.SetTrigger ("Attack");

				//Only do damage if they are not dodging
				if(!player.isDodging())
				{
					//Get the player
					Player p = (Player)collision.gameObject.GetComponent ("Player");

					//Now using an int to calulate our damage before we apply to health
					//Using enemy skill
					int damage = (int)(elevel * attack / 1.5);
					if (damage < 1) {
						damage = 1;
					}
					int newHealth = p.getHealth () - damage;
					p.setHealth (newHealth);

					//Play the sound of hurt, only if the game is still on
					if (!gameManager.getGameStatus ()) {
						hurt.Play ();
					}

					//Shake the screen
					actionCamera.startShake ();

					//impact pause from the player
					actionCamera.startImpact ();
				}

				//Reset attack frames
				attackFrames = totalFrames;
			}
		}
		//also check if we were by exploding enemy
		else if (collision.gameObject.tag == "Enemy" && 
		         collision.gameObject.GetComponent<Enemy>().exploding) 
		{
			//Lose health and knockback
			ehealth = ehealth - collision.gameObject.GetComponent<Enemy>().elevel;

			//Get our opposite facing direction
			int oppDir = 0;
			int curDir = animator.GetInteger("Direction");

			if(curDir == 0)
			{
				oppDir = 2;
			}
			else if(curDir == 1)
			{
				oppDir = 3;
			}
			else if(curDir == 3)
			{
				oppDir = 1;
			}

			//Now knockback ourselves in the oppostie direction we were facing
			knockBack(oppDir, (collision.gameObject.GetComponent<Enemy>().elevel * 2));
		}

		//Lastly check if we are stuck to a wall
		else if (collision.gameObject.tag == "Wall") 
		{
			//Check how far in time we are since we collided
			if((wallCollideTime + 1.5 < Time.realtimeSinceStartup) && !wallCollide)
			{
				//set wallCollide to true
				wallCollide = true;
			}
		}
		
	}

	//Catch when we collide with something
	void OnCollisionExit2D(Collision2D collision) 
	{
		//Reset enemy mass on exit
		if(collision.gameObject.tag == "Enemy")
		{
			//Get the enemy
			Enemy e = (Enemy) collision.gameObject.GetComponent("Enemy");
			
			//reset enemy mass
			e.enemy.mass = defaultMass;
			
		}

		//Check if it is the player
		else if (collision.gameObject.tag == "Player") {
			//Set player collide to false
			playerCollide = false;
		}

		//Always set wall collide to false
		wallCollide = false;
	}

	//After corpse has died, and we are an is trigger
	void OnTriggerEnter2D(Collider2D collision) 
	{
		if(collision.gameObject.tag == "Wall")
		{
			//And stop our knowback if we are in knockback
			//reset our knowckback frames
			knockFrames = totalKnockFrames;
			//and make us kinematic again
			knockBool = false;
			//And remove the force we added
			enemy.angularVelocity = 0f;
			enemy.velocity = Vector2.zero;
		}
	}

	//Set health for enemy
	public void setEHealth(int newHealth)
	{
		ehealth = newHealth;
		if (!showFlash) 
		{
			StartCoroutine("flashDamage");
		}
	}

	//Flash our sprite function
	public IEnumerator flashDamage()
	{
		showFlash = true;
		render.material.color = Color.red;
		yield return new WaitForSeconds(.1f);
		render.material.color = defaultColor;
			showFlash = false;
		
	}
}
