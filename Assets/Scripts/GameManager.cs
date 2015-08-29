using UnityEngine;
using System; //Allows us to catch exception
using System.Collections;

//This class controls all of the UI, and spawning and flow of the game
public class GameManager : MonoBehaviour 
{

	//Boolean to determine if the game has begun
	private bool started;

	//Boolean to determine if the game is over
	private bool gameOver;

	//Rate (seconds) at which monsters should spawn
	public float spawnRate;

	//Our previous time to be stored for the spawn rate
	private float previousTime;

	//Our player object
	private Player user;

	//Our enemy prefab
	public GameObject[] enemies;
	public GameObject[] bosses;
	private int bossesSpawned;
	private int numEnemies;
	//Suggest max enemies 50
	public int maxEnemies;
	//number of enemies spawned
	private int defeatedEnemies;
	//Total number of enemies spawned
	private int totalSpawnedEnemies;
	//umber of enemies before spawning a boss
	public int bossRate;
	private bool bossMode;

	//Our Maps
	public Sprite[] maps;
	private SpriteRenderer gameMap;

	//Our object prefabs
	private GameObject[][] mapObjects;
	public GameObject[] mapOneObjects;
	public GameObject[] mapTwoObjects;
	//Suggest max objects as 7
	public int maxObjects;

	//Our Hud
	private UnityEngine.UI.Text hud;
	//Our Buttons
	private UnityEngine.UI.Button restart;
	private UnityEngine.UI.Button quit;

	//Our score
	private int score;


	//Our background music
	private AudioSource bgFight;
	
	//Array pf things to say once you die
	String[] epitaph = {"Even the mighty fall", "Not even heroes live forever", "Legends are never forgotten"};



	// Use this for initialization
	void Start () 
	{
		//Scale our camera accordingly
		started = true;
		gameOver = false;

		//Set our time to normal speed
		Time.timeScale = 1;

		//Get our player
		user = GameObject.Find ("Person").GetComponent<Player>();

		//Get our Hud
		hud = GameObject.FindGameObjectWithTag ("PlayerHUD").GetComponent<UnityEngine.UI.Text> ();

		//Get our buttons
		restart = GameObject.FindGameObjectWithTag ("Restart").GetComponent<UnityEngine.UI.Button> ();
		quit = GameObject.FindGameObjectWithTag ("Quit").GetComponent<UnityEngine.UI.Button> ();
		//Hide our buttons
		restart.gameObject.SetActive(false);
		quit.gameObject.SetActive(false);

		//get our bg music
		bgFight = GameObject.Find ("GameSong").GetComponent<AudioSource> ();

		//Defeated enemies is one for score calculation at start
		defeatedEnemies = 1;
		//Total spawned enemies is one because we check for it to spawn enemies, and zero would get it stuck
		totalSpawnedEnemies = 1;

		//Set score to zero
		score = 0;
		bossesSpawned = 0;

		//Show our controls
		StartCoroutine("controlsFlash");

		//Get our map
		gameMap = GameObject.Find ("Map").GetComponent<SpriteRenderer>();
		//set it to the saved map
		gameMap.sprite = maps[SaveManager.getMap()];

		//Now set up our map objects
		mapObjects = new GameObject[2][];
		mapObjects [0] = mapOneObjects;
		mapObjects [1] = mapTwoObjects;

		//Spawn our objects
		genObjects ();

		//Spawn an enemies
		invokeEnemies ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Spawn enemies every frame
		if (started) {
			//Get the score for the player
			//Going to calculate by enemies defeated, level, and minutes passed
			score = (int) (user.getLevel () * (defeatedEnemies) * 100) + defeatedEnemies + user.getLevel();


			//Update our hud to player
			hud.text = ("Health: " + user.getHealth () + "\tLevel: " + user.getLevel () + "\tScore: " + score);

			//start the music! if it is not playing
			if(!bgFight.isPlaying)
			{
			bgFight.Play();
			bgFight.loop = true;
			}
		} 
		else if (gameOver) 
		{
			//First get our epitaph index
			int epitaphIndex = 0;
			if(user.getLevel() / 10 <= epitaph.Length - 1)
			{
				epitaphIndex = user.getLevel() / 10;
			}
			else
			{
				epitaphIndex = epitaph.Length - 1;
			}


			//Show our game over
			hud.text = ("GAMEOVER!!!" + "\n" + epitaph[epitaphIndex] + "\nHighest Level:" + user.getLevel()
			            + "\nHighest Score:" + score);

			//Show a button in the middle of the screen to restart
			restart.gameObject.SetActive(true);
			quit.gameObject.SetActive(true);

			//stop the music! if it is playing
			if(bgFight.isPlaying)
			{
				bgFight.Stop();
			}

			//Slow down the game Time
			Time.timeScale = 0.25f;

			//Try to save our highestlevel
			SaveManager.setHighLevel(user.getLevel());

			//Try to set our high score
			SaveManager.setHighScore(score);

			//Now save
			SaveManager.saveSave();

			//Get our key input
			if (Input.GetKey("escape"))
			{
				quitGame();
			}
			else if(Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey ("return"))
			{
				restartGame();
			}
		}
	}

