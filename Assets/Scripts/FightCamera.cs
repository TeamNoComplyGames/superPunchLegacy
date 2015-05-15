using UnityEngine;
using System.Collections;

public class FightCamera : MonoBehaviour 
{

	//Our player object
	private Player user;

	//Our camera body
	public Rigidbody body;

	//our postion
	private Vector3 pos;
	private Vector3 defaultPos;

	//We are colliding
	bool colliding;

	//Our speed we want to fix camera
	public float fixSpeed;


	// Use this for initialization
	void Start () 
	{
		colliding = false;
		defaultPos = new Vector3 (0, 0, -10);
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (colliding) 
		{
			//Donr move the camera
			gameObject.transform.position = pos;

			//Check if our local postion is 0 or or greater on either axis for x
			if (gameObject.transform.localPosition.x != 0) 
			{
				//Ttranslate back to the player
				//Smoothly move the camera back over the player
				gameObject.transform.Translate (-1 * gameObject.transform.localPosition.x * fixSpeed * Time.deltaTime, 0, 0, Space.Self);
			}

			//Check if our local postion is 0 or or greater on either axis for y
			if (gameObject.transform.localPosition.y != 0) 
			{
				//Ttranslate back to the player
				//Smoothly move the camera back over the player
				gameObject.transform.Translate (0, -1 * gameObject.transform.localPosition.y * fixSpeed * Time.deltaTime, 0, Space.Self);
			}
			
			//If both are done trnaslating, we are done exiting the collision
			if(gameObject.transform.localPosition == defaultPos)
			{
				colliding = false;
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
		pos = gameObject.transform.position;
		colliding = true;
	}


}
