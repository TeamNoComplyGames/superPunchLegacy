using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	public Rigidbody player;
	public float moveSpeed = 0f;
	
	void Update ()
	{
		Move();
	}
	
	void Move ()
	{
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical"); 
		Vector3 movement = new Vector3(h, v, 0f); 
		player.AddForce(movement * moveSpeed);
	}
}