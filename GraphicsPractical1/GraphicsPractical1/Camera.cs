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
         * up           : What direction the camera sees as pointing upwards
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

        public Vector3 Up
        {
            get { return this.up; }
        }

        public Vector3 Forward
        {
            get { return this.forward; }
            set
            {
                this.forward = value;
                Vector3 newEye = focus - Forward;
                if (eye != newEye)
                {
                    eye = newEye;
                    this.updateViewMatrix();
                }
            }
        }
        // These matrices have no set-function, because the projection matrix, once set, probably never has to be changed and the view matrix only depends on the eye and focus vector, so it makes more sense to  adjust  the  view  matrix  by  changing  those  properties.
        // However, the viewMatri has to be recalculated each time the eye- or focus vector changes.
        public Matrix ViewMatrix
        {
            get
            {
                return this.viewMatrix;
            }
        }

        public Matrix ProjectionMatrix
        {
            get { return this.projectionMatrix; }
        }

        // These vectors have a set function, so they can be updated when the user tells the camera to move in the world
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

        #region BONUS
        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount"> The amount the camera will move in the 3D world </param>
        public void MoveForward(float amount)
        {
            this.forward.Normalize();
            this.eye += this.forward * amount;
            this.updateViewMatrix();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount"> The amount the camera will move in the 3D world </param>
        public void Strafe(float amount)
        {
            var left = Vector3.Cross(this.up, this.forward);
            left.Normalize();

            this.eye += left * amount;

            this.updateViewMatrix();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount"> The amount the camera will move in the 3D world </param>
        public void Raise(float amount)
        {
            this.up.Normalize();
            this.eye += up * amount;

            this.updateViewMatrix();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount"></param>
        public void Rotate(float amount)
        {
            this.up.Normalize();
            Vector3 x = new Vector3(1, 0, 0);

            this.forward = Vector3.Transform(this.forward, Matrix.CreateFromAxisAngle(this.up, MathHelper.ToRadians(amount)));
            this.focus = forward - eye;

            this.updateViewMatrix();
        }

        /// <summary>
        /// Turns the camera up or downwards
        /// </summary>
        /// <param name="amount"></param>
        public void Pitch(float amount)
        {
            this.up.Normalize();
            var left = Vector3.Cross(this.up, this.forward);
            left.Normalize();

            Focus = Vector3.Transform(this.focus, Matrix.CreateFromAxisAngle(left, MathHelper.ToRadians(amount)));

            this.updateViewMatrix();
        }

        #endregion

        public Camera(Vector3 camEye, Vector3 camFocus, Vector3 camUp, float hFOV = 60, float _aspectRatio = 4.0f / 3.0f)
        {
            this.up = camUp;
            this.eye = camEye;
            this.focus = camFocus;

            // Bonus:
            // Changable FoV, replaces the initial calcuation of the projection matrix
            this.updateFoV(hFOV);

            this.updateViewMatrix();
        }

        /// <summary>
        /// Normally the projectionMatrix would only be created and filled in once.
        /// However, we allowed the FoV to be changeable.
        /// The result? Everytime the FoV changes the projectionMatrix needs to be recalculated.
        /// The projectionMatrix determines how the camera looks at the scene.
        /// </summary>
        /// <param name="fov"> The horizontal view angle a user wants in degrees </param>
        public void updateFoV(float fov)
        {
            // The aspect ratio influences the horizontal FoV, as it determines what the ratio between vertical and horizontal FoV is
            // Here, the vertical FoV is set to 45 degrees (in radians).
            // The aspectRatio needs to be a float with a relatively low value 
            //  (an aspectRatio of 2 corresponds with a 90 degree angle).
            float vFOV = MathHelper.PiOver4;
            float aspectRatio = MathHelper.ToRadians(fov / vFOV);

            // Argument 1: The vertical field of view
            // Argument 2: The aspect ratio of the image, a greater ratio allows the camera to see more horizontally
            // Argument 3: The minimum viewing distance, objects that are too close wont be rendered
            // Argument 4: The maximum viewing distance, objects that are too far away wont be rendered
            this.projectionMatrix = Matrix.CreatePerspectiveFieldOfView(vFOV, aspectRatio, 1.0f, 500.0f);
        }

        // This method creates the view matrix.
        // The viewMatrix stores the position and orientation of the camera, through which we look at the scene.
        private void updateViewMatrix()
        {
            // Argument 1: the position of the camera
            // Argument 2: the direction it's looking in
            // Argument 3: what direction is considered to be up
            this.forward = this.focus - this.eye;
            this.viewMatrix = Matrix.CreateLookAt(this.eye, this.forward, this.up);
        }
    }
}
