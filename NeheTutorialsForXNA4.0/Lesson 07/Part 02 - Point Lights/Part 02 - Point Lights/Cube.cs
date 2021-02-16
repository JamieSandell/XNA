using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lesson_07
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
        public Texture2D Texture { get; set; }

        private Quad m_frontFace, m_backFace, m_topFace, m_bottomFace, m_leftFace, m_rightFace;
        private bool m_created; //Used to determine whether or not the object has been created.
        private float m_size;
        #endregion

        #region Constructors
        public Cube(Vector3 position, float m_size, Color color, Texture2D texture)
        {
            Initialise(ref position, m_size, ref color, texture);
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

        private void Initialise(ref Vector3 position, float m_size, ref Color color, Texture2D texture)
        {
            m_created = false;
            Position = position;
            Size = m_size;
            Color = color;
            Texture = texture;

            InitialiseVertices();
            m_created = true;
        }

        private void InitialiseVertices()
        {
            float m_size = Size;

            Vector2 bottomLeft = new Vector2(0, 1);
            Vector2 topLeft = new Vector2(0, 0);
            Vector2 topRight = new Vector2(1, 0);
            Vector2 bottomRight = new Vector2(1, 1);

            VertexPositionNormalTextureColor[] quadVerticesFront = new VertexPositionNormalTextureColor[4];
            Vector3 vertexPositionFront = new Vector3();
            // Front face bottom left vertex
            vertexPositionFront.X = -(m_size / 2);
            vertexPositionFront.Y = -(m_size / 2);
            vertexPositionFront.Z = +(m_size / 2);
            quadVerticesFront[0].Color = Color.White;
            quadVerticesFront[0].Position = vertexPositionFront;
            quadVerticesFront[0].TextureCoordinate = bottomLeft;
            // Front face top left vertex
            vertexPositionFront.X = -(m_size / 2);
            vertexPositionFront.Y = +(m_size / 2);
            vertexPositionFront.Z = +(m_size / 2);
            quadVerticesFront[1].Color = Color.White;
            quadVerticesFront[1].Position = vertexPositionFront;
            quadVerticesFront[1].TextureCoordinate = topLeft;
            // Front face bottom right vertex
            vertexPositionFront.X = +(m_size / 2);
            vertexPositionFront.Y = -(m_size / 2);
            vertexPositionFront.Z = +(m_size / 2);
            quadVerticesFront[2].Color = Color.White;
            quadVerticesFront[2].Position = vertexPositionFront;
            quadVerticesFront[2].TextureCoordinate = bottomRight;
            // Front face top right vertex
            vertexPositionFront.X = +(m_size / 2);
            vertexPositionFront.Y = +(m_size / 2);
            vertexPositionFront.Z = +(m_size / 2);
            quadVerticesFront[3].Color = Color.White;
            quadVerticesFront[3].Position = vertexPositionFront;
            quadVerticesFront[3].TextureCoordinate = topRight;
            // New code
            for (int i = 0; i < quadVerticesFront.Length; i++)
            {
                quadVerticesFront[i].Normal = Vector3.Backward;
            }
            //
            Vector3 frontFacePosition = new Vector3(0.0f, 0.0f, +(m_size / 2)); // The centre of the face
            m_frontFace = new Quad(quadVerticesFront, frontFacePosition, Texture);

            VertexPositionNormalTextureColor[] quadVerticesLeft = new VertexPositionNormalTextureColor[4];
            Vector3 vertexPositionLeft = new Vector3();
            // Left face bottom left vertex
            vertexPositionLeft.X = -(m_size / 2);
            vertexPositionLeft.Y = -(m_size / 2);
            vertexPositionLeft.Z = -(m_size / 2);
            quadVerticesLeft[0].Color = Color.White;
            quadVerticesLeft[0].Position = vertexPositionLeft;
            quadVerticesLeft[0].TextureCoordinate = bottomLeft;
            // Left face top left vertex
            vertexPositionLeft.X = -(m_size / 2);
            vertexPositionLeft.Y = +(m_size / 2);
            vertexPositionLeft.Z = -(m_size / 2);
            quadVerticesLeft[1].Color = Color.White;
            quadVerticesLeft[1].Position = vertexPositionLeft;
            quadVerticesLeft[1].TextureCoordinate = topLeft;
            // Left face bottom right vertex
            vertexPositionLeft.X = -(m_size / 2);
            vertexPositionLeft.Y = -(m_size / 2);
            vertexPositionLeft.Z = +(m_size / 2);
            quadVerticesLeft[2].Color = Color.White;
            quadVerticesLeft[2].Position = vertexPositionLeft;
            quadVerticesLeft[2].TextureCoordinate = bottomRight;
            // Left face top right vertex
            vertexPositionLeft.X = -(m_size / 2);
            vertexPositionLeft.Y = +(m_size / 2);
            vertexPositionLeft.Z = +(m_size / 2);
            quadVerticesLeft[3].Color = Color.White;
            quadVerticesLeft[3].Position = vertexPositionLeft;
            quadVerticesLeft[3].TextureCoordinate = topRight;
            // New code
            for (int i = 0; i < quadVerticesLeft.Length; i++)
            {
                quadVerticesLeft[i].Normal = Vector3.Left;
            }
            //
            Vector3 leftFacePosition = new Vector3(-(m_size / 2), 0.0f, 0.0f);
            m_leftFace = new Quad(quadVerticesLeft, leftFacePosition, Texture);

            VertexPositionNormalTextureColor[] quadVerticesBack = new VertexPositionNormalTextureColor[4];
            Vector3 vertexPositionBack = new Vector3();
            // Back face bottom left vertex
            vertexPositionBack.X = +(m_size / 2);
            vertexPositionBack.Y = -(m_size / 2);
            vertexPositionBack.Z = -(m_size / 2);
            quadVerticesBack[0].Color = Color.White;
            quadVerticesBack[0].Position = vertexPositionBack;
            quadVerticesBack[0].TextureCoordinate = bottomLeft;
            // Back face top left vertex
            vertexPositionBack.X = +(m_size / 2);
            vertexPositionBack.Y = +(m_size / 2);
            vertexPositionBack.Z = -(m_size / 2);
            quadVerticesBack[1].Color = Color.White;
            quadVerticesBack[1].Position = vertexPositionBack;
            quadVerticesBack[1].TextureCoordinate = topLeft;
            // Back face bottom right vertex
            vertexPositionBack.X = -(m_size / 2);
            vertexPositionBack.Y = -(m_size / 2);
            vertexPositionBack.Z = -(m_size / 2);
            quadVerticesBack[2].Color = Color.White;
            quadVerticesBack[2].Position = vertexPositionBack;
            quadVerticesBack[2].TextureCoordinate = bottomRight;
            // Back face top right vertex
            vertexPositionBack.X = -(m_size / 2);
            vertexPositionBack.Y = +(m_size / 2);
            vertexPositionBack.Z = -(m_size / 2);
            quadVerticesBack[3].Color = Color.White;
            quadVerticesBack[3].Position = vertexPositionBack;
            quadVerticesBack[3].TextureCoordinate = topRight;
            // New code
            for (int i = 0; i < quadVerticesBack.Length; i++)
            {
                quadVerticesBack[i].Normal = Vector3.Forward;
            }
            //
            Vector3 backFacePosition = new Vector3(0.0f, 0.0f, -(m_size / 2));
            m_backFace = new Quad(quadVerticesBack, backFacePosition, Texture);

            VertexPositionNormalTextureColor[] quadVerticesRight = new VertexPositionNormalTextureColor[4];
            Vector3 vertexPositionRight = new Vector3();
            // Right face bottom left vertex
            vertexPositionRight.X = +(m_size / 2);
            vertexPositionRight.Y = -(m_size / 2);
            vertexPositionRight.Z = +(m_size / 2);
            quadVerticesRight[0].Color = Color.White;
            quadVerticesRight[0].Position = vertexPositionRight;
            quadVerticesRight[0].TextureCoordinate = bottomLeft;
            // Right face top left vertex
            vertexPositionRight.X = +(m_size / 2);
            vertexPositionRight.Y = +(m_size / 2);
            vertexPositionRight.Z = +(m_size / 2);
            quadVerticesRight[1].Color = Color.White;
            quadVerticesRight[1].Position = vertexPositionRight;
            quadVerticesRight[1].TextureCoordinate = topLeft;
            // Right face bottom right vertex
            vertexPositionRight.X = +(m_size / 2);
            vertexPositionRight.Y = -(m_size / 2);
            vertexPositionRight.Z = -(m_size / 2);
            quadVerticesRight[2].Color = Color.White;
            quadVerticesRight[2].Position = vertexPositionRight;
            quadVerticesRight[2].TextureCoordinate = bottomRight;
            // Right face top right vertex
            vertexPositionRight.X = +(m_size / 2);
            vertexPositionRight.Y = +(m_size / 2);
            vertexPositionRight.Z = -(m_size / 2);
            quadVerticesRight[3].Color = Color.White;
            quadVerticesRight[3].Position = vertexPositionRight;
            quadVerticesRight[3].TextureCoordinate = topRight;
            // New code
            for (int i = 0; i < quadVerticesRight.Length; i++)
            {
                quadVerticesRight[i].Normal = Vector3.Right;
            }
            //
            Vector3 rightFacePosition = new Vector3(+(m_size / 2), 0.0f, 0.0f);
            m_rightFace = new Quad(quadVerticesRight, rightFacePosition, Texture);

            VertexPositionNormalTextureColor[] quadVerticesTop = new VertexPositionNormalTextureColor[4];
            Vector3 vertexPositionTop = new Vector3();
            // Top face bottom left vertex
            vertexPositionTop.X = -(m_size / 2);
            vertexPositionTop.Y = +(m_size / 2);
            vertexPositionTop.Z = +(m_size / 2);
            quadVerticesTop[0].Color = Color.White;
            quadVerticesTop[0].Position = vertexPositionTop;
            quadVerticesTop[0].TextureCoordinate = bottomLeft;
            // Top face top left vertex
            vertexPositionTop.X = -(m_size / 2);
            vertexPositionTop.Y = +(m_size / 2);
            vertexPositionTop.Z = -(m_size / 2);
            quadVerticesTop[1].Color = Color.White;
            quadVerticesTop[1].Position = vertexPositionTop;
            quadVerticesTop[1].TextureCoordinate = topLeft;
            // Top face bottom right vertex
            vertexPositionTop.X = +(m_size / 2);
            vertexPositionTop.Y = +(m_size / 2);
            vertexPositionTop.Z = +(m_size / 2);
            quadVerticesTop[2].Color = Color.White;
            quadVerticesTop[2].Position = vertexPositionTop;
            quadVerticesTop[2].TextureCoordinate = bottomRight;
            // Top face top right vertex
            vertexPositionTop.X = +(m_size / 2);
            vertexPositionTop.Y = +(m_size / 2);
            vertexPositionTop.Z = -(m_size / 2);
            quadVerticesTop[3].Color = Color.White;
            quadVerticesTop[3].Position = vertexPositionTop;
            quadVerticesTop[3].TextureCoordinate = topRight;
            // New code
            for (int i = 0; i < quadVerticesTop.Length; i++)
            {
                quadVerticesTop[i].Normal = Vector3.Up;
            }
            //
            Vector3 topFacePosition = new Vector3(0.0f, +(m_size / 2), 0.0f);
            m_topFace = new Quad(quadVerticesTop, topFacePosition, Texture);

            VertexPositionNormalTextureColor[] quadVerticesBottom = new VertexPositionNormalTextureColor[4];
            Vector3 vertexPositionBottom = new Vector3();
            // Bottom face bottom left vertex
            vertexPositionBottom.X = -(m_size / 2);
            vertexPositionBottom.Y = -(m_size / 2);
            vertexPositionBottom.Z = -(m_size / 2);
            quadVerticesBottom[0].Color = Color.White;
            quadVerticesBottom[0].Position = vertexPositionBottom;
            quadVerticesBottom[0].TextureCoordinate = bottomLeft;
            quadVerticesBottom[0].TextureCoordinate = bottomLeft;
            // Bottom face top left vertex
            vertexPositionBottom.X = -(m_size / 2);
            vertexPositionBottom.Y = -(m_size / 2);
            vertexPositionBottom.Z = +(m_size / 2);
            quadVerticesBottom[1].Color = Color.White;
            quadVerticesBottom[1].Position = vertexPositionBottom;
            quadVerticesBottom[1].TextureCoordinate = topLeft;
            quadVerticesBottom[1].TextureCoordinate = topLeft;
            // Bottom face bottom right vertex
            vertexPositionBottom.X = +(m_size / 2);
            vertexPositionBottom.Y = -(m_size / 2);
            vertexPositionBottom.Z = -(m_size / 2);
            quadVerticesBottom[2].Color = Color.White;
            quadVerticesBottom[2].Position = vertexPositionBottom;
            quadVerticesBottom[2].TextureCoordinate = bottomRight;
            // Bottom face top right vertex
            vertexPositionBottom.X = +(m_size / 2);
            vertexPositionBottom.Y = -(m_size / 2);
            vertexPositionBottom.Z = +(m_size / 2);
            quadVerticesBottom[3].Color = Color.White;
            quadVerticesBottom[3].Position = vertexPositionBottom;
            quadVerticesBottom[3].TextureCoordinate = topRight;
            // New code
            for (int i = 0; i < quadVerticesBottom.Length; i++)
            {
                quadVerticesBottom[i].Normal = Vector3.Down;
            }
            //
            Vector3 bottomFacePosition = new Vector3(0.0f, -(m_size / 2), 0.0f);
            m_bottomFace = new Quad(quadVerticesBottom, bottomFacePosition, Texture);
        }
        #endregion
    }
}
