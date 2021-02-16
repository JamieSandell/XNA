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

namespace Lesson_07
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
        private Matrix m_worldMatrix, m_viewMatrix, m_projectionMatrix;
        private Pyramid m_pyramid;
        private Cube m_cube;
        private Vector3 m_pyramidPosition, m_cubePosition;
        private Matrix m_translatePyramid, m_translateCube;
        private float m_pyramidRotationAngle, m_pyramidRotationSpeed;
        private float m_cubeRotationAngle, m_cubeRotationSpeed;
        Texture2D m_texture;
        //New code
        Effect m_effect;
        //
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

            //initialise rotation code
            m_pyramidRotationAngle = 0.0f;
            m_cubeRotationAngle = 0.0f;
            m_pyramidRotationSpeed = 0.1f;
            m_cubeRotationSpeed = 0.05f;
            //

            // The world matrix needs to be calculated so we can position our objects that we render
            // The view matrix is calculated each time the camera changes position and/or orientation
            // The projection matrix is normally calculated just once at the start of the game
            m_worldMatrix = Matrix.Identity;
            m_viewMatrix = Matrix.CreateLookAt(new Vector3(0.0f, 0.0f, 0.1f), Vector3.Zero, Vector3.Up);
            ResetProjection();

            // Graphics Properties
            graphics.PreferredBackBufferWidth = 1440;
            graphics.PreferredBackBufferHeight = 900;

            // Window code
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += OnClientSizeChanged;

            m_basicEffect.VertexColorEnabled = true; //So our shapes can be coloured

            RasterizerState rs = new RasterizerState();
            graphics.PreferMultiSampling = true;

            //Make sure the shapes that call Graphics.Draw have their GraphicsDeviceManager set
            Triangle.Graphics = graphics;
            Quad.Graphics = graphics;

            m_basicEffect.TextureEnabled = true;

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
            m_texture = Content.Load<Texture2D>("Textures\\Crate");

            //Create our pyramid
            m_pyramidPosition = new Vector3(0.0f, 0.0f, -7.0f);
            m_pyramid = new Pyramid(m_pyramidPosition, 1.0f, Color.Red, m_texture);
            //Create our Cube
            m_cubePosition = new Vector3(2.0f, 0.0f, -7.0f);
            m_cube = new Cube(m_cubePosition, 1.0f, Color.Blue, m_texture);

            //New code
            m_effect = Content.Load<Effect>("PerPixelSpotlight");
            m_effect.CurrentTechnique = m_effect.Techniques["PerPixelShading"];
            m_effect.Parameters["xWorld"].SetValue(m_worldMatrix);
            m_effect.Parameters["xView"].SetValue(m_viewMatrix);
            m_effect.Parameters["xProjection"].SetValue(m_projectionMatrix);
            m_effect.Parameters["xAmbient"].SetValue(0.0f);
            m_effect.Parameters["xLightPosition"].SetValue(new Vector3(0.0f, 0.0f, 0.0f));
            //
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

            m_pyramidRotationAngle += m_pyramidRotationSpeed *
                (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            m_cubeRotationAngle += m_cubeRotationSpeed *
                (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            // Create a translation matrix for our shapes based upon their desired position.
            m_translatePyramid = Matrix.CreateRotationY(MathHelper.ToRadians(m_pyramidRotationAngle))
               * Matrix.CreateTranslation(m_pyramid.Position);
            // You read matrix operations from right to left, so to make our square orbit around our
            // triangle, we have to rotate it to the position of the triangle, rotate it according to the
            // rotation angle of the square, and then translate it based upon the distance between our
            // two shapes
            Vector3 distanceVec = m_cube.Position - m_pyramidPosition;
            distanceVec.X = Math.Abs(distanceVec.X);
            distanceVec.Y = Math.Abs(distanceVec.Y);
            distanceVec.Z = Math.Abs(distanceVec.Z);
            float cubeRot = MathHelper.ToRadians(m_cubeRotationAngle);
            m_translateCube = Matrix.CreateTranslation(distanceVec)
                * Matrix.CreateFromYawPitchRoll(cubeRot, cubeRot, cubeRot)
                    * Matrix.CreateTranslation(m_pyramidPosition);

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
            // Then apply the current pass of our effect (replaces the deprecated begin/end of XNA 3.x)
            // Then draw the shape.
            m_basicEffect.TextureEnabled = true;
            // New code
            m_basicEffect.LightingEnabled = true;
            m_basicEffect.PreferPerPixelLighting = true;
            m_basicEffect.SpecularColor = Color.White.ToVector3();
            m_basicEffect.SpecularPower = 32.0f;
            //
            m_basicEffect.AmbientLightColor = new Vector3(0.1f, 0.1f, 0.1f);
            m_basicEffect.DiffuseColor = Color.White.ToVector3();
            m_basicEffect.DirectionalLight0.DiffuseColor = Color.White.ToVector3();
            m_basicEffect.DirectionalLight0.Direction = Vector3.Forward;
            m_basicEffect.DirectionalLight0.Enabled = true;
            for (int i = 0; i < m_basicEffect.CurrentTechnique.Passes.Count; i++)
            {
                //m_basicEffect.LightingEnabled = true;
                //m_basicEffect.Texture = m_pyramid.Texture;
                //m_basicEffect.World = m_translatePyramid;
                //m_basicEffect.CurrentTechnique.Passes[i].Apply();
                //m_pyramid.Draw();

                //m_basicEffect.DiffuseColor = Color.White.ToVector3();
                //m_basicEffect.LightingEnabled = false;
                //m_basicEffect.Texture = m_cube.Texture;
                //m_basicEffect.World = m_translateCube;
                //m_basicEffect.CurrentTechnique.Passes[i].Apply();
                //m_cube.Draw();
            }

            for (int i = 0; i < m_effect.CurrentTechnique.Passes.Count; i++)
            {
                m_effect.Parameters["xWorld"].SetValue(m_translatePyramid);
                m_effect.CurrentTechnique.Passes[i].Apply();
                m_pyramid.Draw();

                m_effect.Parameters["xWorld"].SetValue(m_translateCube);
                m_effect.CurrentTechnique.Passes[i].Apply();
                m_cube.Draw();
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