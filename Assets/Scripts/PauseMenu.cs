using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour 
{
	//Our UI
	public GameObject pauseMenu;

	//our boolean so if we are paused
	public static bool paused;

	//Our game manager
	GameManager gameManager;

	void Start()
	{
		//initialize our variables
		pauseMenu.SetActive (false);
		paused = false;
		//Get our gammaneger
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

	void Update()
	{
		//If we are not paused, and game is not over
		if (!paused && !gameManager.getGameStatus()) {
			//Get our key input
			if (Input.GetKeyDown (KeyCode.KeypadEnter) || Input.GetKeyDown ("return") || Input.GetKeyDown ("escape")) {
				//Pause the game
				paused = true;
				Time.timeScale = 0;
				pauseMenu.SetActive (true);
			}
		} 
		//if we are paused and game is not over
		else if(paused && !gameManager.getGameStatus())
		{
			if (Input.GetKeyDown (KeyCode.KeypadEnter) || Input.GetKeyDown ("return")) 
			{
				resumeGame();
			}
			else if (Input.GetKeyDown("escape"))
			{
				//Quit the game
				Application.Quit();
			}
		}
	}

	public void resumeGame()
	{
		//resume the game
		paused = false;
		pauseMenu.SetActive (false);
		Time.timeScale = 1;
	}
}