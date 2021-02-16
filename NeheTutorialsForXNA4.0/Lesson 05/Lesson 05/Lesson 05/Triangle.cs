using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lesson_05
{
    public class Triangle
    {
        #region Member Variables
        public VertexPositionColor[] Vertices { get; set; }
        public Vector3 Position { get; set; }
        static public GraphicsDeviceManager Graphics { private get; set; }
        #endregion

        #region Constructors
        public Triangle(VertexPositionColor[] vertices, Vector3 position)
        {
            Initialise(ref vertices, ref position);
        }
        #endregion

        #region Member Methods
        /// <summary>
        /// Initialise the member variables
        /// </summary>
        /// <param name="vertices">The vertices member variable.</param>
        /// <param name="position">The position member variable.</param>
        private void Initialise(ref VertexPositionColor[] vertices, ref Vector3 position)
        {
            Vertices = vertices;
            Position = position;
        }

        /// <summary>
        /// Draw the triangle using a triangle list
        /// </summary>
        public void Draw()
        {
            Graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList,
                    Vertices, 0, 1);
        }
        #endregion
    }
}
