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
	//Our Sprite
	public GameObject sprite;
	//Our main sprite
	//Our fade speed
	public float fadeSpeed;
	//Our end time
	private float endTime;
	//Boolean to check if we are fading out already
	private bool fadingOut;
	// Use this for initialization
	void Start () 
	{
		//Start at a blank scene
		alphaControl.alpha = 0.0f;

		//Start fading in
		InvokeRepeating("FadeIn", 0 ,.2f);

		//Start our intro sound
		intro.Play();

		//Get our endtime
		endTime = Time.time + 32.47f;

		fadingOut = false;


	}
	
	// Update is called once per frame
	void Update () 
	{
		//If enter, start the game!
		if (Input.GetKeyDown (KeyCode.KeypadEnter) || Input.GetKeyDown ("return")) 
		{
			//Play the select sounds and then start the game
			CancelInvoke();
			if(intro.isPlaying)
			{
				intro.Stop();
			}
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
		if(Time.time + 7.5 > endTime && !fadingOut)
		{
			//Fade out
			fadingOut = true;
			CancelInvoke("FadeIn");
			InvokeRepeating("FadeOut" , 0 ,.2f);
		}


		//Check if it is time to go to intro
		if(Time.time >= endTime)
		{
			//Play the select sounds and then start the game
			CancelInvoke();
			if(intro.isPlaying)
			{
				intro.Stop();
			}
			select.Play();
			Application.LoadLevel("Start");
		}
	}

	public void FadeIn()
	{
		//fade in our alpha
		if(alphaControl.alpha + fadeSpeed < 1.0f)
		{
			alphaControl.alpha = alphaControl.alpha + fadeSpeed;
			sprite.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, alphaControl.alpha);
		}
		else
		{
			alphaControl.alpha = 1.0f;
			sprite.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, alphaControl.alpha);
			CancelInvoke("FadeIn");
		}
	}

	public void FadeOut()
	{
		//fade out our alpha
		if(alphaControl.alpha - fadeSpeed > 0.0f)
		{
			alphaControl.alpha = alphaControl.alpha - fadeSpeed;
			sprite.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, alphaControl.alpha);
		}
		else
		{
			alphaControl.alpha = 0.0f;
			sprite.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, alphaControl.alpha);
			CancelInvoke("FadeOut");
		}
		
	}
}
