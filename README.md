# Simple UDP multiplayer game made to learn networking.

## How to run the game

The game should be player on a local network.

* Build Server scene and run it on a computer,
* Build Client scene and also run it,
* Builds should also be available in Releases section on github,
* On client, press Connect button. Client should detect running server automatically and connect to it,
* If the button doesn't disappear - try again. UDP message could have been lost,
* When client connects successfully a player should spawn on server and client,
* Connect more clients from different computers if you want and play the game.

## Features

* Authoritative server,
* Client-side prediction and server reconcilation,
* Entity interpolation.

I've tried to follow some concepts from Overwatch's netcode, so client runs ahead of server and changes its tick rate when it goes out of sync with server.

## How to play

Use W, A, S, D keys to move. When player is near the ball press SPACE to kick. Place the ball in opposite color goal.

### TODO:

* Handling disconnections,
* Not using long strings as message headers. It's a waste of bandwith but I did it for convenience,
* Using FixedUpdate for ticking client and server may not be the best idea. Research possible better solutions.
