using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;

public static class SaveManager
{
	//Our save file
	public static PlayerSave save = new PlayerSave();

	//load our file
	public static void loadSave()
	{
		if(File.Exists(Application.persistentDataPath + "/punch.legacy")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/punch.legacy", FileMode.Open);
			save = (PlayerSave)bf.Deserialize(file);
			file.Close();
		}
	}

	//save our file
	public static void saveSave() {
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/punch.legacy");
		bf.Serialize(file, save);
		file.Close();
	}

	//Set and get methods
	public static void setHighLevel(int lvl)
	{
		if(lvl > getSaveLevel())
		{
			save.setLevel(lvl);
		}
	}
	
	public static int getSaveLevel()
	{
		return save.getLevel();
	}

	public static void setHighScore(int s)
	{
		if(s > getSaveScore())
		{
			save.setScore(s);
		}
	}

	public static int getSaveScore()
	{
		return save.getScore();
	}

	public static void setMap(int m)
	{
		save.setMapIndex (m);
	}

	public static int getMap()
	{
		return save.getMapIndex ();
	}

	public static void setChar(int c)
	{
		save.setCharIndex (c);
	}

	public static int getChar()
	{
		return save.getCharIndex ();
	}
}
