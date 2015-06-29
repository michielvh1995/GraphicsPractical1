By:
Michiel van Heusden, 4173309
Bob Best,            .
Ruben Meeuwesse,     .

Base Information:
 * The program is made with the HiDef profile
 * No additional packages were installed
 * Programmed on a Windows 8.1 version of Visual Studio 2013
   - This required a non-official installer for XNA Game Studio.

Extra Features:
1) Multiple types of input:
   * Controller controls:
    - Use left stick for movement
    - Use right stick to rotate the camera and "aim"
    - Press the B button to make the camera rotate around the center of the model
    - Pressing the Left Bumper will decrease the FoV of the camera
    - Pressing the Right Bumper will increase the FoV of the camera
    - Press the back button to exit the program

   * Keyboard & Mouse (FPS-like controls):
    - space to ascend
    - shift to descent
    - W to move forward
    - S to move backward
    - A to move left
    - D to move right
    - Pressing the Q button will decrease the FoV of the camera
    - Pressing the E button will increase the FoV of the camera
    - Mouse can be used to look around
    - Pressing the escape button will exit the program

2) A "cinematic view":
  * Activated by either pressing enter (when keyboard controls are activated)
      or by pressing the B button of a gamepad
  * Spins the camera at the current position around the (0,0,0) vector

3) Variable FoV:
  * The (horizontal) view angle is changeable
  * This is done by:
     a) Setting it in the launcher (-fov)
     b) Pressing the bumpers of the controler (LB to decrease, RB to increase)
     c) Pressing the E and Q buttons on the keyboard (Q to decrease, E to increase)
  * WARNING: The FoV may seem to barely change, but eventually it will be messed up (FoV > 200)

4) Terrain Colors based on height:
  * Currently they are crudely implemented
  * The colors are (from low to high):
    - Blue
    - Green
    - Gray
    - White

5) A launcher:
  * The launcher .bat file can be found in the same folder as the solution
  * The launcher only works when the program has been build, which can be done by:
    - Selecting the debug option in visual studio
    - Build -> Build Solution
  * It has few options:
    -c sets the input method:
      -> gamepad sets the input method to gamepad only
      -> anything else will default to keyboard & mouse
    -w sets the window width
    -h sets the window height
    -fov sets the FoV of the camera
    -s sets the screen:
      -> fullscreen sets the screen to fullscreen
      -> anything else will default to windowed
  * None of the options are required; when they are not set, they will be set to a default value
