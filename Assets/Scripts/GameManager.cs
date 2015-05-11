using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{

	//Boolean to determine if the game has begun
	private bool started;

	//Boolean to determine if the game is over
	private bool gameOver;

	//Rate (seconds) at which monsters should spawn
	public int spawnRate;

	//Our previous time to be stored for the spawn rate
	private float previousTime;

	//Our player object
	private GameObject user;

	//Our enemy prefab
	public List<GameObject> prefabs;



	// Use this for initialization
	void Start () 
	{
		started = true;
		gameOver = false;

		//Get our player
		user = GameObject.Find ("Person");


		//Spawn an enemy

		//Record the previous time
		previousTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Spawn enemies every frame
		if (started) 
		{
			//if the time becomes greater than our algorithm, spawn and enemy
			if(Time.time > previousTime + (spawnRate / (Player.level / 2)))
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

				//Now spawn our enemy

					
			}
		}
	}
}
