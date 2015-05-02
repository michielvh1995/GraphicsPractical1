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

        // Bonus: changable FoV
        private float aspectRatio;

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

        #region BONUS
        public void MoveForward(float amount)
        {
            this.forward.Normalize();
            this.eye += this.forward * amount;
            this.updateViewMatrix();
        }

        public void Rotate(float amount)
        {
            // Omzetten naar de focus
            // Forward = target - position
            // Forward - eye = focus
            this.up.Normalize();
            Vector3 x = new Vector3(1, 0, 0);

            this.forward = Vector3.Transform(this.forward, Matrix.CreateFromAxisAngle(this.up, MathHelper.ToRadians(amount)));
            this.focus = forward - eye;

            this.updateViewMatrix();
        }

        public void Pitch(float amount)
        {
            this.up.Normalize();
            var left = Vector3.Cross(this.up, this.forward);
            left.Normalize();

            this.focus = Vector3.Transform(this.focus, Matrix.CreateFromAxisAngle(left, MathHelper.ToRadians(amount)));

            this.updateViewMatrix();
        }

        public void Strafe(float amount)
        {
            var left = Vector3.Cross(this.up, this.forward);
            left.Normalize();

            this.eye += left * amount;

            this.updateViewMatrix();
        }

        public void Raise(float amount)
        {
            this.up.Normalize();
            this.eye += up * amount;

            this.updateViewMatrix();
        }
        #endregion



        public Camera(Vector3 camEye, Vector3 camFocus, Vector3 camUp, float hFOV = 60, float _aspectRatio = 4.0f / 3.0f)
        {
            this.up = camUp;
            this.eye = camEye;
            this.focus = camFocus;

            // Bonus:
            // Changable FoV
            this.updateFoV(hFOV);

            this.updateViewMatrix();
        }

        public void updateFoV(float fov)
        {
            // Filling in the projectionMatrix, with the added Bonus nolonger needs to be only filled once, but everytime the FoV changes.
            // This determines how the camera will look at the scene.
            // The first argument is responsible for the FoV options (the view angle of the camera).
            //      (Field of view in the y direction, in radians) 
            // Argument 2: sets the aspect ratio
            // Argument 3: minimum viewing distance, objects that are too close wont be rendered
            // Argument 4: maximum viewing distance, objects that are too far away wont be rendered

            // Te groter argument 1 is, te groter de FoV

            // The FoV here is in degrees, thus needs to be converted to radians first. 180 degrees = 1 Pi radian

            // The aspectRatio variable influences the horizontal FoV, while the fieldOfView variable influences the vertical FoV.
            // The base FoVs are: V: 1/4 Pi = 45 deg
            //                    H: 1.3* V = 60 deg = 1/3 Pi
            // Increasing horizontal FoV to a specified amount would require us change the FoV to an aspect ratio.
            // where 60 degrees coressponds with a = 1.333, 90 degrees would correspond with a = 2.
            float vFOV = MathHelper.PiOver4;


            this.aspectRatio = MathHelper.ToRadians(fov / vFOV);

            this.projectionMatrix = Matrix.CreatePerspectiveFieldOfView(vFOV, this.aspectRatio, 1.0f, 500.0f);
        }

        // This method creates the view matrix, a matrix that stores the position and orientation of the camera, through which we look at the scene.
        private void updateViewMatrix()
        {
            /* 
              Argument 1: the position of the camera
              Argument 2: the direction it's looking in
              Argument 3: what direction is considered to be up
            */
            this.forward = this.focus - this.eye;
            this.viewMatrix = Matrix.CreateLookAt(this.eye, this.forward, this.up);
        }
    }
}
