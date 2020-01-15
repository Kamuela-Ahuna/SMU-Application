# Resource Manager Assignment
## Preface
The contents of this folder are to be considered Kamuela Ahuna's application
material for SMU Guildhall. Inside this folder, there are two more subfolders.
One folder is for the actual game build itself while the other folder is for
the project. The project folder contains all of the assets used to make the game
including the scripts that I have written. All of what you will see was completed in one week. 
Below, I will walk you through the contents of both folders.

### The Game
#### Before Playing
Before playing the game, extract all contents of the Game Build folder to the Assignment 1 SMU folder.
If you wish to adjust the values in the resource.txt document, navigate to
Assignment 1 SMU>Game Build>Resource Folder>resource.txt. When entering values,
restrict it to two values per line. Values should be separated by a " "(Space) and should
have the name of one resource on the left and one resource on the right. The resource on the left
relies on the existence of the resource on the right. You will notice that some values already exist.
You may choose as many or as few values to include in your tests, however, if you wish to edit the text file,
you will need to close the application if it is running. During my tests, this application was able to
run with any amount of resources I included, but has only been tested with up to 14 different resources.
After editing the text document, save the file and exit.

#### "Playing" the Game

Navigate to Assignment 1 SMU>Game Build>Resource Manager RTS.exe. A dialogue box will display. Ensure
the settings read as follows:

| Option | Setting |
| ------ | ------- |
| Screen resolution | 1920 x 1080 |
| Graphics Quality | Ultra |
| Select Monitor | (Any option is acceptable) |

After you have confirmed these settings, click "Play!" This will launch the game.
You will be welcomed by a black screen with an input field that reads "Enter the resource.txt path."
You will also notice two buttons, one that reads "Go!" and one that reads "Quit." You may
press the quit button to close the application at any time. In the input field, type in
"Resource Folder/resource.txt" and hit "Go!" If you wish to store the resource.txt file elsewhere,
you should be able to, provided you give an accurate file-path.

After you have pressed "Go!" you will be greeted with either a new screen or an error. If
you see an error or there is no change, recheck the file path for resource.txt or email me
at ahunak@student.swosu.edu. It is possible there was an error upon transferring my application to you.

If you are seeing a new screen, it should have a few new buttons including:
1. "Navigate"
2. "Initialize Nodes"
3. "Display Nodes"

To create the nodes, click "Initialize Nodes." You should receive a message displaying the success of the
initialization. After initializing the nodes, click "Display Nodes" to display the UI I have developed. From here,
you have a few inputs to help you interact with the game.

1. The arrow keys or the WASD keys will help you scroll through the world.
2. Clicking on a green node will "delete" the object, consequently deleting all of its
children as well. Deleted nodes appear red. Clicking on the node again will restore it and all of its
children.
3. If you are having trouble seeing the full tree, use the mouse scroll wheel to zoom in and out.
4. If you have lost your way, click in the Input Field next to the Navigate button. Type in the name
of a node exactly, then click "Navigate." This should re-center the camera on that node.

The nodes will be displayed on the black field. Any node that relies on another node will be connected
using a red line. The line will start wide and will narrow as it approaches the child object. With this in mind
you should be able to clearly see what nodes rely on each other.

## Project Folder
If you wish to view my scripts for the game, first extract the contents of the
Resource Manager Project Folder. From there, you can explore the directory of the Unity Project.
This assignment was created using Unity 2018.3.12f1. Viewing it from later versions of Unity
may cause issues. If you wish to get a full view of the project, navigate to
Assignment 1 SMU>Resource Manager Project Folder>Resource Manager RTS>Assets>Scenes>Primary Scene.
Clicking on this will open up a Unity project if you have it installed on your computer. From here,
you should be able to navigate through my resources to see what all I used. My scripts are located in the
"Scripts" folder and are my original work. Note that all of these scripts are written in C#. If you have any further questions or if the game does
not work as I have described it in this document, please email me at ahunak@student.swosu.edu,
 or call me at (580)-641-2519

Thank you for reading.
