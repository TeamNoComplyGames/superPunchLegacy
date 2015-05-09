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



	// Use this for initialization
	void Start () 
	{
		started = true;
		gameOver = false;


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
					
			}
		}
	}
}
