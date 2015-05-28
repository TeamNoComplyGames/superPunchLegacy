using UnityEngine;
using System.Collections;

//This Script will handle scaling canvas and 2d game camera. Attatch to canvas gameobject
public class GameAspect : MonoBehaviour 
{

	//our canvas scaler
	UnityEngine.UI.CanvasScaler uiScale;

	// Use this for initialization
	void Start () 
	{

		//Set up our camera
		//Get our aspect ratio
		float aspectRatio = Camera.main.aspect;

		//Get our scaler
		uiScale = GameObject.Find ("UI").GetComponent<UnityEngine.UI.CanvasScaler>();

		//Now run through if statement to set canvas size and camera size
		//5:4
		if (aspectRatio >= 1.25) 
		{
			//Change camera.orthagraphic size and ui scale values here
			Camera.main.orthographicSize = 0.85f;
			uiScale.scaleFactor = .75f;
		}
		//4:3
		else if (aspectRatio >= 1.33) 
		{
			//Change camera.orthagraphic size and ui scale values here
			Camera.main.orthographicSize = 0.8f;
			uiScale.scaleFactor = .8f;
		}
		//3:2
		else if (aspectRatio >= 1.5) 
		{
			//Change camera.orthagraphic size and ui scale values here
			Camera.main.orthographicSize = 0.725f;
			uiScale.scaleFactor = .9f;
		}
		//16:10
		else if (aspectRatio >= 1.6) 
		{
			//Change camera.orthagraphic size and ui scale values here
			Camera.main.orthographicSize = 0.665f;
			uiScale.scaleFactor = .95f;
		}
		//16:9
		else if (aspectRatio >= 1.77) 
		{
			//Change camera.orthagraphic size and ui scale values here
			Camera.main.orthographicSize = 0.6f;
			uiScale.scaleFactor = 1.0f;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	}
}
