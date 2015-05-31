using UnityEngine;
using System.Collections;

public class StartInput : MonoBehaviour 
{
	//Our text
	public UnityEngine.UI.Text score;
	// Use this for initialization
	void Start () 
	{
		//set up the highscore infor stuff
		//now get our save file
		SaveManager.loadSave();


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
