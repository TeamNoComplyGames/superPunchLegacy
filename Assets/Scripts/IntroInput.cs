using UnityEngine;
using System.Collections;

public class IntroInput : MonoBehaviour 
{
	//Our select Sounds
	public AudioSource select;
	//Our select intro sound
	public AudioSource intro;
	//Our Ui canvas group
	public CanvasGroup alphaControl;

	//Our end time
	private float endTime;
	// Use this for initialization
	void Start () 
	{
		//Start at a blank scene
		alphaControl.alpha = 0.0f;

		//Start fading in
		StartCoroutine("FadeIn");

		//Start our intro sound
		intro.Play();

		//Get our endtime
		endTime = Time.time + 35;


	}
	
	// Update is called once per frame
	void Update () 
	{
		//If enter, start the game!
		if (Input.GetKeyDown (KeyCode.KeypadEnter) || Input.GetKeyDown ("return")) 
		{
			//Play the select sounds and then start the game
			select.Play();
			Application.LoadLevel("Start");
		}
		//if escape, quit
		else if (Input.GetKeyDown("escape"))
		{
			//Quit the game
			Application.Quit();
		}
		//Check if are 3 seconds before our end time
		else if(Time.time + 4 > endTime)
		{
			//Fade out
			StartCoroutine("FadeOut");
		}
		//Check if it is time to go to intro
		else if(Time.time >= endTime)
		{
			//Play the select sounds and then start the game
			select.Play();
			Application.LoadLevel("Start");
		}
	}

	public IEnumerator FadeIn()
	{
		//fade in our alpha
		while(alphaControl.alpha < 1.0f)
		{
			alphaControl.alpha = alphaControl.alpha + .1f;
			yield return new WaitForSeconds(.3f);
		}

	}

	public IEnumerator FadeOut()
	{
		//fade in our alpha
		while(alphaControl.alpha > 0.0f)
		{
			alphaControl.alpha = alphaControl.alpha - .1f;
			yield return new WaitForSeconds(.3f);
		}
		
	}
}
