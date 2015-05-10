using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
	//Our enemy sprite
	public Rigidbody2D enemy;
	//Our enemy movepseed
	public float emoveSpeed = 0f;
	
	//Our player stats
	private int ehealth;
	//Static makes it available to other classes
	private int elevel;
	
	// Use this for initialization
	void Start () 
	{
		int playerLevel = Player.level;
		//Create our enemy based off our the player's current level
		elevel = playerLevel / 2;
		if (elevel <= 0)
			elevel = 1;
		ehealth = elevel * 2;
	}
	
	//Called every frame
	void Update ()
	{
		//Move our player
		//Move();
		
		//Attacks with our player (Check for a level up here as well)
	}
	
	//Function to move our player
	void Move ()
	{
		//Create a vector to where we are moving;
		Vector2 playerPos = GameObject.Find ("Person").transform.position;

		//Get our h and v
		float h = playerPos.x;
		float v = playerPos.y;

		while (h > 1 && h < -1) {
			h = h / 10;
		}

		while (v > 1 && v < -1) {
			v = v / 10;
		}

		Vector2 movement = new Vector2(h, v); 
		//Get our speed according to our current level
		float superSpeed = emoveSpeed + (elevel / 15);
		
		//Move to that position
		enemy.MovePosition(enemy.position + movement * superSpeed);
	}
}
