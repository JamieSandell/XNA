using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lesson_07
{
    // New code
    public class Light
    {
        #region Constructors
        public Light(ref Vector3 position, float size, GraphicsDeviceManager graphicsDeviceManager)
        {
            Graphics = graphicsDeviceManager;
            Initialise(ref position, size);
        }
        public Light(ref Vector3 position, float size)
        {
            Initialise(ref position, size);
        }
        #endregion

        #region Accessors
        public float X
        {
            get
            {
                return m_cube.X;
            }
            set
            {
                m_cube.X = value;
            }
        }
        public float Y
        {
            get
            {
                return m_cube.Y;
            }
            set
            {
                m_cube.Y = value;
            }
        }
        public float Z
        {
            get
            {
                return m_cube.Z;
            }
            set
            {
                m_cube.Z = value;
            }
        }
        public Cube Cube
        {
            get
            {
                return m_cube;
            }
        }
        public Vector3 Position
        {
            get
            {
                return m_cube.Position;
            }
            set
            {
                m_cube.Position = value;
            }
        }
        static public GraphicsDeviceManager Graphics
        {
            set
            {
                Cube.Graphics = value;
            }
        }
        #endregion

        #region Member Variables
        private Cube m_cube; // Physical representation of the light
        #endregion

        #region Member Methods
        /// <summary>
        /// Draws the light
        /// </summary>
        public void Draw()
        {
            m_cube.Draw();
        }

        /// <summary>
        /// Initialise the light
        /// </summary>
        /// <param name="position">The position of the light</param>
        /// <param name="size">The size of the physical representation of the light</param>
        private void Initialise(ref Vector3 position, float size)
        {
            m_cube = new Cube(position, size, Color.White, null);
        }

        /// <summary>
        /// Processes the passed in command
        /// </summary>
        /// <param name="command">The list of commands to process</param>
        /// <param name="deltaTime"></param>
        private void ProcessCommand(List<Commands> command, float deltaTime)
        {
            foreach (Commands c in command)
            {
                switch (c)
                {
                    case Commands.MoveLeft:
                        m_cube.X -= deltaTime;
                        break;
                    case Commands.MoveRight:
                        m_cube.X += deltaTime;
                        break;
                    case Commands.MoveUp:
                        m_cube.Y += deltaTime;
                        break;
                    case Commands.MoveDown:
                        m_cube.Y -= deltaTime;
                        break;
                    case Commands.MoveForward:
                        m_cube.Z -= deltaTime;
                        break;
                    case Commands.MoveBackward:
                        m_cube.Z += deltaTime;
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Updates the light's position if necessary.
        /// </summary>
        /// <param name="deltaTime">For cpu independent animation.</param>
        /// <param name="command">The list of commands to be passed off to the ProcessCommand method</param>
        public void Update(float deltaTime, List<Commands> command)
        {
            ProcessCommand(command, deltaTime);
        }
        #endregion
    }
    //
}
