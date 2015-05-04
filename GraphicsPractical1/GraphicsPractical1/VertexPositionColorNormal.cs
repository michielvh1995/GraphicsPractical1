using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicsPractical1
{
    /// <summary>
    /// The VertexPositionColorNormal struct are actually a bunch of vertices combined in one struct
    /// These vertices are: the position of a point, the color of a point and the normal of this point
    /// This struct does not calculate anything and is only used to store data
    /// </summary>
    struct VertexPositionColorNormal : IVertexType
    {
        public Vector3 Position;
        public Color Color;
        public Vector3 Normal;

        public VertexPositionColorNormal(Vector3 position, Color color, Vector3 normal)
        {
            this.Position = position;
            this.Color = color;
            this.Normal = normal;
        }

        public static VertexElement[] VertexElements =
        {
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(sizeof(float)  *  3, VertexElementFormat.Color, VertexElementUsage.Color, 0),
            new VertexElement(sizeof(float) * 3 + 4, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
        };

        public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(VertexPositionColorNormal.VertexElements);
        
        VertexDeclaration IVertexType.VertexDeclaration
        {
            get
            {
                return VertexPositionColorNormal.VertexDeclaration;
            }
        }
    }
}