	//Function to set gameover boolean
	public void setGameStatus(bool status)
	{
		gameOver = !status;
		started = status;
	}

	//Function to get gameover boolean
	public bool getGameStatus()
	{
		return gameOver;
	}

	//Function to increase/decrease num enemies
	public void plusEnemy()
	{
		++numEnemies;
		++totalSpawnedEnemies;
	}

	public void minusEnemy()
	{
		--numEnemies;

		//Since enemy is gone add to defeated enemies
		++defeatedEnemies;
	}

	//fucntion to get our total number of spawned enemies
	public int getTotalSpawned()
	{
		// never return zero, only 1 or the actual amount
		if (totalSpawnedEnemies > 0) {
			return totalSpawnedEnemies;
		} else {
			return 1;
		}
	}

	//Get and set boss mode
	public void setBossMode(bool mode)
	{
		bossMode = mode;
	}

	public bool getBossMode()
	{
		return bossMode;
	}

	//Get and set bosses spawned
	public int getBossesSpawned()
	{
		return bossesSpawned;
	}
	//Functiont o do our invoke repeating functions
	public void invokeEnemies ()
	{
		//Cancel all of our invokes
		CancelInvoke();

		//Now invoke our enemies
		float superRate = spawnRate / Player.playerLevel;
		InvokeRepeating("spawnEnemies", 0 , superRate);
	}

	//Function to generate our objects
	public void genObjects ()
	{
		//Array list to get our objects positions
		ArrayList spawnPositions = new ArrayList();

		//in a loop generate our objects a specified amount of time
		for (int i = 0; i < maxObjects; ++i) 
		{
			spawnPositions.Add(spawnObjects(spawnPositions));
		}

	}

	//Corotine to flash the controls
	//Function to flash level up color
	private IEnumerator controlsFlash()
	{
		//Our rate of flashing
		float flashRate = .75f;

		//Get our text
		UnityEngine.UI.Text controls = GameObject.FindGameObjectWithTag ("Controls").GetComponent<UnityEngine.UI.Text> ();

		//Flash our text, the time it is off is half the time
		controls.enabled = true;
		yield return new WaitForSeconds(flashRate);
		controls.enabled = false;
		yield return new WaitForSeconds (flashRate / 2);
		controls.enabled = true;
		yield return new WaitForSeconds (flashRate);
		controls.enabled = false;
		yield return new WaitForSeconds (flashRate / 2);
		controls.enabled = true;
		yield return new WaitForSeconds (flashRate);
		controls.enabled = false;
	}


