using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lesson_05
{
    public class Cube
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
                if (m_created) //Used to determine whether or not the object has been created.
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

        private Quad m_frontFace, m_backFace, m_topFace, m_bottomFace, m_leftFace, m_rightFace;
        private bool m_created; //Used to determine whether or not the object has been created.
        private float m_size;
        #endregion

        #region Constructors
        public Cube(Vector3 position, float m_size, Color color)
        {
            Initialise(ref position, m_size, ref color);
        }
        #endregion

        #region Member Methods
        public void Draw()
        {
            m_frontFace.Draw();
            m_leftFace.Draw();
            m_backFace.Draw();
            m_rightFace.Draw();
            m_topFace.Draw();
            m_bottomFace.Draw();
        }

        private void Initialise(ref Vector3 position, float m_size, ref Color color)
        {
            m_created = false;
            Position = position;
            Size = m_size;
            Color = color;

            InitialiseVertices();
            m_created = true;
        }

        private void InitialiseVertices()
        {
            float m_size = Size;

            VertexPositionColor[] quadVerticesFront = new VertexPositionColor[4];
            Vector3 vertexPositionFront = new Vector3();
            // Front face bottom left vertex
            vertexPositionFront.X = -(m_size / 2);
            vertexPositionFront.Y = -(m_size / 2);
            vertexPositionFront.Z = +(m_size / 2);
            quadVerticesFront[0].Color = Color.Red;
            quadVerticesFront[0].Position = vertexPositionFront;
            // Front face top left vertex
            vertexPositionFront.X = -(m_size / 2);
            vertexPositionFront.Y = +(m_size / 2);
            vertexPositionFront.Z = +(m_size / 2);
            quadVerticesFront[1].Color = Color.Red;
            quadVerticesFront[1].Position = vertexPositionFront;
            // Front face bottom right vertex
            vertexPositionFront.X = +(m_size / 2);
            vertexPositionFront.Y = -(m_size / 2);
            vertexPositionFront.Z = +(m_size / 2);
            quadVerticesFront[2].Color = Color.Red;
            quadVerticesFront[2].Position = vertexPositionFront;
            // Front face top right vertex
            vertexPositionFront.X = +(m_size / 2);
            vertexPositionFront.Y = +(m_size / 2);
            vertexPositionFront.Z = +(m_size / 2);
            quadVerticesFront[3].Color = Color.Red;
            quadVerticesFront[3].Position = vertexPositionFront;
            Vector3 frontFacePosition = new Vector3(0.0f, 0.0f, +(m_size / 2)); // The centre of the face
            m_frontFace = new Quad(quadVerticesFront, frontFacePosition);

            VertexPositionColor[] quadVerticesLeft = new VertexPositionColor[4];
            Vector3 vertexPositionLeft = new Vector3();
            // Left face bottom left vertex
            vertexPositionLeft.X = -(m_size / 2);
            vertexPositionLeft.Y = -(m_size / 2);
            vertexPositionLeft.Z = -(m_size / 2);
            quadVerticesLeft[0].Color = Color.Green;
            quadVerticesLeft[0].Position = vertexPositionLeft;
            // Left face top left vertex
            vertexPositionLeft.X = -(m_size / 2);
            vertexPositionLeft.Y = +(m_size / 2);
            vertexPositionLeft.Z = -(m_size / 2);
            quadVerticesLeft[1].Color = Color.Green;
            quadVerticesLeft[1].Position = vertexPositionLeft;
            // Left face bottom right vertex
            vertexPositionLeft.X = -(m_size / 2);
            vertexPositionLeft.Y = -(m_size / 2);
            vertexPositionLeft.Z = +(m_size / 2);
            quadVerticesLeft[2].Color = Color.Green;
            quadVerticesLeft[2].Position = vertexPositionLeft;
            // Left face top right vertex
            vertexPositionLeft.X = -(m_size / 2);
            vertexPositionLeft.Y = +(m_size / 2);
            vertexPositionLeft.Z = +(m_size / 2);
            quadVerticesLeft[3].Color = Color.Green;
            quadVerticesLeft[3].Position = vertexPositionLeft;
            Vector3 leftFacePosition = new Vector3(-(m_size / 2), 0.0f, 0.0f);
            m_leftFace = new Quad(quadVerticesLeft, leftFacePosition);

            VertexPositionColor[] quadVerticesBack = new VertexPositionColor[4];
            Vector3 vertexPositionBack = new Vector3();
            // Back face bottom left vertex
            vertexPositionBack.X = +(m_size / 2);
            vertexPositionBack.Y = -(m_size / 2);
            vertexPositionBack.Z = -(m_size / 2);
            quadVerticesBack[0].Color = Color.Blue;
            quadVerticesBack[0].Position = vertexPositionBack;
            // Back face top left vertex
            vertexPositionBack.X = +(m_size / 2);
            vertexPositionBack.Y = +(m_size / 2);
            vertexPositionBack.Z = -(m_size / 2);
            quadVerticesBack[1].Color = Color.Blue;
            quadVerticesBack[1].Position = vertexPositionBack;
            // Back face bottom right vertex
            vertexPositionBack.X = -(m_size / 2);
            vertexPositionBack.Y = -(m_size / 2);
            vertexPositionBack.Z = -(m_size / 2);
            quadVerticesBack[2].Color = Color.Blue;
            quadVerticesBack[2].Position = vertexPositionBack;
            // Back face top right vertex
            vertexPositionBack.X = -(m_size / 2);
            vertexPositionBack.Y = +(m_size / 2);
            vertexPositionBack.Z = -(m_size / 2);
            quadVerticesBack[3].Color = Color.Blue;
            quadVerticesBack[3].Position = vertexPositionBack;
            Vector3 backFacePosition = new Vector3(0.0f, 0.0f, -(m_size / 2));
            m_backFace = new Quad(quadVerticesBack, backFacePosition);

            VertexPositionColor[] quadVerticesRight = new VertexPositionColor[4];
            Vector3 vertexPositionRight = new Vector3();
            // Right face bottom left vertex
            vertexPositionRight.X = +(m_size / 2);
            vertexPositionRight.Y = -(m_size / 2);
            vertexPositionRight.Z = +(m_size / 2);
            quadVerticesRight[0].Color = Color.DarkTurquoise;
            quadVerticesRight[0].Position = vertexPositionRight;
            // Right face top left vertex
            vertexPositionRight.X = +(m_size / 2);
            vertexPositionRight.Y = +(m_size / 2);
            vertexPositionRight.Z = +(m_size / 2);
            quadVerticesRight[1].Color = Color.DarkTurquoise;
            quadVerticesRight[1].Position = vertexPositionRight;
            // Right face bottom right vertex
            vertexPositionRight.X = +(m_size / 2);
            vertexPositionRight.Y = -(m_size / 2);
            vertexPositionRight.Z = -(m_size / 2);
            quadVerticesRight[2].Color = Color.DarkTurquoise;
            quadVerticesRight[2].Position = vertexPositionRight;
            // Right face top right vertex
            vertexPositionRight.X = +(m_size / 2);
            vertexPositionRight.Y = +(m_size / 2);
            vertexPositionRight.Z = -(m_size / 2);
            quadVerticesRight[3].Color = Color.DarkTurquoise;
            quadVerticesRight[3].Position = vertexPositionRight;
            Vector3 rightFacePosition = new Vector3(+(m_size / 2), 0.0f, 0.0f);
            m_rightFace = new Quad(quadVerticesRight, rightFacePosition);

            VertexPositionColor[] quadVerticesTop = new VertexPositionColor[4];
            Vector3 vertexPositionTop = new Vector3();
            // Top face bottom left vertex
            vertexPositionTop.X = -(m_size / 2);
            vertexPositionTop.Y = +(m_size / 2);
            vertexPositionTop.Z = +(m_size / 2);
            quadVerticesTop[0].Color = Color.Bisque;
            quadVerticesTop[0].Position = vertexPositionTop;
            // Top face top left vertex
            vertexPositionTop.X = -(m_size / 2);
            vertexPositionTop.Y = +(m_size / 2);
            vertexPositionTop.Z = -(m_size / 2);
            quadVerticesTop[1].Color = Color.Bisque;
            quadVerticesTop[1].Position = vertexPositionTop;
            // Top face bottom right vertex
            vertexPositionTop.X = +(m_size / 2);
            vertexPositionTop.Y = +(m_size / 2);
            vertexPositionTop.Z = +(m_size / 2);
            quadVerticesTop[2].Color = Color.Bisque;
            quadVerticesTop[2].Position = vertexPositionTop;
            // Top face top right vertex
            vertexPositionTop.X = +(m_size / 2);
            vertexPositionTop.Y = +(m_size / 2);
            vertexPositionTop.Z = -(m_size / 2);
            quadVerticesTop[3].Color = Color.Bisque;
            quadVerticesTop[3].Position = vertexPositionTop;
            Vector3 topFacePosition = new Vector3(0.0f, +(m_size / 2), 0.0f);
            m_topFace = new Quad(quadVerticesTop, topFacePosition);

            VertexPositionColor[] quadVerticesBottom = new VertexPositionColor[4];
            Vector3 vertexPositionBottom = new Vector3();
            // Bottom face bottom left vertex
            vertexPositionBottom.X = -(m_size / 2);
            vertexPositionBottom.Y = -(m_size / 2);
            vertexPositionBottom.Z = -(m_size / 2);
            quadVerticesBottom[0].Color = Color.Goldenrod;
            quadVerticesBottom[0].Position = vertexPositionBottom;
            // Bottom face top left vertex
            vertexPositionBottom.X = -(m_size / 2);
            vertexPositionBottom.Y = -(m_size / 2);
            vertexPositionBottom.Z = +(m_size / 2);
            quadVerticesBottom[1].Color = Color.Goldenrod;
            quadVerticesBottom[1].Position = vertexPositionBottom;
            // Bottom face bottom right vertex
            vertexPositionBottom.X = +(m_size / 2);
            vertexPositionBottom.Y = -(m_size / 2);
            vertexPositionBottom.Z = -(m_size / 2);
            quadVerticesBottom[2].Color = Color.Goldenrod;
            quadVerticesBottom[2].Position = vertexPositionBottom;
            // Bottom face top right vertex
            vertexPositionBottom.X = +(m_size / 2);
            vertexPositionBottom.Y = -(m_size / 2);
            vertexPositionBottom.Z = +(m_size / 2);
            quadVerticesBottom[3].Color = Color.Goldenrod;
            quadVerticesBottom[3].Position = vertexPositionBottom;
            Vector3 bottomFacePosition = new Vector3(0.0f, -(m_size / 2), 0.0f);
            m_bottomFace = new Quad(quadVerticesBottom, bottomFacePosition);
        }
        #endregion
    }
}
