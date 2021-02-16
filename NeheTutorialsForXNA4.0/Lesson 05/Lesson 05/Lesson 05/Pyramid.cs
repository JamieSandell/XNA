using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lesson_05
{
    public class Pyramid
    {
        #region Member Variables
        public Vector3 Position { get; set; }
        public Color Color { get; set; }
        public float Size 
        {
            get
            {
                return m_size;
            }
            set
            {
                m_size = value;
                //If the object has been created, re-initialise the vertices
                if (m_created)
                    InitialiseVertices();
            }
        }
        static public GraphicsDeviceManager Graphics
        {
            set
            {
                Triangle.Graphics = value;
            }
        }

        private Triangle m_frontFace, m_leftFace, m_rightFace, m_backFace;
        private bool m_created; //Used to determine whether or not the object has been created.
        private float m_size;
        #endregion

        #region Constructors
        public Pyramid(Vector3 position, float m_size, Color color)
        {
            Initialise(ref position, m_size, ref color);
        }
        #endregion

        #region Member Methods
        /// <summary>
        /// Draw the shape
        /// </summary>
        public void Draw() 
        {
            m_frontFace.Draw();
            m_leftFace.Draw();
            m_rightFace.Draw();
            m_backFace.Draw();
        }

        /// <summary>
        /// Initialise the member variables. Calls InitialiseVertices
        /// </summary>
        /// <param name="position">The position member variable to initialise.</param>
        /// <param name="m_size">The m_size member variable to initialise.</param>
        /// <param name="color">The color member variable to initialise.</param>
        private void Initialise(ref Vector3 position, float m_size, ref Color color)
        {
            m_created = false; //This a flag to prevent the vertices been initialise twice during the constructor
                // because setting the Size value also initialises the vertices.
            Position = position;
            Size = m_size;
            Color = color;

            InitialiseVertices();
            m_created = true;
        }

        /// <summary>
        /// Initialise the vertices of the shape.
        /// </summary>
        private void InitialiseVertices()
        {
            VertexPositionColor[] triangleVerticesFront = new VertexPositionColor[3];
            Vector3 vertexPositionFront = new Vector3();
            // Front face bottom left vertex
            vertexPositionFront.X = -(m_size / 2);
            vertexPositionFront.Y = -(m_size / 2);
            vertexPositionFront.Z = +(m_size / 2);
            triangleVerticesFront[0].Position = vertexPositionFront;
            triangleVerticesFront[0].Color = Color.Green;
            // Front face top middle vertex
            vertexPositionFront.X = 0.0f;
            vertexPositionFront.Y = +(m_size / 2);
            vertexPositionFront.Z = 0.0f;
            triangleVerticesFront[1].Position = vertexPositionFront;
            triangleVerticesFront[1].Color = Color.Red;
            // Front face bottom right vertex
            vertexPositionFront.X = +(m_size / 2);
            vertexPositionFront.Y = -(m_size / 2);
            vertexPositionFront.Z = +(m_size / 2);
            triangleVerticesFront[2].Position = vertexPositionFront;
            triangleVerticesFront[2].Color = Color.Blue;

            Vector3 frontFacePosition = new Vector3(0.0f, 0.0f, +(m_size / 2)); // The centre of the face
            m_frontFace = new Triangle(triangleVerticesFront, frontFacePosition);

            VertexPositionColor[] triangleVerticesLeft = new VertexPositionColor[3];
            Vector3 vertexPositionLeft = new Vector3();
            // Left face bottom left vertex
            vertexPositionLeft.X = -(m_size / 2);
            vertexPositionLeft.Y = -(m_size / 2);
            vertexPositionLeft.Z = -(m_size / 2);
            triangleVerticesLeft[0].Position = vertexPositionLeft;
            triangleVerticesLeft[0].Color = Color.Blue;
            // Left face top middle vertex
            vertexPositionLeft.X = 0.0f;
            vertexPositionLeft.Y = +(m_size / 2);
            vertexPositionLeft.Z = 0.0f;
            triangleVerticesLeft[1].Position = vertexPositionLeft;
            triangleVerticesLeft[1].Color = Color.Red;
            // Left face bottom right vertex
            vertexPositionLeft.X = -(m_size / 2);
            vertexPositionLeft.Y = -(m_size / 2);
            vertexPositionLeft.Z = +(m_size / 2);
            triangleVerticesLeft[2].Position = vertexPositionLeft;
            triangleVerticesLeft[2].Color = Color.Green;

            Vector3 leftFacePosition = new Vector3(-(m_size / 2), 0.0f, 0.0f);
            m_leftFace = new Triangle(triangleVerticesLeft, leftFacePosition);

            VertexPositionColor[] triangleVerticesRight = new VertexPositionColor[3];
            Vector3 vertexPositionRight = new Vector3();
            // Right face bottom left vertex
            vertexPositionRight.X = +(m_size / 2);
            vertexPositionRight.Y = -(m_size / 2);
            vertexPositionRight.Z = +(m_size / 2);
            triangleVerticesRight[0].Position = vertexPositionRight;
            triangleVerticesRight[0].Color = Color.Blue;
            // Right face top middle vertex
            vertexPositionRight.X = 0.0f;
            vertexPositionRight.Y = +(m_size / 2);
            vertexPositionRight.Z = 0.0f;
            triangleVerticesRight[1].Position = vertexPositionRight;
            triangleVerticesRight[1].Color = Color.Red;
            // Right face bottom right vertex
            vertexPositionRight.X = +(m_size / 2);
            vertexPositionRight.Y = -(m_size / 2);
            vertexPositionRight.Z = -(m_size / 2);
            triangleVerticesRight[2].Position = vertexPositionRight;
            triangleVerticesRight[2].Color = Color.Green;

            Vector3 rightFacePosition = new Vector3(-(m_size / 2), 0.0f, 0.0f);
            m_rightFace = new Triangle(triangleVerticesRight, rightFacePosition);

            VertexPositionColor[] triangleVerticesBack = new VertexPositionColor[3];
            Vector3 vertexPositionBack = new Vector3();
            // Back face bottom left vertex
            vertexPositionBack.X = +(m_size / 2);
            vertexPositionBack.Y = -(m_size / 2);
            vertexPositionBack.Z = -(m_size / 2);
            triangleVerticesBack[0].Position = vertexPositionBack;
            triangleVerticesBack[0].Color = Color.Green;
            // Back face top middle vertex
            vertexPositionBack.X = 0.0f;
            vertexPositionBack.Y = +(m_size / 2);
            vertexPositionBack.Z = 0.0f;
            triangleVerticesBack[1].Position = vertexPositionBack;
            triangleVerticesBack[1].Color = Color.Red;
            // Back face bottom right vertex
            vertexPositionBack.X = -(m_size / 2);
            vertexPositionBack.Y = -(m_size / 2);
            vertexPositionBack.Z = -(m_size / 2);
            triangleVerticesBack[2].Position = vertexPositionBack;
            triangleVerticesBack[2].Color = Color.Blue;

            Vector3 backFacePosition = new Vector3(0.0f, 0.0f, -(m_size / 2));
            m_backFace = new Triangle(triangleVerticesBack, backFacePosition);
        }
        #endregion
    }
}
