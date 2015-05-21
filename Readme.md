(WORK IN PROGRESS)

## Intro

![alt tag](http://i.imgur.com/Zjy44T2.png)

This is an arcade top-down action fighting game (With some "RPG" elements) being created in unity.
Draws inspiration from the Legacy of Goku series released on the GBA.

## Language

C# using unity scripts, e.g

//Called every frame
	void Update ()
	{

		//check if dead, allow movement if alive
		if (health <= 0)
		{
			//make our player object invisible
			//possible display some animation first
			//Renderer r = (Renderer) gameObject.GetComponent("SpriteRenderer");
			//r.enabled = false;
			//No longer turning invisible, just looping death animation
			//play our death animation
			if(!animator.GetBool("Death"))
			{
				animator.SetTrigger("DeathTrigger");
				animator.SetBool("Death", true);
				//play the death sound
				if(!death.isPlaying)
				{
					death.Play();
				}
			}
			//Set our gameover text
			gameManager.setGameStatus(false);
		}
		else
		{
			//Move our player
			Move();

			//Attacks with our player (Check for a level up here as well)
			if (Input.GetKeyDown ("space")) {
				if(!attacking)
				{
					//Attacking working great
					StartCoroutine ("Attack");
				}
			}
		}

		//Check for levelUp
		if (exp >= playerLevel * playerLevel)
		{
			//Reset/increase stats
			exp = 0;
			++playerLevel;
			health = playerLevel * 5;
			gameManager.invokeEnemies();
			//Play our sound
			levelUp.Play();
		}
	}

## Motivation

A short description of the motivation behind the creation and maintenance of the project. This should explain **why** the project exists.

## Installation

To Edit in Unity: Simple download the zip and extract, and everything should work

To install and Play: testers should contact me via E-mail, but this will eventually be released on mobile and desktop

## Screenshots (Alpha Build)

![alt tag](http://i.imgur.com/6MXe2r9.png)
![alt tag](http://i.imgur.com/UAvM6RG.png)
![alt tag](http://i.imgur.com/ATa05Um.png)

## Team

Founder/Programming/Unity Dev/Design/Sound - Aaron Turner (Torch2424)
Art/Story/Design - Kevin Vigil (Heebe)
Art - Emanual Partida

## License

All Assessts and rights of the games are reserved to No Comply Games

However this code is open-source and may be used to help in your own projects

P.s I don't know much about each liscences haha
