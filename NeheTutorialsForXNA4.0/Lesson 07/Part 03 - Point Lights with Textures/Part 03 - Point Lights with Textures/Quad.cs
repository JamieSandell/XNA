using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lesson_07
{
    public class Quad
    {
        #region Member Variables
        public VertexPositionNormalTextureColor[] Vertices { get; set; }
        public Vector3 Position { get; set; }
        static public GraphicsDeviceManager Graphics { private get; set; }
        #endregion

        #region Constructors
        public Quad(VertexPositionNormalTextureColor[] vertices, Vector3 position, Texture2D texture)
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
        private void Initialise(ref VertexPositionNormalTextureColor[] vertices, ref Vector3 position)
        {
            Vertices = vertices;
            Position = position;
        }

        /// <summary>
        /// Draw the quad using a triangle strip.
        /// </summary>
        public void Draw()
        {
            Graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTextureColor>(PrimitiveType.TriangleStrip,
                Vertices, 0, 2);
        }
        #endregion
    }
}
