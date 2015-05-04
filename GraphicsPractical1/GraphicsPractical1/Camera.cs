using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicsPractical1
{
    class Camera
    {
        // The Variables:
        //  Matrices:
        //  * viewMatrix       : What part of the world the camera can see
        //  * projectionMatrix : How the "world" is projected on the screen and what transformations are applied
        //
        //  Vertices:
        //  * up           : What direction the camera sees as pointing upwards
        //  * eye          : The position of the camera in the 3D world
        //  * focus        : The exact point the camera is looking twards in the 3D world
        //  * aspectRatio  : the ratio width : height of the screen, for Chapter 1 we've set the screen to 800 by 600, making it ~1.3333
        // The Bonus Assignment Variables:
        // * forward (vector): The direction the camera is pointing in, used in some functions
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

        // These matrices (the ViewMatrix and ProjectionMatrix) have no set-function.
        // This is because the ViewMatrix is only updated by functions inside the camera class, and only whenever either the Eye or Focus vector changes values.
        // The ProjectionMatrix is only updated whenever the FoV changes. This again is calculated inside the camera class.
        public Matrix ViewMatrix
        {
            get { return this.viewMatrix; }
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
        /// Moves the camera forwards in the 3D world
        /// The forward vector is normalized as this will make sure that, when multiplied, the camera will move the amount we have put in
        /// Everytime the camera moves, the viewMatrix will have to be updated as it now looks at the world from a different angle
        /// </summary>
        /// <param name="amount"> The amount the camera will move in the 3D world </param>
        public void MoveForward(float amount)
        {
            this.forward.Normalize();
            this.eye += this.forward * amount;
            this.updateViewMatrix();
        }

        /// <summary>
        /// Moves the camera sideways in the 3D world
        /// For this the left vector is calculated, to know which direction is sideways
        /// Everytime the camera moves, the viewMatrix will have to be updated as it now looks at the world from a different angle
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
        /// Moves the camera up in the 3D world by a specified amount
        /// Everytime the camera moves, the viewMatrix will have to be updated as it now looks at the world from a different angle
        /// </summary>
        /// <param name="amount"> The amount the camera will move in the 3D world </param>
        public void Raise(float amount)
        {
            this.up.Normalize();
            this.eye += up * amount;
            this.updateViewMatrix();
        }

        /// <summary>
        /// Rotates the camera to the left or right by a specified amount in degrees
        /// The rotation is done around the up axis of the camera
        /// Everytime the camera moves, the viewMatrix will have to be updated as it now looks at the world from a different angle
        /// </summary>
        /// <param name="amount"> The amount of degrees the camera will rotate </param>
        public void Rotate(float amount)
        {
            this.up.Normalize();

            this.forward = Vector3.Transform(this.forward, Matrix.CreateFromAxisAngle(this.up, MathHelper.ToRadians(amount)));
            this.focus = forward - eye;

            this.updateViewMatrix();
        }

        /// <summary>
        /// Rotates the camera up or downwards by a specified amount in degrees
        /// Rotation is done around the left vector - the cross-product of the up and forward vectors
        /// Everytime the camera moves, the viewMatrix will have to be updated as it now looks at the world from a different angle
        /// </summary>
        /// <param name="amount"> The amount of degrees the camera will rotate </param>
        public void Pitch(float amount)
        {
            this.up.Normalize();
            var left = Vector3.Cross(this.up, this.forward);
            left.Normalize();

            Focus = Vector3.Transform(this.focus, Matrix.CreateFromAxisAngle(left, MathHelper.ToRadians(amount)));

            this.updateViewMatrix();
        }

        #endregion

        // the constructor method
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
            this.viewMatrix = Matrix.CreateLookAt(this.eye, this.focus, this.up);
        }
    }
}
