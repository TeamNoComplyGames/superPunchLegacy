using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlayerSave
{
	//our level
	int level;
	//our score
	int score;

	//our constructor
	public PlayerSave()
	{
		level = 0;
		score = 0;
	}

	//Set and get methods
	public void setLevel(int lvl)
	{
		level = lvl;
	}
	
	public int getLevel()
	{
		return level;
	}

	public void setScore(int s)
	{
		score = s;
	}

	public int getScore()
	{
		return score;
	}


}
