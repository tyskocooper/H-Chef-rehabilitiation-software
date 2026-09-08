## NAME
H-CHEF
## Description
H-Chef has been specifically designed to work with Articare's H-Man rehabilitation device which supports upper-body physical therapy. H-Chef attemps to emulate a cooking environment through various movement exercises such as chopping and stirring with incremental difficulty. It also uses ‘pattern-matching’ techniques for recipes to inspire cognitive engagement. The overall aim for this H-Man game is to encourage the practice and improvement of movement exercises for stroke recovery patients. 


## Screenshots

<img width="856" height="480" alt="image" src="https://github.com/user-attachments/assets/a292828c-f499-4423-b155-9501f4f951c2" />

<img width="856" height="497" alt="image" src="https://github.com/user-attachments/assets/3b644ec1-7ccb-400c-9d9a-bb1a438a2344" />


## Installation
After cloning this repository, you can launch the game via the GameLauncher file.
If you wish to edit the project, you need to import the project to a UNITY Editor.

## Usage
The game can be played with H-Man Rehabilitation Device. Failing this, the fallback is mouse/trackpad based movement.
The input and functionality is built around planal X/Y 2D movement input and collision triggers. 

## Scripts

DropArea ->  dropping ingredients at both the chopping board and service area 

BoilingPot -> the boiling pot where the player drops chopped items. contains logic for its multiple states. Also triggers stirring minigame

PrepArea -> minigame logic used multiple times in the game. cursor needs to collide with hit points to eliminate them

ChoppingMinigame ->  spawns an chopping board overlay and hit points defined from PrepArea. Also re-used for stirring minigame in the inspector.

PickupItem -> logic for the different ingredients that can be equipped by the player ans their relationship with different colliders (drop areas)

HManConnection -> calls Articares DLL which allows for calling different low-level functions from the H-Man Device (adapted from a MatLab script and rewritten in C#) 

H-ManCursor -> cursor built based on H-Man coordinates which translates to a sprite on screen

HoverToClick -> as the H-Man device contains no press or click input, hover to click creates time based triggers that call actions

StartButton -> works with HoverToClick and defines various menu buttons such as Start,Exit and Restart

PlayerController -> controls the chef sprite that follows the cursor, also updates based on ingredients equipped and triggers collisions

Countdown -> timer for the game

ScoreManager -> score for the game, updates on delivering finished dish to the sevice area

ServiceOver -> game over UI

## Acknowledgment
I would like to thank Dr Damien Anderson for supervising this project.

I would also like to thank Dr Andy Kerr and the bio-engineering department for facilitating this project's development.
