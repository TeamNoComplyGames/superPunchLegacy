﻿using UnityEngine;
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
		Move();
		
		//Attacks with our player (Check for a level up here as well)
	}
	
	//Function to move our player
	void Move ()
	{
		//Create a vector to where we are moving
		//Vector2 movement = new Vector3(Player.player.position.x, Player.player.position.y); 
		//Get our speed according to our current level
		float superSpeed = emoveSpeed + (elevel / 15);
		
		//Move to that position
		//enemy.MovePosition(enemy.position + movement * superSpeed);
	}
}
