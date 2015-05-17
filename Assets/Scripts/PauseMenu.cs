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
		Debug.Log ("hi");
		//If we are not paused
		if (!paused) {
			//Get our key input
			if (Input.GetKeyDown (KeyCode.KeypadEnter) || Input.GetKeyDown ("return") || Input.GetKeyDown ("escape")) {
				//Pause the game
				paused = true;
				Time.timeScale = 0;
				pauseMenu.SetActive (true);
			}
		} 
		//if we are paused
		else 
		{
			if (Input.GetKeyDown (KeyCode.KeypadEnter) || Input.GetKeyDown ("return")) 
			{
				//resume the game
				paused = false;
				pauseMenu.SetActive (false);
				Time.timeScale = 1;
			}
			else if (Input.GetKeyDown("escape"))
			{
				//Quit the game
				Application.Quit();
			}
		}
	}
}