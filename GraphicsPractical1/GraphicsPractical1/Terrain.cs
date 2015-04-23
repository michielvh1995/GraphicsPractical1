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

        // Chapter 6:
        //
        private VertexPositionColor[] loadVertices()
        {
            VertexPositionColor[] vertices = new VertexPositionColor[this.width * this.height];
            for (int x = 0; x < this.width; x++)
                for (int y = 0; y < this.height; y++)
                {
                    int v = x + y * this.width;
                    vertices[v].Position = new Vector3(x, 0, -y);
                    vertices[v].Color = Color.White;
                }
            return vertices;
        }
    }
}
