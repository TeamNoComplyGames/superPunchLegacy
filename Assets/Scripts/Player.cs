using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	//Our player sprite
	public Rigidbody2D player;
	//Our player movepseed
	public float moveSpeed = 0f;
	//Player health rate
	public int healthRate;
	//Player health regen rate
	public int healthRegenRate;
	int healthRegen;

	//Our player stats
	private int health;
	//Static makes it available to other classes
	public static int playerLevel;
	private int exp;

	//Game jucin'
	private int moveDec;

	private SpriteRenderer render; //Our sprite renderer to change our sprite color
	private bool showFlash;
	private Animator animator;   //Used to store a reference to the Player's animator component.

	//Boolean to check if attacking
	bool attacking;
	//Boolean to check if we are dodging
	bool dodging;
	//Counter for holding space to punch
	int holdPunch;
	//How long do they have to hold before punching
	public int holdDuration;

	//Our sounds 
	private AudioSource punch;
	private AudioSource dodge;
	private AudioSource levelUp;
	private AudioSource death;

	//Our game manager
	GameManager gameManager;
	//our camera Script
	Bounded2DCamera actionCamera;

	// Use this for initialization
	void Start () 
	{
		//Get a component reference to the Player's animator component
		animator = GetComponent<Animator>();
		render = GetComponent<SpriteRenderer>();
		showFlash = false;

		//Set our default values
		playerLevel = 1;
		health = playerLevel * healthRate;
		exp = 0;
		attacking = false;
		dodging = false;
		moveDec = 1;
		holdPunch = 0;
		healthRegen = 0;

		//Get our sounds
		punch = GameObject.Find ("Punch").GetComponent<AudioSource> ();
		dodge = GameObject.Find ("Dodge").GetComponent<AudioSource> ();
		levelUp = GameObject.Find ("LevelUp").GetComponent<AudioSource> ();
		death = GameObject.Find ("Death").GetComponent<AudioSource> ();

		//Default looking down
		animator.SetInteger("Direction", 0);



		//Get our gammaneger
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		//Get our camera script
		actionCamera = Camera.main.GetComponent<Bounded2DCamera>();
	}

	//Called every frame
	void Update ()
	{

		//check if dead, allow movement if alive
		if (health <= 0) 
		{
			//make our player object invisible
			//possible display some animation first
			//Renderer r = (Renderer) gameObject.GetComponent("SpriteRenderer");
			//r.enabled = false;
			//No longer turning invisible, just looping death animation
			//play our death animation
			if(!animator.GetBool("Death"))
			{
				animator.SetTrigger("DeathTrigger");
				animator.SetBool("Death", true);
				//play the death sound
				if(!death.isPlaying)
				{
					death.Play();
				}
			}
			//Set our gameover text
			gameManager.setGameStatus(false);
		} 
		else 
		{
			//Move our player
			Move();
			
			//Attacks with our player (Check for a level up here as well), only attack if not dodging
			if (Input.GetKey ("space") && 
			    !dodging &&
			    !animator.GetCurrentAnimatorStateInfo(0).IsName("Forward Dodge") &&
			    !animator.GetCurrentAnimatorStateInfo(0).IsName("Right Dodge") &&
			    !animator.GetCurrentAnimatorStateInfo(0).IsName("Back Dodge") &&
			    !animator.GetCurrentAnimatorStateInfo(0).IsName("Left Dodge")) {
				//Now since we are allowing holding space to punch we gotta count for it
				if(!attacking && holdPunch % holdDuration == 0)
				{
					//Set hold punch to zero
					holdPunch = 0;
					//Attacking working great
					StopCoroutine("Attack");
					StartCoroutine ("Attack");
				}

				//Increase hold punch
				holdPunch++;
			}

			//If they stop holding space, set holdpunch back to zero
			if (Input.GetKeyUp ("space")) {
				//Set hold punch to zero
				holdPunch = 0;
			}

			//Dodging, cant dodge if attacking
			if((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) || 
			    Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl) 
			    || Input.GetKeyDown(KeyCode.B)) && !attacking &&
				!animator.GetCurrentAnimatorStateInfo(0).IsName("ForwardAttack") &&
				!animator.GetCurrentAnimatorStateInfo(0).IsName("RightAttack") &&
				!animator.GetCurrentAnimatorStateInfo(0).IsName("BackAttack") &&
				!animator.GetCurrentAnimatorStateInfo(0).IsName("LeftAttack")) {
				//Start the dodging coroutine
				StopCoroutine("Dodge");
				StartCoroutine ("Dodge");
			}

			//Increase health Regen if we are not attacking (doing it halo style, so attacking wont stop you), or being hit/levling up (Check if we are flashing!)
			if(!showFlash)
			{
				healthRegen++;
			}
			//And if we do, reset it to zero
			else
			{
				healthRegen = 0;
			}

			//Now check if we are attacking for health regen
			if(healthRegen >= healthRegenRate)
			{
				//Set health regen to zero
				healthRegen = 0;

				//get the level's max health
				int levelHealth = playerLevel * healthRate;

				//increase health by .5%
				int hpUp = (int)(levelHealth * .05);

				//Check if it is less than one
				if(hpUp < 1)
				{
					hpUp = 1;
				}

				//We don't want to exceed our maximum health
				if(hpUp + health > levelHealth)
				{
					//health is equal to full health
					health = playerLevel * healthRate;
				}
				else
				{
					//INcrease the health!
					health = health + hpUp;
				}
			}

		}

		//Check for levelUp
		int requiredXP = playerLevel * playerLevel / 2;
		if(requiredXP < 1)
		{
			requiredXP = 1;
		}
		if (exp >= requiredXP) 
		{
			//Reset/increase stats
			exp = 0;
			++playerLevel;
			health = playerLevel * healthRate;
			gameManager.invokeEnemies();
			//Play our sound
			levelUp.Play();

			//Increase our camera shake if multiple of 2
			if(playerLevel % 2 == 0)
			{
				actionCamera.incShake(.002f);
				actionCamera.declerp(.2f);
			}

			//Increase our player scale
			//Removing this because it will mess up 
			//our camera aspect ratio
			/*
			if(transform.localScale.x < .9f)
			{
				//transform.localScale += new Vector3(0.01F, 0.01F, 0);
			}
			*/

			//Show our text, stop if already showing
			StopCoroutine("levelFlash");
			StartCoroutine("levelFlash");
		}
	}

	//Function to move our player
	void Move ()
	{
		//Get our input
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical"); 

		//Also check to make sure we stay that direction when not moving, so check that we are
		if (h != 0 || v != 0) {
			//tell the animator to stop idling
			animator.SetBool("Moving", true);
		
			//animate to the direction we are going to move
			//Find the greatest absolute value to get most promenint direction
			/*
		 * 		2
		 * 3		1
		 * 		0
		 * */
			if (Mathf.Abs (h * 100) > Mathf.Abs (v * 100)) {
				if (h > 0) {
					animator.SetInteger ("Direction", 1);
				} else {
					animator.SetInteger ("Direction", 3);
				}
			} else {
				if (v > 0) {
					animator.SetInteger ("Direction", 2);
				} else {
					animator.SetInteger ("Direction", 0);
				}
			}
			//Create a vector to where we are moving
			Vector2 movement = new Vector2 (h, v); 

			//When attacking start a slow movemnt coroutine
			if (attacking) {
				//Attacking working great
				StopCoroutine ("slowMoving");
				StartCoroutine ("slowMoving");
			}


			//Get our speed according to our current level
			float levelSpeed = (float)playerLevel / 400;
		

			//Get our actual speed
			float superSpeed = levelSpeed + (moveSpeed / 10) / moveDec;
			//Can't go above .5 though
			if (superSpeed > .032f) {
				superSpeed = .032f;
			}

			//Move to that position
			player.MovePosition (player.position + movement * superSpeed);
		}
		//then we are not moving
		else {
			//Set our position to our current position, so we dont drift away
			player.MovePosition (player.position);
			//tell the animator we are no longer moving
			animator.SetBool("Moving", false);
		}
	}

	//Function to slow movement for a certain amount of time
	IEnumerator slowMoving()
	{
		//Double move decrement, wait for half of a second, and then set back to normal
		moveDec = playerLevel * playerLevel * 2;
		yield return new WaitForSeconds(.5f);
		moveDec = 1;
	}

	//Function to catch attack commands
	IEnumerator Attack()
	{
			//Set attacking to true
			attacking = true;
			//Set the attack trigger of the player's animation controller in order to play the player's attack animation.
			animator.SetTrigger ("Attack");
			//Play the sounds
			punch.Play ();

		//Check what direction we are moving, and slight move that way when attacking
		int dir = animator.GetInteger("Direction");
		float moveAmount = .01f;
		if(dir == 0)
		{
			gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - moveAmount, 0);
		}
		else if(dir == 1)
		{
			gameObject.transform.position = new Vector3(gameObject.transform.position.x + moveAmount, gameObject.transform.position.y, 0);
		}
		else if(dir == 2)
		{
			gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + moveAmount, 0);
		}
		else
		{
			gameObject.transform.position = new Vector3(gameObject.transform.position.x - moveAmount, gameObject.transform.position.y, 0);
		}
			//Let the frame finish
		yield return null;
		//set attacking to false
		attacking = false;

	}

	//Function to catch dodge commands
	IEnumerator Dodge()
	{
		//Set dodging to true
		dodging = true;
		//Set the attack trigger of the player's animation controller in order to play the player's attack animation.
		animator.SetTrigger ("Dodge");
		//Play the sounds
		dodge.Play ();

		//Set the player to the dodging layer
		gameObject.layer = 9;
		
		//Check what direction we are moving, and slightly move that way when dodging
		int dir = animator.GetInteger("Direction");
		float moveAmount = .02f;
		if(dir == 0)
		{
			gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - moveAmount, 0);
		}
		else if(dir == 1)
		{
			gameObject.transform.position = new Vector3(gameObject.transform.position.x + moveAmount, gameObject.transform.position.y, 0);
		}
		else if(dir == 2)
		{
			gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + moveAmount, 0);
		}
		else
		{
			gameObject.transform.position = new Vector3(gameObject.transform.position.x - moveAmount, gameObject.transform.position.y, 0);
		}
		//Let a couple of frames finish
		for (int i = 0; i < 4; ++i) {
			yield return null;
		}

		//Set the player to the colliding layer
		gameObject.layer = 8;
		//set dodging to false
		dodging = false;
		
	}

	//Catch when we collide with enemy
	void OnCollisionStay2D(Collision2D collision) 
	{
		//check if we are attacking
		if (attacking) 
		{
			//Check if it is an enemy
			if(collision.gameObject.tag == "Enemy")
			{
				//Check if the enemy is in the direction we are facing
				int dir = animator.GetInteger("Direction");

				//Get our x and y
				float playerX = gameObject.transform.position.x;
				float playerY = gameObject.transform.position.y;
				float enemyX = collision.gameObject.transform.position.x;
				float enemyY = collision.gameObject.transform.position.y;
				//Our window for our punch range
				float window = .15f;

				//Deal damage if we are facing the right direction, and they are not too above or around us
				if((dir == 1 && enemyX >= playerX && enemyY <= (playerY + window) && enemyY >= (playerY - window)) ||
				   (dir == 3 && enemyX <= playerX && enemyY <= (playerY + window) && enemyY >= (playerY - window)) ||
				   (dir == 2 && enemyY >= playerY && enemyX <= (playerX + window) && enemyX >= (playerX - window)) ||
				   (dir == 0 && enemyY <= playerY & enemyX <= (playerX + window) && enemyX >= (playerX - window)))
				{
				//Get the enemy and decrease it's health
				Enemy e = (Enemy) collision.gameObject.GetComponent("Enemy");
				//Do damage
					e.setEHealth(e.ehealth - playerLevel);

					//Now knockback
					e.knockBack(animator.GetInteger("Direction"), playerLevel);

					//Shake the screen
					actionCamera.startShake();

					//Add slight impact pause
					actionCamera.startImpact();
				}
			}
		}

	}

	//Get/set funtion for health
	public int getHealth()
	{
		return health;
	}

	//Get/set funtion for health
	public void setHealth(int newHealth)
	{
		health = newHealth;
		if (!showFlash && health > 0) 
		{
			StartCoroutine("flashDamage");
		}
	}

	//Get funtion for level
	public int getLevel()
	{
		return playerLevel;
	}

	//Function for if dodging
	public bool isDodging()
	{
		return dodging;
	}

	//Function to Add exp to our player
	public void addEXP(int add)
	{
		exp = exp + add;
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

	//Function to flash level up color
	private IEnumerator levelFlash()
	{
		showFlash = true;
		render.material.color = Color.green;
		yield return new WaitForSeconds(.2f);
		render.material.color = Color.white;
		yield return new WaitForSeconds (.2f);
		render.material.color = Color.green;
		yield return new WaitForSeconds(.2f);
		render.material.color = Color.white;
		yield return new WaitForSeconds(.2f);
		render.material.color = Color.green;
		yield return new WaitForSeconds(.2f);
		render.material.color = Color.white;
		yield return new WaitForSeconds(.2f);
		render.material.color = Color.green;
		yield return new WaitForSeconds(.2f);
		render.material.color = Color.white;
		showFlash = false;
	}
}