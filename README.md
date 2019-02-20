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
All the scripts can be found in the ~/Assets/Scripts folder, with each sub-folder containing code for the respective behaviour.

Most notably:
* **~/Assets/Scripts/Player** has the code responsible player behaviour - controls, animation manager etc.
* **~/Assets/Scripts/ProceduralGeneration** contains the procedural generation code, based on the Red Blob Games [guide from 2010](http://www-cs-students.stanford.edu/~amitp/game-programming/polygon-map-generation/)
* **~/Assets/Scripts/Steering** is where all the enemy AI steering behaviour scripts can be found


## Features/refactoring I intend to add
* Unit tests
* Footsteps and Snoring SFX
* Noise sprite mask kills framerate - pending investigation if can still be included
* Enemy collision avoidance is buggy, due to reliance on colliders


