using UnityEngine;
using System.Collections;

public class Bounded2DCamera : MonoBehaviour 
{
	//Our camera body
	public Rigidbody body;

	//Shake amount for screenshake
	public float shakeAmount;
	private float currentShake;
	public float decreaseAmount;
	public float totalShake;
	private bool shaking;

	//The speed the camera will lerp, e.g. 1.5f
	public float lerpSpeed;
	private bool impacting;

	//our postion
	private float posX;
	private float posY;
	private Vector3 defaultPos;
	private Vector3 wallSides;

	//We are colliding
	bool colliding;

	//Our game manager
	GameManager gameManager;


	// Use this for initialization
	void Start () 
	{
		colliding = false;
		shaking = false;
		defaultPos = new Vector3 (0, 0, -10);
		wallSides = Vector3.zero;
		impacting = false;

		//off set the camera by just a little bit to add some lerp when we start
		gameObject.transform.localPosition = new Vector3 (-.03f, -.03f, -10);

		//Get our gammaneger
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Use Vector3 lerp to lerp our camera to all ofit's positions
		if (colliding) 
		{
			//Dont move the camera for the axes that are colliding
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


			//Now need to check if we are done colliding
			if((wallSides.x == 1 && gameObject.transform.localPosition.x >= 0) || (wallSides.x == -1 && gameObject.transform.localPosition.x <= 0))
			{
				wallSides = new Vector3(0, wallSides.y, 0);
			}

			if((wallSides.y == 1 && gameObject.transform.localPosition.y >= 0) || (wallSides.y == -1 && gameObject.transform.localPosition.y <= 0))
			{
				wallSides = new Vector3(wallSides.x, 0, 0);
			}


			//If wallsides is zero, we are done colliding
			if(wallSides == Vector3.zero)
			{
				colliding = false;
			}


		} 
		else 
		{
			//translate back to ourself, 0,0, -10, with lerp
			gameObject.transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition, defaultPos, lerpSpeed * Time.deltaTime);
		}

		//Now check if we need to shake the camera
		if(shaking)
		{
			if(currentShake > 0)
			{
				//First check if we are colliding
				float xShake = 0;
				float yShake = 0;
				if(colliding)
				{
					//first check if we need to fix our x
					if(wallSides.x != 0)
					{
						//now check if we are negative or positive
						if(wallSides.x > 0)
						{
							//Then x shake must be the other direction
							xShake = Random.Range(0, .5f);
						}
						else
						{
							//Then x shake must be the other direction
							xShake = Random.Range(0, .75f);
						}
					}

					//copy/paste to fix y
					if(wallSides.y != 0)
					{
						//now check if we are negative or positive
						if(wallSides.y > 0)
						{
							//Then x shake must be the other direction
							yShake = Random.Range(0, .75f);
						}
						else
						{
							//Then x shake must be the other direction
							yShake = Random.Range(0, .75f);
						}
					}
				}
				else
				{
					xShake = Random.insideUnitCircle.x;
					yShake = Random.insideUnitCircle.y;
				}

				//If we still have some shake value, make the current camera position that much more amount
				//Also need to lerp our screen shake, and because of this make sure the camera cant go out of a certain distnace
				if(Vector3.Distance(defaultPos, gameObject.transform.localPosition) > 1.25f)
				{
					//Mve away from player
					gameObject.transform.localPosition =  gameObject.transform.localPosition + 
					new Vector3(xShake * currentShake, yShake * currentShake, 0);
				}
				else
				{
					//Move towards player
					gameObject.transform.localPosition =  gameObject.transform.localPosition - 
						new Vector3(xShake * currentShake, yShake * currentShake, 0);
				}


				currentShake = currentShake - Time.deltaTime * decreaseAmount;
			}
			else
			{
				shaking = false;
				currentShake = shakeAmount;
			}
		}
	}



	//Wall sides and camera bouns are as follows
	/*
	 * 			2
	 * 3				1
	 * 			0
	 * */
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

	//Function tto start shaking
	public void startShake()
	{
		//First check if the game is still going, gamestatus returns gameover
		if(!gameManager.getGameStatus())
		{
			//reset current shake and shake!
			currentShake = shakeAmount;
			shaking = true;
		}
	}

	//Function to increase screen shake
	public void incShake(float amount)
	{
		if(shakeAmount + amount < totalShake)
		{
			shakeAmount = shakeAmount + amount;
		}
	}

	//function to decrease lerp speed
	public void declerp(float amount)
	{
		if(lerpSpeed - amount > .1)
		{
			lerpSpeed = lerpSpeed - amount;
		}
	}

	//Function for impact pause
	//Function to call for impact pause
	public void startImpact()
	{
		StartCoroutine("impactPause");
	}
	//Pause our game for some slight seconds
	public IEnumerator impactPause()
	{
		if(!impacting)
		{
			impacting = true;
			Time.timeScale = 0.1f;
			yield return new WaitForSeconds(.002f);
			Time.timeScale = 1;
			impacting = false;
		}
	}
}
