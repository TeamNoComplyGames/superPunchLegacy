using UnityEngine;
using System.Collections;

public class FlashingText : MonoBehaviour 
{
	//Our text
	public UnityEngine.UI.Text text;

	//Our flashing speed
	public float flashSpeed;

	//Our flashing boolean
	public bool flashOnStart;

	// Use this for initialization
	void Start () 
	{
		//Call our flash function multiple times
		InvokeRepeating ("flash", 0, flashSpeed);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	//Our flahing text function
	private void flash()
	{
		//If the text is eneabled disable it, else renable
		if (flashOnStart) {
			if (text.enabled) {
				text.enabled = false;
			} else {
				text.enabled = true;
			}
		} 
		else 
		{
			text.enabled = false;
		}
	}

	//Stop our repeating invoke
	public void stopFlash()
	{
		//set our booleans to false
		flashOnStart = false;
	}

	//Start flashing again
	public void startFlash()
	{
		//Call our flash function multiple times
		flashOnStart = true;
	}

}
