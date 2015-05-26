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
	private int numEnemies;
	//Suggest max enemies 50
	public int maxEnemies;

	//Our Hud
	private UnityEngine.UI.Text hud;
	//Our Buttons
	private UnityEngine.UI.Button restart;
	private UnityEngine.UI.Button quit;


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


		//Spawn an enemies
		invokeEnemies ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Spawn enemies every frame
		if (started) {
			//Update our hud to player
			hud.text = ("Health: " + user.getHealth () + "\nLevel: " + user.getLevel ());

			//start the music! if it is not playing
			if(!bgFight.isPlaying)
			{
			bgFight.Play();
			bgFight.loop = true;
			}
		} 
		else if (gameOver) 
		{
			//Show our game over
			hud.text = ("GAMEOVER!!!" + "\n" + epitaph[user.getLevel()/10] + "\nHighest Level:" + user.getLevel());

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
	}

	public void minusEnemy()
	{
		--numEnemies;
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


	//Function to spawn enemies repeatedly
	private void spawnEnemies()
	{
		//Only do this if there aren't a max number of enemies
		if (numEnemies < maxEnemies) {
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


			/* OLD SPAWNING METHOD
			//Get a random boolean
			bool ran;
			if(UnityEngine.Random.Range(0, 1.0f) > .5f)
			{
				ran = true;
			}
			else
			{
				ran = false;
			}

			//Get our X, check if it is too great for surrounding the character, then surround them
			if(userPos.x > 1.0f)
			{
				enemyX = UnityEngine.Random.Range (userPos.x - .9f, -1.9f);
			}
			else if(userPos.x < -1.0f)
			{
				enemyX = UnityEngine.Random.Range (.9f + userPos.x, 1.9f);
			}
			else
			{
				if(ran)
				{
					enemyX = UnityEngine.Random.Range (.9f + userPos.x, 1.9f);
				}
				else
				{
					enemyX = UnityEngine.Random.Range (userPos.x - .9f, -1.9f);
				}
			}

			//Get our y the same way
			if(userPos.y > .9f)
			{
				enemyY = UnityEngine.Random.Range (userPos.y - .8f, -1.8f);
			}
			else if(userPos.y < -.9f)
			{
				enemyY = UnityEngine.Random.Range (.8f + userPos.y, 1.8f);
			}
			else
			{
				if(!ran)
				{
					enemyY = UnityEngine.Random.Range (.8f + userPos.y, 1.8f);
				}
				else
				{
					enemyY = UnityEngine.Random.Range (userPos.y - .8f, -1.8f);
				}
			}
			*/
		
			//Now create a vector with our x and y
			Vector2 spawnPos = new Vector2 (enemyX, enemyY);

			//Now re-create our spawn rates
			//Get our enemy index
			int enemyIndex = (int)Mathf.Floor (UnityEngine.Random.Range (0, enemies.Length - 1));

			//Try catch for index out of range
			try {
				//create a copy of our gameobject
				Instantiate (enemies [enemyIndex], spawnPos, Quaternion.identity);
			} catch (IndexOutOfRangeException ex) {
				//Print our exception to the console
				print (ex);
			}

		}

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
