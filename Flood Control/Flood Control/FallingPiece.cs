using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Flood_Control
{
    /// <summary>
    /// A FallingPiece has a vertical offset that shows how high it is from its final destination it is located.
    /// </summary>
    class FallingPiece : GamePiece
    {
        public int verticalOffset;
        public static int fallRate = 5;

        public FallingPiece(string pieceType, int verticalOffset)
            : base(pieceType)
        {
            this.verticalOffset = verticalOffset;
        }

        public void UpdatePiece()
        {
            verticalOffset = (int)MathHelper.Max(0, verticalOffset - fallRate); //Decrease the position by the fall rate but ensure it doesn't fall below 0
        }
    }
}
