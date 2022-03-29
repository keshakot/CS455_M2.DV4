# CS455 Module 2 DV4: Goal-oriented Behaviors   
Author: Georgiy Antonovich Bondar  

Go to https://keshakot.github.io/CS455_M2.DV4/ to play the game)

# Behaviors
Goal-oriented logic: Assets/Scripts/GOBMovement.cs

# Description
1. The 'player' has three 'goals' which it seeks to satisfy - eating, sleeping, and using the loo.   
2. The player has the following actions: use the loo, eat a snack, eat a meal, and sleep.  
3. The AI selects an action by finding the one which minimizes the player's overall 'discontentment' (ex. eating will alleviate hunger, but will increase the need to use the loo).   
3. Timing here is incorporated into the impact on the weights of each action - e.g. sleeping increases the need to use the loo and to eat, for sleeping takes time, in theory. For the sake of simulation, however, all actions are performed in 1 second.   



