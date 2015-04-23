using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicsPractical1
{
    class Camera
    {
        /*
         The Variables:
          Matrices:
         * viewMatrix       : 
         * projectionMatrix : 
          
          Vertices:
         * up           : 
         * eye          : The position of the camera in the 3D world
         * focus        : The exact point the camera is looking twards in the 3D world
         * aspectRatio  : the ratio width : height of the screen, for Chapter 1 we've set the screen to 800 by 600, making it ~1.3333
        */
        private Matrix viewMatrix;
        private Matrix projectionMatrix;

        private Vector3 up;
        private Vector3 eye;
        private Vector3 focus;
        private Vector3 forward;

        public Vector3 Forward
        {
            get { return this.forward; }
            set
            {
                this.forward = value;
                Vector3 newEye = focus - Forward;
                if (eye != newEye)
                    eye = newEye;
            }
        }

        // These matrices have no set-function, because the projection matrix, once set, probably never has to be changed and the view matrix only depends on the eye and focus vector, so it makes more sense to  adjust  the  view  matrix  by  changing  those  properties.
        // However, the viewMatri has to be recalculated each time the eye- or focus vector changes.
        public Matrix ViewMatrix
        {
            get { return this.viewMatrix; }

            // Testing: adding 3 dimensinal rotation
            set { viewMatrix = value; }
        }

        public Matrix ProjectionMatrix
        {
            get { return this.projectionMatrix; }
        }

        // These do have a set method, so they can be adjusted using the keyboard and mouse.
        // Each time one of these changes the viewMatrix will have to be recaluted (as the camera looks at a different thing).
        public Vector3 Eye
        {
            get { return this.eye; }
            set
            {
                this.eye = value;
                this.updateViewMatrix();
            }
        }
        public Vector3 Focus
        {
            get { return this.focus; }
            set
            {
                this.focus = value;
                this.updateViewMatrix();
            }
        }

        // Used for rotating the thing up and down
        public Vector3 Up
        {
            get { return this.up; }
            set
            {
                this.up = value;
                updateViewMatrix();
            }
        }

        public void Pitch(float amount)
        {
            Forward.Normalize();

            var left = Vector3.Cross(Up, Forward);
            left.Normalize();

            Console.WriteLine("F: " + Forward + " L: " + left);

            Forward = Vector3.Transform(Forward, Matrix.CreateFromAxisAngle(left, MathHelper.ToRadians(amount)));
            Up = Vector3.Transform(Up, Matrix.CreateFromAxisAngle(left, MathHelper.ToRadians(amount)));
            Console.WriteLine("F: " + Forward + " U: " + Up);
        }

        public Camera(Vector3 camEye, Vector3 camFocus, Vector3 camUp, float aspectRatio = 4.0f / 3.0f)
        {
            this.up = camUp;
            this.eye = camEye;
            this.focus = camFocus;

            this.updateViewMatrix();

            // Filling in the projectionMatrix (needs to be done once)
            // This determines how the camera will look at the scene.
            // The first argument is responsible for the FoV options (the view angle of the camera).
            // The second sets the aspect ratio
            // Argument 3: minimum viewing distance, objects that are too close wont be rendered
            // Argument 4: maximum viewing distance, objects that are too far away wont be rendered
            this.projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 1.0f, 300.0f);
        }

        // This method creates the view matrix, a matrix that stores the position and orientation of the camera, through which we look at the scene.
        private void updateViewMatrix()
        {
            /* 
              Argument 1: the position of the camera
              Argument 2: the direction it's looking in
              Argument 3: what direction is considered to be up
            */
            this.viewMatrix = Matrix.CreateLookAt(this.eye, this.focus, this.up);
            this.Forward = this.focus - this.eye;

        }
    }
}
