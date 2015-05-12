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
	
	// Use this for initialization
	void Start () 
	{
		int playerLevel = Player.playerLevel;
		//Create our enemy based off our the player's current level
		elevel = playerLevel / 2;
		if (elevel <= 0)
			elevel = 1;
		ehealth = elevel * 2;

		//Set the mass of the rigid body to be really hgihg so they dont go flying
		enemy.mass = 10000;

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
}
