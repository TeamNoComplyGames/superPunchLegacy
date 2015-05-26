using UnityEngine;
using System.Collections;

//This Script will handle scaling canvas and 2d game camera. Attatch to canvas gameobject
public class GameAspect : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{

		//Set up our camera
		//Get our aspect ratio
		float aspectRatio = Camera.main.aspect;
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Show our aspect ratio
		Debug.Log (Camera.main.aspect);
	}
}
