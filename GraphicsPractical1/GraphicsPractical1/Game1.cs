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
        // UU provided FPS Counter
        private FrameRateCounter frameRateCounter;

        // Multithreading
        private Thread t;

        // Adding effects so we can use shaders
        private BasicEffect effect;

        // Chapter 2: Vertices 
        private VertexPositionColor[] vertices;

        // Chapter 3: adding our new camera class
        private Camera camera;

        // Chapter 4: enabling rotation
        private float angle;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            // Initializing the FPS Counter
            this.frameRateCounter = new FrameRateCounter(this);

            // Telling the program the FPS Counter has to be used. Will be called after the Update() and Draw() methods
            this.Components.Add(this.frameRateCounter);

            // Setting up the concurrent 


            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /*
         Chapter 2: 
         * Creating a bunch of random Vertices to demonstrate the drawing power of XNA
         Chapter 3:
         * Changing the coordinates from a 2D plane to a 3D World Space.
         * Result:    Triangle's gone. 
         * Why?       We've stopped using the predifined screen-coordinates ([-1 to 1; -1 to 1]).
         * Solution:  Creating a camera to transform these 3D coords to the 2D screen-coords.
        */
        private void setupVertices()
        {
            this.vertices = new VertexPositionColor[3];
            this.vertices[0].Position = new Vector3(0f, 0f, 0f);
            this.vertices[0].Color = Color.Red;
            this.vertices[1].Position = new Vector3(10f, 10f, 0f);
            this.vertices[1].Color = Color.Yellow;
            this.vertices[2].Position = new Vector3(10f, 0f, -5f);
            this.vertices[2].Color = Color.Green;
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
            this.graphics.PreferredBackBufferWidth = 800;
            this.graphics.PreferredBackBufferHeight = 600;

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

            // Temporary
            this.setupVertices();

            // Creating our camera
            this.camera = new Camera(new Vector3(0, 0, 50), new Vector3(0, 0, 0), new Vector3(0, 1, 0));

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            float timeStep = (float)gameTime.ElapsedGameTime.TotalSeconds;
            // Changing the title of the game to contain the FPS.
            this.Window.Title = "Graphics Tutorial | FPS: " + this.frameRateCounter.FrameRate;

            // Spin the triangle!
            this.angle += 0.05f;


            #region Keyboard stuff
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }
            #endregion

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Enabling/disabling culling (not drawing certain triangles).
            this.GraphicsDevice.RasterizerState = new RasterizerState
            {
                CullMode = CullMode.None
            };

            // Background color of the image
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Changing the world accordingly to what the camera sees
            this.effect.Projection = this.camera.ProjectionMatrix;
            this.effect.View = this.camera.ViewMatrix;
            this.effect.World = Matrix.Identity;

            foreach (EffectPass pass in this.effect.CurrentTechnique.Passes)
            {
                pass.Apply();
            }

            this.GraphicsDevice.DrawUserPrimitives(
                PrimitiveType.TriangleList,
                this.vertices, 0, 1,
                VertexPositionColor.VertexDeclaration);

            base.Draw(gameTime);
        }
    }
}
