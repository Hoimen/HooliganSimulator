# Hooligan Simulator
Some Multiplayer Unity game i made 


I started this sometime around my freshman year of high school and stopped working on it in December of the following year. If you want to add anything to it or download it, feel free I don’t mind. 🤩👍

<img width="1700" height="1247" alt="Screenshot 2025-11-03 204912" src="https://github.com/user-attachments/assets/ffdeaab2-12d9-4589-8280-e9fdadaa0908" />



# Download and Start
If you want to download and play this game here are the steps:

1 Unzip the file of course.

2 Open the folder named "builds”.

3 Run the .exe file (sorry I only have Windows builds right now(linux freaks)).

4 You don’t need to allow any permissions if Unity asks for access just click No.

5 If joining a room:
Click a player’s name from the list and hit Join, or choose Direct Connect and type in the code.

6 If creating a room:
Hit New Room, adjust the room settings, then click Create Room.

<img width="817" height="381" alt="Screenshot 2025-11-04 184753" src="https://github.com/user-attachments/assets/e0855af1-4703-4ce1-bd6b-f92ba6d33a37" />


# Kea-binds to know 
This game is only partially finished so I set up key binds for most features that I plan to replace later.

WASD = move 

Right click + Drag = rotate head

Space = jump

Shift = run

L = first person toggle 

E = inventory 

N = player given 1$ (only way to get money)

Q = remove current item

C = change outfit

1-4 = switch item held

Tab = small menu

Esc = Settings

<img width="470" height="461" alt="Screenshot 2025-11-03 202454" src="https://github.com/user-attachments/assets/0087e9d4-3f17-44d2-93c6-a5f93d5527c3" />
<img width="470" height="461" alt="Screenshot 2025-11-03 204119" src="https://github.com/user-attachments/assets/25a99821-0dfc-4bc2-9c08-c22a10fb258a" />

# Game features
There isn’t really a full game yet to be honest but there are a few features you can try out:

Spray Paint: Only works on the billboards and the large panel area of the junkyard.

BoomBox: Uploaded MP3s don’t work in multiplayer yet.

Money System: Your items and money are saved to files, not just stored in RAM.

Parkour Setup: There are some buildings around the map set up for basic parkour kinda cool.


<img width="948" height="594" alt="Screenshot 2025-11-03 203917" src="https://github.com/user-attachments/assets/552bc684-72f3-42ab-956b-9460becfc93c" />

ya thats pretty much it 


# Multiplayer
Adding new features to a multiplayer stinks. I used Alterune which made things a lot easier but it still kind of sucked (mostly because of my lack of skill).

Movement and held items are all networked properly and the same goes for dropped items like spray paint and other placable objects.

Some current issues include:

Can’t handle more than about 10 players per server.

Spray paint area limitations.

Some latency when items first spawn in due to low update speeds (though there aren’t any real ping issues).

<img width="2559" height="1532" alt="Screenshot 2025-11-03 202158" src="https://github.com/user-attachments/assets/19a5111e-b3ac-480d-a497-36f5c04816d6" />


# Usefull For testing

PURCHASING ITEMS - should have talked about this sooner

Pressing "n" gets you 1$ (lol)

Click the cube in the gas station to open the shop menu this is where you can buy items.

The first item costs $10, the next $20, then $30, and so on.

Each item in the shop is listed in the same order as your inventory.

Orange means the item is already purchased.

Red means you can’t afford it.

In the Settings menu pressing "SFT Leave" will return you to the Main Menu.

<img width="400" height="1085" alt="Screenshot 2025-11-04 190915" src="https://github.com/user-attachments/assets/56e6fcd8-6a20-4c8b-bf9f-7e0b2b5f9625" />

# Disclaimer
This project is mostly for fun and learning don’t expect anything polished.
I may update it occasionally, but feel free to fork it, break it, or make it better. 🤩👍









