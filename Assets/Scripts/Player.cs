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

	private Animator animator;   //Used to store a reference to the Player's animator component.

	// Use this for initialization
	void Start () 
	{
		//Get a component reference to the Player's animator component
		animator = GetComponent<Animator>();

		//Set our default values
		health = 5;
		playerLevel = 1;
		exp = 0;
	}

	//Called every frame
	void Update ()
	{
		//Move our player
		Move();

		//Attacks with our player (Check for a level up here as well)
		Attack ();

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
		float superSpeed = moveSpeed + (playerLevel / 10);

		//Move to that position
		player.MovePosition(player.position + movement * superSpeed);
	}

	//Function to catch attack commands
	void Attack()
	{
		if (Input.GetKeyDown ("space")) 
		{
			//Set the attack trigger of the player's animation controller in order to play the player's attack animation.
			animator.SetTrigger ("Attack");
		}
	}
}