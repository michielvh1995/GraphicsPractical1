using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicsPractical1
{
    class Terrain
    {
        /*
          Chapter 6: The Terrain class, used for storing all data on the terrain.
          A seperate class is used to make our "engine" suited for all sizes (more dynamic)
          
         * 
         * 
          The width and height values of the Terrain are the number of vertices horizontally and vertically respectively.
         
          The color and the position of the vertices of the terrain will be stored in the "vertices" variable. 
          The VertexPositionColor variable type can be seen as a tuple consisting of a position (a Vector in a 3D plane) and a color (RGBA).
        */
        private int width;
        private int height;

        private VertexPositionColor[] vertices;

        public int Width
        {
            get { return this.width; }
        }
        public int Height
        {
            get { return this.height; }
        }

        // The constructor method
        public Terrain(float[,] heightData)
        {
            this.width = heightData.GetLength(0);
            this.height = heightData.GetLength(1);
            VertexPositionColor[] heightDataVertices = this.loadVertices(heightData);
            this.setupVertices(heightDataVertices);
        }
        // Chapter 6:
        //
        private VertexPositionColor[] loadVertices(float[,] heightData)
        {
            VertexPositionColor[] vertices = new VertexPositionColor[this.width * this.height];
            for (int x = 0; x < this.width; x++)
                for (int y = 0; y < this.height; y++)
                {
                    int v = x + y * this.width;
                    vertices[v].Position = new Vector3(x, heightData[x, y], -y);
                    vertices[v].Color = Color.White;
                }
            return vertices;
        }

        /*
          The first line of the setupVerices(..) function intializes the vertices array to be just big enough for the amount of vertices there will have to be stored.
          If so, then why *3? Every triangle has 3 corners, each of which have a position and a color.
         * 
         * Step 2 of Chapter 6: Replaced the *3 wih *6, since we will be drawing twice as many triangles now, by implementing 3D
         * 
        */
        private void setupVertices(VertexPositionColor[] heightDataVertices)
        {
            this.vertices = new VertexPositionColor[(this.width - 1) * (this.height - 1) * 6];
            int counter = 0;
            for
            (int x = 0; x < this.width - 1; x++)
                for (int y = 0; y < this.height - 1; y++)
                {
                    int lowerLeft = x + y * this.width;
                    int lowerRight = (x + 1) + y * this.width;
                    int topLeft = x + (y + 1) * this.width;
                    int topRight = (x + 1) + (y + 1) * this.width;

                    this.vertices[counter++] = heightDataVertices[topLeft];
                    this.vertices[counter++] = heightDataVertices[lowerRight];
                    this.vertices[counter++] = heightDataVertices[lowerLeft];

                    this.vertices[counter++] = heightDataVertices[topLeft];
                    this.vertices[counter++] = heightDataVertices[topRight];
                    this.vertices[counter++] = heightDataVertices[lowerRight];
                }
        }

        // Chapter 6: the Terrain's Draw(..) function, used to draw this thing on the screen when called by the main functions
        public void Draw(GraphicsDevice device)
        {
            device.DrawUserPrimitives(
                PrimitiveType.TriangleList, this.vertices, 0,
                this.vertices.Length / 3, VertexPositionColor.VertexDeclaration
                );
        }
    }
}
