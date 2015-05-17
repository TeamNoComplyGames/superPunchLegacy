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
		started = true;
		gameOver = false;

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
			if (userPos.x > this.transform.position.x) {
				enemyX = UnityEngine.Random.Range (1.1f, 2.2f);
			}
		//Less than or equal to
		else {
				enemyX = UnityEngine.Random.Range (-1.1f, -2.2f);
			}
		
			if (userPos.y > this.transform.position.y) {
				enemyY = UnityEngine.Random.Range (-1.1f, -2.0f);
			}
		//Less than or equal to
		else {
				enemyY = UnityEngine.Random.Range (1.1f, 2.0f);
			}
		
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
		Application.LoadLevel (0);
	}

	public void quitGame()
	{
		Application.Quit ();
	}
}
