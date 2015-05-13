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
	private int elevel;

	//Our target to fight
	private Transform player;

	private Animator animator;   //Used to store a reference to the Player's animator component.

	//Frames until the enemy will atack
	public int attackFrames;
	private int totalFrames;

	//Knockback value
	public float knockWieght;
	
	// Use this for initialization
	void Start () 
	{

		//Get a component reference to the Player's animator component
		animator = GetComponent<Animator>();


		int playerLevel = Player.playerLevel;
		//Create our enemy based off our the player's current level
		elevel = playerLevel / 2;
		if (elevel <= 0)
			elevel = 1;
		ehealth = elevel * 5;

		//Set the mass of the rigid body to be really hgihg so they dont go flying
		enemy.mass = 10000;

		//Save the total amount of frames before we attack 
		totalFrames = attackFrames;

		//Go after our player!
		player = GameObject.Find("Person").transform;
	}
	
	//Called every frame
	void Update ()
	{
		//Move our player
		Move();
		
		//Attacks with our player (Check for a level up here as well)

		//Check if enemy is dead
		if (ehealth <= 0) 
		{
			//Destroy this enemy, possible display some animation first
			Destroy(gameObject);
		}
	}
	
	//Function to move our player
	void Move ()
	{
		//Get our speed according to our current level
		float superSpeed = emoveSpeed + (elevel / 15);
		
		//Get the position we want to move to, and go to it using move towards
		transform.position =  Vector2.MoveTowards(transform.position, player.position, superSpeed * Time.deltaTime);
	}

	//Knockback function for enemies
	public void knockBack(int direction, int amount)
	{
		//Knockback according to player direction
		if (direction == 0) 
		{
			//Down
			enemy.AddForce(new Vector2(0, -1000.0f * (amount / knockWieght)));
		} 
		else if (direction == 1) 
		{
				//Right
			enemy.AddForce(new Vector2( 1000.0f * (amount / knockWieght), 0));
		} 
		else if (direction == 2) 
		{
				//Up
			enemy.AddForce(new Vector2(0, 1000.0f * (amount / knockWieght)));
		} 
		else 
		{
				//KLeft
			enemy.AddForce(new Vector2( -1000.0f * (amount / knockWieght), 0));
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
