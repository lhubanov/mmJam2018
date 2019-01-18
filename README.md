# The Tree of Life

> A top-down adventure game about emotional power, developed as a part of Guildford Game Jam: Power (2018).

In this 2D sandbox, the player-controlled main character possesses a special power, allowing her to heal her ill mother through the surrounding world. After venturing deeper into the forest, however, the player must confront the realization that the cost for this is the death of everything nearby. The game was developed as a part of a 6-person team.

More info on the game itself [here](https://elhubanov.com/portfolio/the-tree-of-life/).

The playable latest build of the game can be downloaded from [itch.io](https://elhubanov.itch.io/the-tree-of-life).

## Credits
[Ioana Iulia Cazacu](https://twitter.com/GreenStorm27) : Design and Art

Mike Everett : Design

[Alex Jones](https://twitter.com/Alyx_Jones) : Composition & Sound Design

[Dave Allen](https://twitter.com/daveallenbpm) : Composition & Sound Design

[Lorraine Ansell](https://twitter.com/LAvoiceart) : Voice Acting

[Lachezar Hubanov](https://twitter.com/elhubanov) : Programming

##  Getting started
### Building

The latest update of the game was made under Unity 2018.3.0f2. 

The game is a single sandbox found in the MainSandbox.scene.

All the code is in C#.


### Code
All the scripts can be found in the ~/Assets/Scripts folder, with each sub-folder containing the respective behaviour- e.g. Player control, Steering behaviours, Procedural generation etc.

Note: I intend to do another update of the game in the immediate future. There are still some animations and audio fx missing. I also need to create a sprite for the enemies in the game, as they are placeholders right now. See the lists below of what is left.

## Pending features
* Enemies should flee after stealing the player's held energy
* Enemy spawning is pre-determined currently - make them spawn in the Rosebush/Marsh biome of the procedurally generated world.
* Replace placeholder enemy object with 2D sprite
* Teleportation animation
* Footsteps and Snoring SFX


## Pending fixes/refactoring
* Noise sprite mask kills framerate - pending investigation if can still be included
* ~~LoseLifeIdly coroutine on the Mom GameObject is triggered via a flag, but needs to be via event, instead~~
* Enemy collision avoidance is buggy, due to reliance on colliders
* ~~TileLookup dictionary needs refactoring to get a Sprite object~~
* Animator.Play() uses a string as an input argument in Branch.cs, but surely can be done in a neater way
* PlayerController.cs is in dire need of refactoring
* Container classes need refactoring or replacing
* Biome constructors are messy
* There are a variety of smaller FIXME comments, that need looking at


