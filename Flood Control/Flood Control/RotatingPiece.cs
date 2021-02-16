using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Flood_Control
{
    /// <summary>
    /// A rotated piece will rotate 90 degress clockwise over 10 animations frames.
    /// Will rotate anti-clockwise is clockwise is set to false when the RotatingPiece is constructed.
    /// </summary>
    class RotatingPiece : GamePiece
    {
        public bool clockwise;

        public static float rotationRate = (MathHelper.PiOver2 / 10);
        private float rotationAmount = 0;
        public int rotationTicksRemaining = 10;

        public RotatingPiece(string pieceType, bool clockwise)
            : base(pieceType)
        {
            this.clockwise = clockwise;
        }

        public float RotationAmount
        {
            get
            {
                if (clockwise)
                    return rotationAmount;
                else
                    return (MathHelper.Pi * 2) - rotationAmount;
            }
        }

        public void UpdatePiece()
        {
            rotationAmount += rotationRate;
            rotationTicksRemaining = (int)MathHelper.Max(0, rotationTicksRemaining - 1); //Decrenenbt rotationTicksRemaining by 1 but make sure it doesn't fall below 0
        }
    }
}
