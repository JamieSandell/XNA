using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lesson_05
{
    public class Quad
    {
        #region Member Variables
        public VertexPositionColor[] Vertices { get; set; }
        public Vector3 Position { get; set; }
        static public GraphicsDeviceManager Graphics { private get; set; }
        #endregion

        #region Constructors
        public Quad(VertexPositionColor[] vertices, Vector3 position)
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
        /// Draw the quad using a triangle strip.
        /// </summary>
        public void Draw()
        {
            Graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip,
                Vertices, 0, 2);
        }
        #endregion
    }
}
