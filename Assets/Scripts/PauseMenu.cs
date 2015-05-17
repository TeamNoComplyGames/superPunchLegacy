using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour 
{
	//Our UI
	public GameObject pauseMenu;

	//our boolean so if we are paused
	private bool paused;

	void Start()
	{
		//initialize our variables
		pauseMenu.SetActive (false);
		paused = false;
	}

	void Update()
	{
		//If we are not paused
		if (!paused) {
			//Get our key input
			if (Input.GetKey (KeyCode.KeypadEnter) || Input.GetKey ("return") || Input.GetKey ("escape")) {
				//Pause the game
				paused = true;
				Time.timeScale = 0;
			}
		} 
		//if we are paused
		else 
		{
			if (Input.GetKey (KeyCode.KeypadEnter) || Input.GetKey ("return")) 
			{
				//resume the game
				paused = false;
				Time.timeScale = 1;
			}
			else if (Input.GetKey("escape"))
			{
				//Quit the game
				Application.Quit();
			}
		}
	}
}