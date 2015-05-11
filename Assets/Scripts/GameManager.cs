using UnityEngine;
using System.Collections;
using System.Collections.Generic;       //Allows us to use Lists.

public class GameManager : MonoBehaviour 
{

	//Boolean to determine if the game has begun
	private bool started;

	//Boolean to determine if the game is over
	private bool gameOver;

	//Rate (seconds) at which monsters should spawn
	public float spawnRate;

	//Our previous time to be stored for the spawn rate
	private float previousTime;

	//Our player object
	private GameObject user;

	//Our enemy prefab
	public GameObject[] enemies;
	public int numEnemies = 1;



	// Use this for initialization
	void Start () 
	{
		started = true;
		gameOver = false;

		//Get our player
		user = GameObject.Find ("Person");


		//Spawn an enemies
		invokeEnemies ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Spawn enemies every frame
		if (started) 
		{

					
		}
	}

	//Functiont o do our invoke repeating functions
	public void invokeEnemies ()
	{
		//Cancel all of our invokes
		CancelInvoke();

		//Now invoke our enemies
		InvokeRepeating("spawnEnemies", 0 , spawnRate / Player.playerLevel);
	}


	//Function to spawn enemies repeatedly
	private void spawnEnemies()
	{
		//We can spawn an enemy anywhere outside of the camera
		//Get ouyr player's position
		user = GameObject.Find("Person");
		Vector2 userPos = user.transform.position;
		
		//Now find an x and y coordinate that wouldnt be out of bounds the level, attaching this script to it's own object
		//It's position is X: 52, Y: -20 X is left lower, right higher, Y is top higher, bottom lower
		
		//Find an X And Y to spawn
		float enemyX = 0;
		float enemyY = 0;
		if(userPos.x > this.transform.position.x)
		{
			enemyX = Random.Range(2, 48);
		}
		//Less than or equal to
		else
		{
			enemyX = Random.Range(60, 98);
		}
		
		if(userPos.y > this.transform.position.y)
		{
			enemyY = Random.Range(-24, -44);
		}
		//Less than or equal to
		else
		{
			enemyY = Random.Range(1, -17);
		}
		
		//Now create a vector with our x and y
		Vector2 spawnPos = new Vector2(enemyX,enemyY);
		
		//Now spawn our enemy, our prefab, our spawn position, 0,0,0 rotation
		Debug.Log("Spawning!");

		//Now re-create our spawn rates
		//Get our enemy index
		int enemyIndex = Random.Range (0, (numEnemies - 1));
		Instantiate(enemies[0], spawnPos, Quaternion.identity); 

	}
}
