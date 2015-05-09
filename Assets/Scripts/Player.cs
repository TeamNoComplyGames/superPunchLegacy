using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	public Rigidbody2D player;
	public float moveSpeed = 0f;
	
	void Update ()
	{
		Move();
	}
	
	void Move ()
	{
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical"); 
		Vector2 movement = new Vector3(h, v); 
		player.MovePosition(player.position + movement * moveSpeed);
	}
}