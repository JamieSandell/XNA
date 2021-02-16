using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Lesson_03
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Member Variables
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private KeyboardState m_currKeyboardState, m_prevKeyboardState;
        private List<Keys> m_keyAltEnter; //FullScreen key
        private bool m_IsFullScreen;
        private BasicEffect m_basicEffect;
        private VertexPositionColor[] m_triangleVertices;
        private Matrix m_worldMatrix, m_viewMatrix, m_projectionMatrix, m_translateTriangle,
            m_translateQuad;
        private Vector3 m_trianglePosition, m_quadPosition;
        private Quad m_quad;
        //New variables this lesson
        private Color m_quadColour;
        #endregion
        #region Constructors
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        #endregion
        #region Methods
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            m_currKeyboardState = new KeyboardState();
            m_prevKeyboardState = new KeyboardState();
            m_keyAltEnter = new List<Keys>() { Keys.LeftAlt, Keys.Enter };
            m_IsFullScreen = false;
            IsMouseVisible = true;

            // basicEffect
            m_basicEffect = new BasicEffect(graphics.GraphicsDevice);

            // The world matrix needs to be calculated so we can position our objects that we render
            // The view matrix is calculated each time the camera changes position and/or orientation
            // The projection matrix is normally calculated just once at the start of the game
            m_worldMatrix = Matrix.Identity;
            m_viewMatrix = Matrix.CreateLookAt(new Vector3(0.0f, 0.0f, 0.1f), Vector3.Zero, Vector3.Up);
            ResetProjection();

            // Graphics Properties
            graphics.PreferredBackBufferWidth = 640;
            graphics.PreferredBackBufferHeight = 480;

            //Create our triangle
            m_trianglePosition = new Vector3(0.0f, 0.0f, -5.0f);
            m_triangleVertices = new VertexPositionColor[3];
            // Specify the vertices in a clockwise winding order so culling is applied properly
            m_triangleVertices[0].Position = new Vector3(-0.5f, -0.5f, 0.0f); //bottom left
            m_triangleVertices[1].Position = new Vector3(0.0f, 0.0f, 0.0f); //top middle
            m_triangleVertices[2].Position = new Vector3(0.5f, -0.5f, 0.0f); // bottom right

            // Window code
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += OnClientSizeChanged;

            // New init code for this lesson
            m_quadColour = new Color(0.0f, 0.0f, 1.0f, 1.0f);
            m_triangleVertices[0].Color = Color.Green;
            m_triangleVertices[1].Color = Color.Red;
            m_triangleVertices[2].Color = Color.Blue;

            m_basicEffect.VertexColorEnabled = true;
            //

            //Create our Quad
            m_quadPosition = new Vector3(-2.0f, 0.0f, -5.0f);
            m_quad = new Quad(1.0f, 1.0f, ref m_quadPosition, ref m_quadColour);
            Quad.Graphics = graphics;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            m_currKeyboardState = Keyboard.GetState();
            ProcessInput();
            m_prevKeyboardState = m_currKeyboardState;

            // Create a translation matrix for our shapes based upon their desired position.
            m_translateTriangle = Matrix.CreateTranslation(m_trianglePosition);
            m_translateQuad = Matrix.CreateTranslation(m_quad.Position);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer,
                Color.CornflowerBlue, 1.0f, 0);

            // TODO: Add your drawing code here
            // Cycle through all of passes of our effect
            // Set the World matrix of our effect to our shape translation matrix
            // (so our shape is translated to the correct position on screen)
            // Then apply the current pass of our effect (replaces the deprecated begin/end of XNA 3.x
            // Then draw the shape.

            for (int i = 0; i < m_basicEffect.CurrentTechnique.Passes.Count; i++)
            {
                m_basicEffect.World = m_translateTriangle;
                m_basicEffect.CurrentTechnique.Passes[i].Apply();
                graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList,
                    m_triangleVertices, 0, 1);

                m_basicEffect.World = m_translateQuad;
                m_basicEffect.CurrentTechnique.Passes[i].Apply();
                m_quad.Draw();
            }
            base.Draw(gameTime);
        }

        protected void OnClientSizeChanged(object sender, EventArgs e)
        {
            ResetProjection();
        }

        /// <summary>
        /// Resets the current projection matrix so that the aspect ratio is kept the same.
        /// </summary>
        private void ResetProjection()
        {
            Viewport viewport = graphics.GraphicsDevice.Viewport;
            float aspect = (float)viewport.Width / (float)viewport.Height;
            // Could store this straight into our basicEffect Projection Matrix
            // However if we store it in a separate variable we could set different projections
            // into different effects.
            m_projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspect, 0.1f, 100.0f);
            m_basicEffect.Projection = m_projectionMatrix;
        }

        /// <summary>
        /// Process inputs such as the mouse, keyboard, gamepad, etc.
        /// </summary>
        private void ProcessInput()
        {
            //////////////////////////////////////////////////////////////////////////
            //GamePad Code
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            //////////////////////////////////////////////////////////////////////////
            //Keyboard code
            // Toggles fullscreen mode
            else if ((Utility.HasKeyBeenPressed(m_keyAltEnter, ref m_prevKeyboardState, ref m_currKeyboardState)))
            {
                m_IsFullScreen = !m_IsFullScreen;
                graphics.ToggleFullScreen();
                if (m_IsFullScreen)
                    IsMouseVisible = false;
                else
                    IsMouseVisible = true;
            }
        }
        #endregion
    }
}
