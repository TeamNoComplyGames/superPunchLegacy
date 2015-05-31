using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;

public class PlayerSave
{
	//Our highest level
	int level;

	//Our highest score
	long score;

	//Our constructor
	public PlayerSave()
	{
		//set level and score to zero
		level = 0;
		score = 0;

		//Load the save
		loadSave();
	}

	public void loadSave()
	{

	}
}
