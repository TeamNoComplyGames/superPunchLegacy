﻿using UnityEngine;
using System.Collections;

public class StartInput : MonoBehaviour 
{
	//Our text
	public UnityEngine.UI.Text score;
	//Our select Sounds
	public AudioSource select;
	//Our select intro sound
	public AudioSource intro;
	// Use this for initialization
	void Start () 
	{
		//set up the highscore infor stuff
		//now get our save file
		SaveManager.loadSave();

		//Start our itro sound
		intro.Play();


		//now set the text of the score, placed in inspector, \t for tab
		//throw our values in a function to return a string with our desired zeroes
		score.text = "\t" + System.String.Format("{0:000}", SaveManager.getSaveLevel()) + "\t\t\t" + "10000000000";
	}
	
	// Update is called once per frame
	void Update () 
	{
		//If enter, start the game!
		if (Input.GetKeyDown (KeyCode.KeypadEnter) || Input.GetKeyDown ("return")) 
		{
			//Play the select sounds and then start the game
			select.Play();
			Application.LoadLevel("Game");
		}
		//if escape, quit
		else if (Input.GetKeyDown("escape"))
		{
			//Quit the game
			Application.Quit();
		}
	}
}
