## Intro, SUPER PUNCH LEGACY

(Work in progress, Readme shall be update with game)

Last Update: 6/27/15, between Alpha 4.0 and 5.0

![alt tag](http://i.imgur.com/4nPtt1D.png)
![alt tag](http://i.imgur.com/jsC4yL8.png)

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

## Inspiration

I (Aaron Turner) was thinking of a good simple game to create as a side project in between school and work.
That would help me learn how to use the beast we know as UNITY. I came across some old
Legacy of Goku Let's plays and made me remember how fun those games were. I have been a big fan of Dragon Ball Z games growing up,
and I am particularly fond of the GBA era. So I decided to make a game like it, but add my own, arcade, fun, and ideas to it. Thus,
Punch Legacy was born. Kevin and Emanuel came into the mix as they are close childhood friends of mine, and they have been wanting to
make a game themselves, but just really didn't know where to start. And After I had half-completed a game myself, I kind of
already had a clue on the mysterious world of independent game development.

However, Before this game is finished it will be known as Super Punch Legacy, because that is exactly,
what it shall be.

## Installation

To Edit in Unity: Simple download the zip and extract, and everything should work

To Install and Play: testers should contact me via E-mail, but this will eventually be released on mobile and desktop

## Screenshots (Alpha 4.0, pre Alpha 5.0)

![alt tag](http://i.imgur.com/abN5mJ0.png)
![alt tag](http://i.imgur.com/Nw2DxpA.png)
![alt tag](http://i.imgur.com/TXYEokf.png)

## Gameplay GIF (Alpha 4.0, pre Alpha 5.0)

![alt tag](http://giant.gfycat.com/LastGlumIrukandjijellyfish.gif)

## Team

Founder/Programming/Unity Dev/Design/Sound - Aaron Turner (Torch2424)
Art/Story/Design - Kevin Vigil (Heebe)
Art - Emanuel Partida

## License

All Assets and rights of the games are reserved to No Comply Games

However this code is open-source and may be used to help in your own projects

P.S I don't know much about each liscences haha
