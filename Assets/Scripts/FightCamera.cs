using UnityEngine;
using System.Collections;

public class FightCamera : MonoBehaviour 
{

	//Our player object
	private Player user;

	//Our camera body
	public Rigidbody body;

	//our postion
	private float posX;
	private float posY;
	private Vector3 defaultPos;
	private Vector3 wallSides;

	//We are colliding
	bool colliding;


	// Use this for initialization
	void Start () 
	{
		colliding = false;
		defaultPos = new Vector3 (0, 0, -10);
		wallSides = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (colliding) 
		{
			//Dont move the camera
			if(wallSides.x != 0 && wallSides.y != 0)
			{
				gameObject.transform.position = new Vector3(posX, posY, -10);
			}
			else if(wallSides.x != 0 && wallSides.y == 0)
			{
				gameObject.transform.position = new Vector3(posX, gameObject.transform.position.y, -10);
			}
			else
			{
				gameObject.transform.position = new Vector3(gameObject.transform.position.x, posY, -10);
			}

		} 
		else 
		{
			//translate back to ourself, 0,0, -10
			gameObject.transform.localPosition = defaultPos;
		}
		
	}

	//When we enter bounds collison
	void OnCollisionEnter(Collision collision) 
	{
		//Dont move the camera until we exit the collision
		colliding = true;

		//Create a vector depending on if we hit the left or right side
		if (collision.collider.name.Contains ("CameraBounds1")) 
		{
			posX = gameObject.transform.position.x;
			wallSides.x = 1;
		} else if (collision.collider.name.Contains ("CameraBounds3")) 
		{
			posX = gameObject.transform.position.x;
			wallSides.x = -1;
		} else if (collision.collider.name.Contains ("CameraBounds2")) 
		{
			posY = gameObject.transform.position.y;
			wallSides.y = 1;
		} 
		else if (collision.collider.name.Contains ("CameraBounds0")) 
		{
			posY = gameObject.transform.position.y;
			wallSides.y = -1;
		}
	}


}
