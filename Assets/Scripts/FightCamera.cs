using UnityEngine;
using System.Collections;

public class FightCamera : MonoBehaviour 
{

	//Our player object
	private Player user;

	//Our camera body
	public Rigidbody body;

	//our postion
	private Transform pos;

	//We are colliding
	bool colliding;


	// Use this for initialization
	void Start () 
	{
		//Get our player
		user = GameObject.Find ("Person").GetComponent<Player>();

		colliding = false;
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (colliding) {
			gameObject.transform.position = pos.position;
		} 
		else 
		{
			transform.position = user.transform.position;
		}
		
	}

	//When we enter bounds collison
	void OnCollisionEnter(Collision collision) 
	{
		//Dont move the camera until we exit the collision
		pos = body.transform;
		colliding = true;
	}

	//After we exit bounds collision
	void OnCollisionExit(Collision collisionInfo) 
	{
		colliding = false;
	}


}
