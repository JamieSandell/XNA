using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lesson_03
{
    public class Quad
    {
        #region Member Variables
        public float Width { get; set; }
        public float Height { get; set; }
        public Vector3 Position { get; set; }
        public Color Color { get; set; }
        static public GraphicsDeviceManager Graphics { private get; set; }
        private VertexPositionColor[] m_Vertices;
        #endregion
        #region Constructors
        public Quad()
        {
            Width = 1.0f;
            Height = 1.0f;
            Position = new Vector3(0.0f, 0.0f, 0.0f);
            Color = Color.White;
            InitVertices();
        }

        public Quad(float width, float height, ref Vector3 position, ref Color color)
        {
            Width = width;
            Height = height;
            Position = position;
            Color = color;
            InitVertices();
        }
        #endregion
        #region Member Methods
        /// <summary>
        /// Initialise the four vertices of the quad in a clockwise manner.
        /// </summary>
        private void InitVertices()
        {
            m_Vertices = new VertexPositionColor[4];
            //bottom left corner
            m_Vertices[0].Position = new Vector3(-(Width / 2), -(Height / 2), 0.0f);
            //top left corner
            m_Vertices[1].Position = new Vector3(-(Width / 2), +(Height / 2), 0.0f);
            //bottom right corner
            m_Vertices[2].Position = new Vector3(+(Width / 2), -(Height / 2), 0.0f);
            //top right corner
            m_Vertices[3].Position = new Vector3(+(Width / 2), +(Height / 2), 0.0f);

            for (int i = 0; i < m_Vertices.Length; i++ )
            {
                m_Vertices[i].Color = Color;
            }
        }

        /// <summary>
        /// Draw the quad using a triangle strip.
        /// </summary>
        public void Draw()
        {
            Graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip,
                m_Vertices, 0, 2);
        }
        #endregion
    }
}
