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
          The VertexPositionColorNormal variable type can be seen as a tuple consisting of a position (a Vector in a 3D plane) and a color (RGBA).
        */
        private int width;
        private int height;

        private VertexPositionColorNormal[] vertices;

        public int Width
        {
            get { return this.width; }
        }
        public int Height
        {
            get { return this.height; }
        }

        // The constructor method
        public Terrain(HeightMap heightMap, float heightScale)
        {
            this.width = heightMap.Width;
            this.height = heightMap.Height;

            VertexPositionColorNormal[] heightDataVertices = this.loadVertices(heightMap, heightScale);
            this.setupVertices(heightDataVertices);

            // Chapter 7:
            this.calculateNormals();
        }
        // Chapter 6:
        //
        // Chapter 7:
        // Changed all occurences of VertexPositionColor to VertexPositionColorNormalNormal
        private VertexPositionColorNormal[] loadVertices(HeightMap heightMap, float heightScale)
        {
            VertexPositionColorNormal[] vertices = new VertexPositionColorNormal[this.width * this.height];

            for (int x = 0; x < this.width; ++x)
                for (int y = 0; y < this.height; ++y)
                {
                    int v = x + y * this.width;
                    float h = heightMap[x, y] * heightScale;
                    vertices[v].Position = new Vector3(x, h, -y);
                    vertices[v].Color = Color.Green;
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
        private void setupVertices(VertexPositionColorNormal[] heightDataVertices)
        {
            this.vertices = new VertexPositionColorNormal[(this.width - 1) * (this.height - 1) * 6];
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

        // Chapter 7:
        // Calculates the normals of each of the vertices
        private void calculateNormals()
        {
            for (int i = 0; i < this.vertices.Length / 3; i++)
            {
                VertexPositionColorNormal v1 = this.vertices[i * 3];
                VertexPositionColorNormal v2 = this.vertices[i * 3 + 1];
                VertexPositionColorNormal v3 = this.vertices[i * 3 + 2];

                Vector3 side1 = v3.Position - v1.Position;
                Vector3 side2 = v2.Position - v1.Position;
                Vector3 normal = Vector3.Cross(side1, side2);

                normal.Normalize();
                this.vertices[i * 3].Normal = normal;
                this.vertices[i * 3 + 1].Normal = normal;
                this.vertices[i * 3 + 2].Normal = normal;
            }
        }

        // Chapter 6: the Terrain's Draw(..) function, used to draw this thing on the screen when called by the main functions
        public void Draw(GraphicsDevice device)
        {
            device.DrawUserPrimitives(
                PrimitiveType.TriangleList, this.vertices, 0,
                this.vertices.Length / 3, VertexPositionColorNormal.VertexDeclaration
                );
        }
    }
}
