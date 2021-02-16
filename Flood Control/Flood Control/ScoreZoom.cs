using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Flood_Control
{
    class ScoreZoom
    {
        public string Text;
        public Color DrawColour;

        private int displayCounter;
        private int maxDisplayCount = 30;
        private float scale = 0.4f; //Actual size of the text when drawing it
        private float lastScaleAmount = 0.0f; //The amount the text was scaled by in the previous frame
        private float scaleAmount = 0.4f; //The growth in scale between each frame

        public ScoreZoom(string displayText, Color fontColor)
        {
            Text = displayText;
            DrawColour = fontColor;
        }

        public float Scale
        {
            get { return scaleAmount * displayCounter; }
        }

        public bool IsCompleted()
        {
            return (displayCounter > maxDisplayCount);
        }

        /// <summary>
        /// Scales the text in an exponential fashion
        /// </summary>
        public void Update()
        {
            scale += lastScaleAmount + scaleAmount;
            lastScaleAmount += scaleAmount;
            displayCounter++;
        }
    }
}
