using UnityEngine;
using System.Collections;

public class MapInput : MonoBehaviour {

	//Our array of map images
	public UnityEngine.UI.Image[] maps;

	//Array of maps names, should correspond with maps
	public string[] mapNames;

	//Our map image
	private UnityEngine.UI.Image mapImage;

	//Our map name
	private UnityEngine.UI.Text mapText;

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

		currentIndex = 0;
	}
	
	// Update is called once per frame
	void Update () {

		//Get our inputs
		if (Input.GetKey ("escape")) 
		{
			//Go back ot the start screen
			select.Play();
			Application.LoadLevel("Start");
		}
		else if(Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey ("return"))
		{
			//save the current Index
			SaveManager.setMap(currentIndex);
			SaveManager.saveSave();

			//Go back ot the start screen
			select.Play();
			Application.LoadLevel("Start");
		}

	
	}
}
