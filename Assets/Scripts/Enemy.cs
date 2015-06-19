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
	
	//Our player stats
	public int ehealth;
	//Static makes it available to other classes
	public int elevel;
	//Boolean if our enemy is dead
	private bool dead;
	//Boolean if we are exploding
	private bool exploding;

	//Our target to fight
	private Player player;

	private SpriteRenderer render; //Our sprite renderer to change our sprite color
	private bool showFlash;
	private Animator animator;   //Used to store a reference to the Player's animator component.

	//Frames until the enemy will atack
	public int attackFrames;
	private int totalFrames;

	//boolean if colliding with player
	private bool playerCollide;

	//Knockback value
	private float knockForce;
	public int knockFrames;
	private int totalKnockFrames;
	private bool knockBool;

	//Our sounds
	private AudioSource hurt;

	//Our game manager
	GameManager gameManager;

	//our camera Script
	Bounded2DCamera cameraShake;
	
	// Use this for initialization
	void Start () 
	{

		//Get a component reference to the Player's animator component
		render = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
		showFlash = false;

		//Enemy is not dead
		dead = false;

		//Enemy is not colliding with player
		playerCollide = false;


		int playerLevel = Player.playerLevel;
		//Create our enemy based off our the player's current level
		elevel = playerLevel / 2;
		if (elevel <= 0) {
			elevel = 1;
		}
		ehealth = (int) (elevel * 2.5f);

		//Set the mass of the rigid body to be really high so they dont go flying
		defaultMass = enemy.mass;
		//Our knock force so we do go flying haha
		knockForce = 100000;

		//Save the total amount of frames before we attack 
		totalFrames = attackFrames;
		totalKnockFrames = knockFrames;
		knockBool = false;

		//Get our sounds
		hurt = GameObject.Find ("Hurt").GetComponent<AudioSource> ();

		//Get our gammaneger
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		//Get our camera script
		cameraShake = Camera.main.GetComponent<Bounded2DCamera>();

		//Increase our number of enemies
		gameManager.plusEnemy();


		//Go after our player!
		player = GameObject.Find("Person").GetComponent<Player>();
	}
	
	//Called every frame
	void Update ()
	{
		//If we are being knockbacked, knock back for so many frames
		if (knockBool) 
		{
			if (knockFrames > 0) 
			{
				--knockFrames;
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
		
		//Attacks with our player (Check for a level up here as well)

		//Check if enemy is dead
		if (ehealth <= 0 && !dead) 
		{
			//Get an explosion chance
			int exChance = Random.Range(0, 20);
			//Add experience to the player
			player.addEXP(elevel);
			//Decrease the number of enemies we have
			gameManager.minusEnemy();

			//Destroy this enemy, possible display some animation first
			//Destroy(gameObject);


			//Now check explosion chance
			if(exChance > 19)
			{
				//EXPLODEEEE

				//Call our explosion coroutine
				StartCoroutine("explode");
			}
			else
			{
				//Move our enemy out of the way and play the death animation
				animator.SetBool("Death", true);
				//and remove box collider
				GetComponent<BoxCollider2D>().enabled = false;
			}

			//Set death boolean to true
			dead = true;

		}
	}
	
	//Function to move our player
	void Move ()
	{
		//Get our speed according to our current level
		float superSpeed = emoveSpeed + (elevel / 10);

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

		//increase enemy mass so it'll runthrough other enemies
		enemy.mass = defaultMass/2;
	}

	//Knockback function for enemies, with velocity, not currently used
	public void knockBack(Vector2 velocity, int amount)
	{
		//Knockback according to velocity
		enemy.AddForce(velocity);
		
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
	}

	//Our explosion coroutine
	IEnumerator explode()
	{
		//First set exploding to true
		exploding = true;
	}

	//Catch when we collide with something
	void OnCollisionEnter2D(Collision2D collision) 
	{
			//Check if it is awall
			if(collision.gameObject.tag == "Wall")
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
			}
		//Check if it is another enemy, mass will lower for pushing one another
		else if(collision.gameObject.tag == "Enemy")
		{
			//Check if we were in knockback
			if(knockBool)
			{
			//Lose some health, this is only called if they were being knock backed
			--ehealth;

			//Knockback the enemy

			//Get the enemy
			Enemy e = (Enemy) collision.gameObject.GetComponent("Enemy");

			//lower enemy mass
			e.enemy.mass = defaultMass / 3;
			}
			//Check if they were exploding
			if(collision.gameObject.GetComponent<Enemy>().exploding)
			{
				//Lose some health
				ehealth = (ehealth / 2) - 1;
			}

		}

	}

	//Catch when we collide with enemy
	void OnCollisionStay2D(Collision2D collision) 
	{
			//Check if it is the player
			if(collision.gameObject.tag == "Player")
			{
			//Set player collide to true
			playerCollide = true;


			//Decrease the number of frames until we attack
				if(attackFrames > 0)
				{
				--attackFrames;
				}
			else if(dead)
			{
				//Do nothing ig dead
			}
			//attack the player
			else
			{
				//Set the attack trigger of the player's animation controller in order to play the player's attack animation.
				animator.SetTrigger ("Attack");
				Player p = (Player) collision.gameObject.GetComponent("Player");
				//Now using an int to calulate our damage before we apply to health
				int damage = (int)(elevel / 1.5);
				if(damage < 1)
				{
					damage = 1;
				}
				int newHealth = p.getHealth() - damage;
				p.setHealth(newHealth);

				//Play the sound of hurt, only if the game is still on
				if(!gameManager.getGameStatus())
				{
					hurt.Play();
				}

				//Shake the screen
				cameraShake.startShake();

				//impact pause from the player
				p.startImpact();

				//Reset attack frames
				attackFrames = totalFrames;
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
		render.material.color = Color.white;
			showFlash = false;
		
	}
}
