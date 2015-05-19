using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
	//Our enemy sprite
	public Rigidbody2D enemy;
	//Our enemy movepseed
	public float emoveSpeed = 0f;
	
	//Our player stats
	public int ehealth;
	//Static makes it available to other classes
	public int elevel;

	//Our target to fight
	private Player player;

	private Animator animator;   //Used to store a reference to the Player's animator component.

	//Frames until the enemy will atack
	public int attackFrames;
	private int totalFrames;

	//Knockback value
	private float knockForce;
	public int knockFrames;
	private int totalKnockFrames;
	private bool knockBool;

	//Our sounds
	private AudioSource hurt;

	//Our game manager
	GameManager gameManager;
	
	// Use this for initialization
	void Start () 
	{

		//Get a component reference to the Player's animator component
		animator = GetComponent<Animator>();


		int playerLevel = Player.playerLevel;
		//Create our enemy based off our the player's current level
		elevel = playerLevel / 2;
		if (elevel <= 0) {
			elevel = 1;
		}
		ehealth = elevel * 8;

		//Set the mass of the rigid body to be really high so they dont go flying
		enemy.mass = 10000;
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
			}
		} 
		else if(!gameManager.getGameStatus())
		{
			//Move our player
			Move ();
		}
		
		//Attacks with our player (Check for a level up here as well)

		//Check if enemy is dead
		if (ehealth <= 0) 
		{
			//Add experience to the player
			player.addEXP(elevel);
			//Decrease the number of enemies we have
			gameManager.minusEnemy();
			//Destroy this enemy, possible display some animation first
			Destroy(gameObject);
		}
	}
	
	//Function to move our player
	void Move ()
	{
		//Get our speed according to our current level
		float superSpeed = emoveSpeed + (elevel / 10);
		
		//Get the position we want to move to, and go to it using move towards
		transform.position =  Vector2.MoveTowards(transform.position, player.transform.position, superSpeed * Time.deltaTime);
	}

	//Knockback function for enemies
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
		knockForce = knockForce + (amount * 10000);
		if (knockForce > 1500000) 
		{
			knockForce = 1500000;
		}
		//Set knockback to true
		knockBool = true;
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
	}

	//Catch when we collide with enemy
	void OnCollisionStay2D(Collision2D collision) 
	{
			//Check if it is the player
			if(collision.gameObject.tag == "Player")
			{
			//Decrease the number of frames until we attack
				if(attackFrames > 0)
				{
				--attackFrames;
				}
			//attack the player
			else
			{
				//Set the attack trigger of the player's animation controller in order to play the player's attack animation.
				animator.SetTrigger ("EAttack");
				Player p = (Player) collision.gameObject.GetComponent("Player");
				int newHealth = p.getHealth() - elevel;
				p.setHealth(newHealth);

				//Reset attack frames
				attackFrames = totalFrames;
			}
			}
		
	}
}
