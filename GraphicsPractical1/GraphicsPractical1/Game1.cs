using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Threading;

namespace GraphicsPractical1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        // used to store the heights of the "landscape", as derived from the bitmap 
        private float[,] heightData;

        // UU provided FPS Counter, with  some additions made by us
        private FrameRateCounter frameRateCounter;

        // Adding effects so we can use shaders
        private BasicEffect effect;

        // Chapter 3: adding our new camera class
        private Camera camera;

        // Chapter 6: the Terrain
        private Terrain terrain;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        #region Bonus:
        // Laucher options:
        // Changable FOV:
        private float fov;

        // Set width at launch
        private int width;

        // Set height at launch
        private int height;

        // Set fullscreen or windowed mode
        private bool fullscreen;

        // Sets Control options
        // When enableGamepad is set to true, the program will only accept the input given when using a controller. When set to false the program will only accept mouse and keyboard controls
        private bool enableGamepad;

        // Used for storing the position of the mouse, when using the mouse and keyboard for camera controls
        // Storing the initial state will allow us to track the movement of the mouse and use it to control the camera
        MouseState originalMouseState;
        #endregion

        /// <summary>
        /// The struct-function of the game class. Also used to set some launcher values
        /// </summary>
        /// <param name="_fov"> The horizontal field of view we want our program to display in degrees </param>
        /// <param name="_width"> The width of the window </param>
        /// <param name="_height"> The height of the window </param>
        /// <param name="_fullscreen"> Whether we want our program to run in fullscreen (true) or windowed (false) </param>
        public Game1(float _fov, int _width, int _height, bool _fullscreen, bool controlOptions)
        {
            // Initializing the FPS Counter
            this.frameRateCounter = new FrameRateCounter(this);

            // Telling the program the FPS Counter has to be used. Will be called after the Update() and Draw() methods
            this.Components.Add(this.frameRateCounter);

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // BONUS:
            // Launcher options
            this.fov = _fov;
            this.width = _width;
            this.height = _height;
            this.fullscreen = _fullscreen;

            this.enableGamepad = controlOptions;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Screensize
            this.graphics.PreferredBackBufferWidth = this.width;
            this.graphics.PreferredBackBufferHeight = this.height;

            // Fullscreen (Toggle with F11)
            this.graphics.IsFullScreen = false;

            // VSync
            this.graphics.SynchronizeWithVerticalRetrace = false;

            // Update Settings
            this.graphics.ApplyChanges();

            // Setting this to true  makes the Update() and Draw() methods get called 60 times per second (Locks FPS to 60).
            // Setting this to false makes them called whenever ready (No maximum FPS). False is more desirable for measuring performance
            this.IsFixedTimeStep = false;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Filling the effect with an actual effect...
            this.effect = new BasicEffect(this.GraphicsDevice);

            // ... and using that effect to enable colors
            this.effect.VertexColorEnabled = true;

            // Chapter 7:
            // 
            this.effect.LightingEnabled = true;
            this.effect.DirectionalLight0.Enabled = true;
            this.effect.DirectionalLight0.DiffuseColor = Color.White.ToVector3();
            this.effect.DirectionalLight0.Direction = new Vector3(0, -1, 0);
            this.effect.AmbientLightColor = new Vector3(0.3f);

            // Creating our camera
            // BONUS:
            // The camera class now accepts an FOV option, by default it is 60 degrees.
            this.camera = new Camera(new Vector3(50, 100, -120), new Vector3(0, 0, 0), new Vector3(0, 1, 0), this.fov);

            // BONUS:
            // Using the mouse to control the camera
            if (!enableGamepad)
            {
                Mouse.SetPosition(
                    GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2
                    );
                originalMouseState = Mouse.GetState();
            }

            // Chapter 6, turning our loaded image into an array and passing it on to the terrain to make a 3D terrain out of it
            Texture2D map = Content.Load<Texture2D>("heightmap");
            this.terrain = new Terrain(new HeightMap(map), 0.2f, GraphicsDevice);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Amount of seconds between this and the previous Update()
            float timeStep = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Changing the title of the game to contain the FPS.
            this.Window.Title = "Graphics Tutorial | FPS: " + this.frameRateCounter.FrameRate;

            // Bonus: camera with differing FoV
            // NEEDS SOME MORE LOVE!
            if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                //this._FoV += 0.1f * timeStep;
                this.fov += 5f * timeStep;
                this.camera.updateFoV(this.fov);
            }

            // Allows the game to exit
            KeyboardState kbState = Keyboard.GetState();
            GamePadState gpState = GamePad.GetState(PlayerIndex.One);

            if (gpState.Buttons.Back == ButtonState.Pressed || kbState.IsKeyDown(Keys.Escape))
                this.Exit();

            if (enableGamepad)
            {
                this.gamepadHandler(gpState, timeStep);
            }
            else
            {
                this.keyboardHandler(kbState, timeStep);
            }

            #region Rotating the world
            /*
             Spin the triangle!
             Not using a fixed amount per Update() (angle += 3.0f), but an fixed amount per time (angle += 3*timeStep)!
             Doing the former would make the "Physics" of this game/simulation be bound to the frame rate: doubling the FPS would double the rotation speed.
             Using the latter, the triangle will rotate every x seconds y degrees, no matter what the FPS is.
            */

            // Rotating the world-object horizontally (around the y-axis).
            float deltaAngle = 0;
            if (kbState.IsKeyDown(Keys.Left))
            {
                deltaAngle += -3 * timeStep;
            }
            if (kbState.IsKeyDown(Keys.Right))
                deltaAngle += 3 * timeStep;
            if (deltaAngle != 0)
            {
                this.camera.Eye = Vector3.Transform(
                       this.camera.Eye,
                       Matrix.CreateFromAxisAngle(this.camera.Up, deltaAngle)
                       );
            }
            #endregion Rotating the world

            base.Update(gameTime);
        }

        #region Bonus: Differing control options
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kbState">The current state of the keyboard (what keys are pressed and what not)</param>
        /// <param name="timeStep">Requires the time between the current and the last calcuation</param>
        private void keyboardHandler(KeyboardState kbState, float timeStep)
        {
            // Mouse handler:
            MouseState currentMouseState = Mouse.GetState();
            if (currentMouseState != originalMouseState)
            {
                float xDifference = currentMouseState.X - originalMouseState.X;
                float yDifference = currentMouseState.Y - originalMouseState.Y;

                this.camera.Rotate(-25f * timeStep * xDifference);
                this.camera.Pitch(25f * timeStep * yDifference);

                Mouse.SetPosition(
                    GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2
                    );
            }

            // Keyboard handler
            if (kbState.IsKeyDown(Keys.W))
            {
                float distance = 25f * timeStep;
                this.camera.MoveForward(distance);
            }
            if (kbState.IsKeyDown(Keys.S))
            {
                float distance = -25f * timeStep;
                this.camera.MoveForward(distance);
            }
            if (kbState.IsKeyDown(Keys.A))
            {
                float distance = 25f * timeStep;
                this.camera.Strafe(distance);
            }
            if (kbState.IsKeyDown(Keys.D))
            {
                float distance = -25f * timeStep;
                this.camera.Strafe(distance);
            }

            if (kbState.IsKeyDown(Keys.Space))
            {
                float distance = 25f * timeStep;
                this.camera.Raise(distance);
            }
            if (kbState.IsKeyDown(Keys.LeftShift))
            {
                float distance = -25f * timeStep;
                this.camera.Raise(distance);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gpState">Requires the current state of the gamepad</param>
        /// <param name="timeStep"></param>
        private void gamepadHandler(GamePadState gpState, float timeStep)
        {
            if (gpState.ThumbSticks.Left != Vector2.Zero)
            {
                this.camera.Strafe(
                    -20f * timeStep * gpState.ThumbSticks.Left.X
                    );
                this.camera.MoveForward(
                    40f * timeStep * gpState.ThumbSticks.Left.Y
                );
            }
            if (gpState.ThumbSticks.Right != Vector2.Zero)
            {
                this.camera.Pitch(
                    -40f * timeStep * gpState.ThumbSticks.Right.Y
                    );
                this.camera.Rotate(
                    -40f * timeStep * gpState.ThumbSticks.Right.X
                );
            }
        }
        #endregion

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Enabling/disabling culling (not drawing certain triangles)
            // Chapter 6: FillMode is the way the triangles are set up: here it is set to just drawing the wireframe. The other option is to fill in each triangle and have a bunch of survaces
            // Chapter 7: FillMode will be set to Solid. This will color the entire surface of the traingles.
            this.GraphicsDevice.RasterizerState = new RasterizerState
            {
                CullMode = CullMode.None,
                FillMode = FillMode.Solid
            };

            // Background color of the image
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Matrix translation = Matrix.CreateTranslation(-0.5f * this.terrain.Width, 0, 0.5f * this.terrain.Width);

            // Changing the world accordingly to what the camera sees
            this.effect.Projection = this.camera.ProjectionMatrix;
            this.effect.View = this.camera.ViewMatrix;
            this.effect.World = translation;

            foreach (EffectPass pass in this.effect.CurrentTechnique.Passes)
            {
                pass.Apply();
            }


            // Chapter 6: replaced DrawUserPrimitives() with this
            this.terrain.Draw(this.GraphicsDevice);

            base.Draw(gameTime);
        }
    }
}
