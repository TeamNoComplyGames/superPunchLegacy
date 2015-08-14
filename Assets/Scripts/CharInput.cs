using UnityEngine;
using System.Collections;

public class CharInput : MonoBehaviour {

	//Our array of char images
	public Sprite[] chars;

	//Array of chars names, should correspond with chars, and stats
	public string[] charNames;
	public string[] charStats;

	//Our char image
	private UnityEngine.UI.Image charImage;

	//Our char name
	private UnityEngine.UI.Text charText;

	//our char stats
	private UnityEngine.UI.Text charStat;

	//Our arrows
	private UnityEngine.UI.Text rightArrow;
	private UnityEngine.UI.Text leftArrow;

	//our current index
	int currentIndex;

	//oUR SOUNDS
	public AudioSource select;
	public AudioSource hurt;
	public AudioSource punch;

	// Use this for initialization
	void Start () {

		//Set our char image
		charImage = GameObject.FindGameObjectWithTag ("HUDImage").GetComponent<UnityEngine.UI.Image> ();

		//Set our charText
		charText = GameObject.FindGameObjectWithTag ("PlayerHUD").GetComponent<UnityEngine.UI.Text> ();

		//Set our charstats
		charStat = GameObject.Find ("Stats").GetComponent<UnityEngine.UI.Text> ();

		//Set our arrows
		rightArrow = GameObject.Find ("Right").GetComponent<UnityEngine.UI.Text> ();
		leftArrow = GameObject.Find ("Left").GetComponent<UnityEngine.UI.Text> ();

		currentIndex = 0;

		//Set the char to the current index
		charImage.sprite = chars [currentIndex];
		charText.text = charNames [currentIndex];
		charStat.text = charStats [currentIndex].Replace("/", "\n");

		arrowCheck ();
	}
	
	// Update is called once per frame
	void Update () {

		//Get our inputs
		if (Input.GetKey ("escape")) {
			//Go back ot the start screen
			select.Play ();
			Application.LoadLevel ("Start");
		} else if (Input.GetKey (KeyCode.KeypadEnter) || Input.GetKey ("return")) {
			//save the current Index
			SaveManager.setChar (currentIndex);
			SaveManager.saveSave ();

			//Go back ot the start screen
			select.Play ();
			Application.LoadLevel ("Start");
		} 
		else if (Input.GetKeyUp (KeyCode.RightArrow) || Input.GetKeyUp (KeyCode.D)) 
		{
			//check if we can increase the current index
			if(currentIndex + 1 >= chars.Length)
			{
				//Play the hurt sound, as we cannot increase the index
				hurt.Play ();
			}
			else
			{
				//Increase the index, and play the punch sound
				currentIndex++;

				//Set the char to the current index
				charImage.sprite = chars [currentIndex];
				charText.text = charNames [currentIndex];
				charStat.text = charStats [currentIndex].Replace("/", "\n");

				punch.Play();

				arrowCheck();

			}
		}
		else if (Input.GetKeyUp (KeyCode.LeftArrow) || Input.GetKeyUp (KeyCode.A)) 
		{
			//check if we can increase the current index
			if(currentIndex - 1 < 0)
			{
				//Play the hurt sound, as we cannot increase the index
				hurt.Play ();
			}
			else
			{
				//Decrease the index, and play the punch sound
				currentIndex--;
				
				//Set the char to the current index
				charImage.sprite = chars [currentIndex];
				charText.text = charNames[currentIndex];
				charStat.text = charStats [currentIndex].Replace("/", "\n");
				
				punch.Play();

				arrowCheck();
				
			}
		}
	}


	//Function to check if we should remove any of the arrows
	private void arrowCheck()
	{
		if (chars.Length == 1) {
			leftArrow.enabled = false;
			rightArrow.enabled = false;
		}
		else if (currentIndex >= chars.Length - 1) {
			rightArrow.enabled = false;
			leftArrow.enabled = true;
		} else if (currentIndex <= 0) {
			leftArrow.enabled = false;
			rightArrow.enabled = true;
		} 
		else {
			leftArrow.enabled = true;
			rightArrow.enabled = true;
		}
	}
}
