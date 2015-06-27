using UnityEngine;
using System.Collections;

//This Script will handle scaling canvas and 2d game camera. Attatch to canvas gameobject
public class GameAspect : MonoBehaviour 
{

	//our canvas scaler
	UnityEngine.UI.CanvasScaler uiScale;

	//static int to return on aspect ratio
	public static float aspectRatio;

	//Our box Collider (if it exists)
	private BoxCollider theBox;

	// Use this for initialization
	void Start () 
	{

		//Set up our camera
		//Get our aspect ratio
		aspectRatio = Camera.main.aspect;

		// attempt to get our Our box collider
		theBox = Camera.main.GetComponent<BoxCollider> ();

		//Get our scaler
		uiScale = GameObject.Find ("UI").GetComponent<UnityEngine.UI.CanvasScaler>();

		//Now run through if statement to set canvas size and camera size
		//5:4
		if (aspectRatio <= 1.26) {
			//Change camera.orthagraphic size and ui scale values here
			Camera.main.orthographicSize = 0.85f;
			uiScale.scaleFactor = .75f;

			//Change box collider if it exists
			if(theBox != null)
			{
				theBox.size = new Vector3(2.85f, 2.25f, 1.0f);
			}
		}
		//4:3
		else if (aspectRatio <= 1.34) {
			//Change camera.orthagraphic size and ui scale values here
			Camera.main.orthographicSize = 0.8f;
			uiScale.scaleFactor = .8f;

			//Change box collider if it exists
			if(theBox != null)
			{
				theBox.size = new Vector3(2.85f, 2.15f, 1.0f);
			}
		}
		//3:2
		else if (aspectRatio <= 1.51) {
			//Change camera.orthagraphic size and ui scale values here
			Camera.main.orthographicSize = 0.725f;
			uiScale.scaleFactor = .9f;

			//Change box collider if it exists
			if(theBox != null)
			{
				theBox.size = new Vector3(2.835f, 1.95f, 1.0f);
			}
		}
		//16:10
		else if (aspectRatio <= 1.61) 
		{
			//Change camera.orthagraphic size and ui scale values here
			Camera.main.orthographicSize = 0.665f;
			uiScale.scaleFactor = .95f;

			//Change box collider if it exists
			if(theBox != null)
			{
				theBox.size = new Vector3(2.8f, 1.75f, 1.0f);
			}
		}
		//16:9
		else if (aspectRatio <= 1.78) 
		{
			//Change camera.orthagraphic size and ui scale values here
			Camera.main.orthographicSize = 0.6f;
			uiScale.scaleFactor = 1.0f;

			//Change box collider if it exists
			if(theBox != null)
			{
				theBox.size = new Vector3(2.8f, 1.75f, 1.0f);
			}
		} 
		else 
		{
			//Default to nothing
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	//function to return our our aspect ratio
	public static float getAspect()
	{
		return aspectRatio;
	}
}
