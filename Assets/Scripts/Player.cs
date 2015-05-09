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
	public static int level;
	private int exp;

	// Use this for initialization
	void Start () 
	{
		//Set our default values
		health = 5;
		level = 1;
		exp = 0;
	}

	//Called every frame
	void Update ()
	{
		//Move our player
		Move();

		//Attacks with our player (Check for a level up here as well)
	}

	//Function to move our player
	void Move ()
	{
		//Get our input
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical"); 
		//Create a vector to where we are moving
		Vector2 movement = new Vector3(h, v); 
		//Get our speed according to our current level
		float superSpeed = moveSpeed + (level / 10);

		//Move to that position
		player.MovePosition(player.position + movement * superSpeed);
	}
}