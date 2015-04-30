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

        // Chapter 7
        private VertexPositionColorNormal[] vertices;

        // Chapter 8
        private short[] indices;
        private VertexBuffer vertexBuffer;
        private IndexBuffer indexBuffer;

        public int Width
        {
            get { return this.width; }
        }
        public int Height
        {
            get { return this.height; }
        }

        // The constructor method
        public Terrain(HeightMap heightMap, float heightScale, GraphicsDevice device)
        {
            this.width = heightMap.Width;
            this.height = heightMap.Height;

            VertexPositionColorNormal[] heightDataVertices = this.loadVertices(heightMap, heightScale);

            // Chapter 8:
            // Changed the use of vertices[] as well
            this.vertices = this.loadVertices(heightMap, heightScale);
            this.setupIndices();

            // Chapter 7:
            this.calculateNormals();

            // Chapter 8:
            this.copyToBuffers(device);
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
        /*
            Chapter 8:
              Changed SetupVertices(..) to SetupIndices()
        */
        private void setupIndices()
        {
            this.indices = new short[(this.width - 1) * (this.height - 1) * 6];

            int counter = 0;

            for (int x = 0; x < this.width - 1; x++)
                for (int y = 0; y < this.height - 1; y++)
                {
                    int lowerLeft = x + y * this.width;
                    int lowerRight = (x + 1) + y * this.width;
                    int topLeft = x + (y + 1) * this.width;
                    int topRight = (x + 1) + (y + 1) * this.width;

                    this.indices[counter++] = (short)topLeft;
                    this.indices[counter++] = (short)lowerRight;
                    this.indices[counter++] = (short)lowerLeft;
                    this.indices[counter++] = (short)topLeft;
                    this.indices[counter++] = (short)topRight;
                    this.indices[counter++] = (short)lowerRight;
                }
        }

        // Chapter 7:
        // Calculates the normals of each of the vertices
        // Chapter 8:
        // Improved calculation method
        private void calculateNormals()
        {
            for (int i = 0; i < this.indices.Length / 3; i++)
            {
                short i1 = this.indices[i * 3];
                short i2 = this.indices[i * 3 + 1];
                short i3 = this.indices[i * 3 + 2];

                Vector3 side1 = this.vertices[i3].Position - this.vertices[i1].Position;
                Vector3 side2 = this.vertices[i2].Position - this.vertices[i1].Position;
                Vector3 normal = Vector3.Cross(side1, side2);

                normal.Normalize();

                this.vertices[i1].Normal += normal;
                this.vertices[i2].Normal += normal;
                this.vertices[i3].Normal += normal;
            }

            for (int i = 0; i < this.vertices.Length; i++)
                this.vertices[i].Normal.Normalize();
        }

        // Chapter 8:
        //
        private void copyToBuffers(GraphicsDevice device)
        {
            this.vertexBuffer = new VertexBuffer(
                device, VertexPositionColorNormal.VertexDeclaration,
                this.vertices.Length, BufferUsage.WriteOnly);
            this.vertexBuffer.SetData(this.vertices);

            this.indexBuffer = new IndexBuffer(device, typeof(short), this.indices.Length, BufferUsage.WriteOnly);
            this.indexBuffer.SetData(this.indices);

            device.Indices = this.indexBuffer;
            device.SetVertexBuffer(this.vertexBuffer);
        }
        // Chapter 6: the Terrain's Draw(..) function, used to draw this thing on the screen when called by the main functions
        public void Draw(GraphicsDevice device)
        {
            device.DrawIndexedPrimitives(
                PrimitiveType.TriangleList, 0, 0,
                this.vertices.Length, 0, this.indices.Length / 3);
        }
    }
}
