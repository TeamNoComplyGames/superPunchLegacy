using UnityEngine;
using System.Collections;

public class FlashingText : MonoBehaviour 
{
	//Our text
	public UnityEngine.UI.Text text;

	// Use this for initialization
	void Start () 
	{
		//Call our flash function multiple times
		InvokeRepeating("flash", 0 , 0.5f);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	//Our flahing text function
	private void flash()
	{
		//If the text is eneabled disable it, else renable
		if (text.enabled) {
			text.enabled = false;
		} else {
			text.enabled = true;
		}
	}
}
