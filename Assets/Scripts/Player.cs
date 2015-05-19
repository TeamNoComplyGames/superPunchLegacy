using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	//Our player sprite
	public Rigidbody2D player;
	//Our player movepseed
	public float moveSpeed = 0f;

	//Our player stats
	private int health;
	//Static makes it available to other classes
	public static int playerLevel;
	private int exp;

	private SpriteRenderer render; //Our sprite renderer to change our sprite color
	private bool showFlash;
	private Animator animator;   //Used to store a reference to the Player's animator component.

	//Boolean to check if attacking
	bool attacking;

	//Our sounds 
	private AudioSource punch;
	private AudioSource levelUp;
	private AudioSource death;

	//Our game manager
	GameManager gameManager;

	// Use this for initialization
	void Start () 
	{
		//Get a component reference to the Player's animator component
		animator = GetComponent<Animator>();
		render = GetComponent<SpriteRenderer>();
		showFlash = false;

		//Set our default values
		health = 5;
		playerLevel = 1;
		exp = 0;
		attacking = false;

		//Get our sounds
		punch = GameObject.Find ("Punch").GetComponent<AudioSource> ();
		levelUp = GameObject.Find ("LevelUp").GetComponent<AudioSource> ();
		death = GameObject.Find ("Death").GetComponent<AudioSource> ();


		//Get our gammaneger
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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
			
			//Attacks with our player (Check for a level up here as well)
			if (Input.GetKeyDown ("space")) {
				if(!attacking)
				{
					//Attacking working great
					StartCoroutine ("Attack");
				}
			}
		}

		//Check for levelUp
		if (exp >= playerLevel * playerLevel) 
		{
			//Reset/increase stats
			exp = 0;
			++playerLevel;
			health = playerLevel * 5;
			gameManager.invokeEnemies();
			//Play our sound
			levelUp.Play();
		}
	}

	//Function to move our player
	void Move ()
	{
		//Get our input
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical"); 

		//animate to the direction we are going to move
		//Find the greatest absolute value to get most promenint direction
		/*
		 * 		2
		 * 3		1
		 * 		0
		 * */
		if (Mathf.Abs (h * 100) > Mathf.Abs (v * 100)) 
		{
			if(h > 0)
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
			if(v > 0)
			{
				animator.SetInteger("Direction", 2);
			}
			else
			{
				animator.SetInteger("Direction", 0);
			}
		}
		//Create a vector to where we are moving
		Vector2 movement = new Vector2(h, v); 
		//Get our speed according to our current level
		float levelSpeed = (float) playerLevel / 400;
		float superSpeed = levelSpeed + (moveSpeed / 10);
		//Can't go above .5 though
		if (superSpeed > .032f) 
		{
			superSpeed = .032f;
		}

		//Move to that position
		player.MovePosition(player.position + movement * superSpeed);
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
			yield return new WaitForSeconds (0.1f);
			attacking = false;
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

				//Deal damage if we are facing the right direction
				if((dir == 1 && collision.gameObject.transform.position.x >= gameObject.transform.position.x) ||
				   (dir == 3 && collision.gameObject.transform.position.x <= gameObject.transform.position.x) ||
				   (dir == 2 && collision.gameObject.transform.position.y >= gameObject.transform.position.y) ||
				   (dir == 0 && collision.gameObject.transform.position.y <= gameObject.transform.position.y))
				{
				//Get the enemy and decrease it's health
				Enemy e = (Enemy) collision.gameObject.GetComponent("Enemy");
				//Do damage
					e.setEHealth(e.ehealth - playerLevel);

				//Now knockback
				e.knockBack(animator.GetInteger("Direction"), playerLevel);
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
}