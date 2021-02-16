using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lesson_07
{
    public class Triangle
    {
        #region Member Variables
        // New code
        public VertexPositionNormalTextureColor[] Vertices { get; set; }
        //
        public Vector3 Position { get; set; }
        static public GraphicsDeviceManager Graphics { private get; set; }
        #endregion

        #region Constructors
        public Triangle(VertexPositionNormalTextureColor[] vertices, Vector3 position)
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
        // New code
        private void Initialise(ref VertexPositionNormalTextureColor[] vertices, ref Vector3 position)
        //
        {
            Vertices = vertices;
            Position = position;
        }

        /// <summary>
        /// Draw the triangle using a triangle list
        /// </summary>
        public void Draw()
        {
            // New code
            Graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTextureColor>(PrimitiveType.TriangleList,
                    Vertices, 0, 1);
            //
        }
        #endregion
    }
}