	//Function to spawn enemies repeatedly
	private void spawnEnemies()
	{
		//Only do this if there aren't a max number of enemies
		if (numEnemies < maxEnemies && !bossMode) {


			//We can spawn an enemy anywhere outside of the camera
			//Get ouyr player's position
			user = GameObject.Find ("Person").GetComponent<Player> ();
			Vector2 userPos = user.transform.position;
		
			//Now find an x and y coordinate that wouldnt be out of bounds the level, attaching this script to it's own object
			//It's position is X: 52, Y: -20 X is left lower, right higher, Y is top higher, bottom lower


			//Find an X And Y to spawn
			float enemyX = 0;
			float enemyY = 0;

			//get a random direction
			float eDir = -1;
			//Our enemy spawn offset
			float sOffY = .9f;
			float sOffX = 1.4f;
			float boundsY = .7f;
			float boundsX = .4f;
			//Get a random number to slightly influence our off set
			float slight = UnityEngine.Random.Range(0.0f, 0.7f);
			//loop until we get a direction that works
			while(eDir == -1)
			{
				//Get our direction 0,1,2,3
				eDir = Mathf.Floor(UnityEngine.Random.Range(0, 4.0f));

				//Check what direction we got
				if(eDir == 0)
				{
					//Then confirm we can use that direction
					if(userPos.y < -boundsY)
					{
						eDir = -1;
					}
					else
					{
						//Use the direction, and add a slight change to our other coordinate
						if(userPos.x > 0)
						{
							enemyX = userPos.x - slight;
						}
						else
						{
							enemyX = userPos.x + slight;
						}
						enemyY = userPos.y - sOffY;
					}
				}
				else if(eDir == 1)
				{
					if(userPos.x > boundsX)
					{
						eDir = -1;
					}
					else
					{
						enemyX = userPos.x + sOffX;
						//Use the direction, and add a slight change to our other coordinate
						if(userPos.y > 0)
						{
							enemyY = userPos.y - slight;
						}
						else
						{
							enemyY = userPos.y + slight;
						}
					}
				}
				else if(eDir == 2)
				{
					if(userPos.y > boundsY)
					{
						eDir = -1;
					}
					else
					{
						//Use the direction, and add a slight change to our other coordinate
						if(userPos.x > 0)
						{
							enemyX = userPos.x - slight;
						}
						else
						{
							enemyX = userPos.x + slight;
						}
						enemyY = userPos.y + sOffY;
					}
				}
				else if(eDir == 3)
				{
					if(userPos.x < -boundsX)
					{
						eDir = -1;
					}
					else
					{
						enemyX = userPos.x - sOffX;
						//Use the direction, and add a slight change to our other coordinate
						if(userPos.y > 0)
						{
							enemyY = userPos.y - slight;
						}
						else
						{
							enemyY = userPos.y + slight;
						}
					}
				}
				else
				{
					//Keep looping 
					eDir = -1;
				}
			}

		
			//Now create a vector with our x and y
			Vector2 spawnPos = new Vector2 (enemyX, enemyY);

			//Now re-create our spawn rates
			//Get our enemy index
			int enemyIndex = (int)Mathf.Floor (UnityEngine.Random.Range (0, enemies.Length));

			//Try catch for index out of range
			try {

				//Check if we are about to spawn a boss
				if(totalSpawnedEnemies % bossRate == 0)
				{
					//now check if there aren't anymore enemies, at east less than 6
					if(numEnemies < 5)
					{
						//increase bosses spawned, and make sure it does not go over
						bossesSpawned++;

						//set boss mode to true
						bossMode = true;

						//create a copy of our gameobject
						Instantiate (bosses [(bossesSpawned % bosses.Length)], spawnPos, Quaternion.identity);
					}
				}
				else
				{
					//create a copy of our gameobject
					Instantiate (enemies [enemyIndex], spawnPos, Quaternion.identity);
				}
			} catch (IndexOutOfRangeException ex) {
				//Print our exception to the console
				print (ex);
			}

		}

	}

