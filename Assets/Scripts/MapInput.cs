using UnityEngine;
using System.Collections;

public class MapInput : MonoBehaviour {

	//Our array of map images
	public Sprite[] maps;

	//Array of maps names, should correspond with maps
	public string[] mapNames;

	//Our map image
	private UnityEngine.UI.Image mapImage;

	//Our map name
	private UnityEngine.UI.Text mapText;

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

		//Set our map image
		mapImage = GameObject.FindGameObjectWithTag ("HUDImage").GetComponent<UnityEngine.UI.Image> ();

		//Set our mapText
		mapText = GameObject.FindGameObjectWithTag ("PlayerHUD").GetComponent<UnityEngine.UI.Text> ();

		//Set our arrows
		rightArrow = GameObject.Find ("Right").GetComponent<UnityEngine.UI.Text> ();
		leftArrow = GameObject.Find ("Left").GetComponent<UnityEngine.UI.Text> ();

		currentIndex = 0;

		//Set the map to the current index
		mapImage.sprite = maps [currentIndex];
		mapText.text = mapNames [currentIndex];

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
			SaveManager.setMap (currentIndex);
			SaveManager.saveSave ();

			//Go back ot the start screen
			select.Play ();
			Application.LoadLevel ("Start");
		} 
		else if (Input.GetKeyUp (KeyCode.RightArrow) || Input.GetKeyUp (KeyCode.D)) 
		{
			//check if we can increase the current index
			if(currentIndex + 1 >= maps.Length)
			{
				//Play the hurt sound, as we cannot increase the index
				hurt.Play ();
			}
			else
			{
				//Increase the index, and play the punch sound
				currentIndex++;

				//Set the map to the current index
				mapImage.sprite = maps [currentIndex];
				mapText.text = mapNames [currentIndex];

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
				
				//Set the map to the current index
				mapImage.sprite = maps [currentIndex];
				mapText.text = mapNames [currentIndex];
				
				punch.Play();

				arrowCheck();
				
			}
		}
	}


	//Function to check if we should remove any of the arrows
	private void arrowCheck()
	{
		if (currentIndex <= 0) {
			leftArrow.enabled = false;
			rightArrow.enabled = true;
		} else if (currentIndex >= maps.Length - 1) {
			rightArrow.enabled = false;
			leftArrow.enabled = true;
		} else {
			leftArrow.enabled = true;
			rightArrow.enabled = true;
		}
	}
}