	//Function to spawn objects, simply copy pasta from spawn enemy
	private Vector2 spawnObjects(ArrayList positions)
	{
		//We can spawn an object anywhere outside of the camera

		//THe players starting posisiton is Vector3.zero

		//Position we are going to spawn objects to
		Vector2 spawnPos = Vector2.zero;


		//Boolean if the position is valid
		bool validPos = false;
			
		//Now find an x and y coordinate that wouldnt be out of bounds the level, attaching this script to it's own object
		//It's position is X: 52, Y: -20 X is laeft lower, right higher, Y is top higher, bottom lower

		//Loop until position is valid
		while (!validPos) 
		{
			//Find an X And Y to spawn
			float enemyX = 0;
			float enemyY = 0;
			
			//get a random direction
			float eDir = -1;
			//Our objects spawn offset
			float sOffY = .75f;
			float sOffX = 1.15f;

			//No longe rneed bounds since slight can not go larger than this
			//float boundsY = .7f;
			//float boundsX = .4f;

			//Get a random number to slightly influence our off set
			float slightY = UnityEngine.Random.Range (0.0f, 0.6f);
			float slightX = UnityEngine.Random.Range (0.0f, 0.7f);

			//Get a random number to subtract or add our slight
			float ranBool = UnityEngine.Random.Range (0.0f, 1.01f);
			//loop until we get a direction that works
			while (eDir == -1) 
			{
				//Get our direction 0,1,2,3
				eDir = Mathf.Floor (UnityEngine.Random.Range (0, 4));
				
				//Check what direction we got
				if (eDir == 0) 
				{
					//Use the direction, and add a slight change to our other coordinate
					if (ranBool > 0.5f) 
					{
						enemyX = slightX - sOffX;
					} 
					else 
					{
						enemyX = slightX + sOffX;
					}

					//extra cause it tends to hug the top
					enemyY = slightY - sOffY - .6f;
				} 
				else if (eDir == 1) {
					//Use the direction, and add a slight change to our other coordinate
					if (ranBool > 0.5f) 
					{
						enemyY = slightY - sOffY;
					} 
					else 
					{
						enemyY = slightY + sOffY;
					}

					enemyX = slightX + sOffX;
				} 
				else if (eDir == 2) {
					//Use the direction, and add a slight change to our other coordinate
					if (ranBool > 0.5f) 
					{
						enemyX = slightX - sOffX;
					} 
					else 
					{
						enemyX = slightX + sOffX;
					}

					enemyY = slightY + sOffY;
				} 
				else if (eDir == 3) 
				{
					//Use the direction, and add a slight change to our other coordinate
					if (ranBool > 0.5f) 
					{
						enemyY = slightY - sOffY;
					} 
					else 
					{
						enemyY = slightY + sOffY;
					}

					enemyX = slightX - sOffX;
				} 
				else 
				{
					//Keep looping 
					eDir = -1;
				}
			}
			
			//Now create a vector with our x and y
			spawnPos = new Vector2 (enemyX, enemyY);

			//Check beofore looping
			if(positions.Count == 0)
			{
				//Nothing to compare to, therefore it is valid
				validPos = true;
			}
			//Then we need to check it's vadility
			else
			{

				//Loop and check if position is valid to our other objects
				for(int i = 0; i < positions.Count; ++i)
				{
					//Check the distance from our current position and old ones
					if(Vector2.Distance(spawnPos, (Vector2) (positions[i])) > 0.45f)
					{
						//check if this is the last index of i
						if(i == positions.Count - 1)
						{
							//THen we have found a valid position
							validPos = true;
						}
					}
					else
					{
						//Break and start over
						validPos = false;
						i = positions.Count;
					}
				}
			}
			//THis will go back and check if we got a valid position, if we did, we break and
			//make the object!
		}
			
		//Now re-create our spawn rates
		//Get our enemy index
		int objectsIndex = (int)Mathf.Floor (UnityEngine.Random.Range (0, mapObjects[SaveManager.getMap()].Length));
		
		//Try catch for index out of range
		try {
			//create a copy of our gameobject
			Instantiate (mapObjects[SaveManager.getMap()][objectsIndex], spawnPos, Quaternion.identity);
		} catch (IndexOutOfRangeException ex) {
			//Print our exception to the console
			print (ex);
		}
		//Return spawn position
		return spawnPos;
		
	}

	public void restartGame()
	{
		//Restart the scene
		Application.LoadLevel ("Game");
	}

	public void quitGame()
	{
		Application.Quit ();
	}
}
