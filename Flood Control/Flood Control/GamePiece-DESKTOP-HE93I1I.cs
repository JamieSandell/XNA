using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Flood_Control
{
    class GamePiece
    {
        public static string[] PieceTypes =
        {
            "Left,Right",
            "Top,Bottom",
            "Left,Top",
            "Top,Right",
            "Right,Bottom",
            "Bottom,Left",
            "Empty"
        };

        public const int PieceHeight = 40;
        public const int PieceWidth = 40;

        public const int MaxPlayablePieceIndex = 5;
        public const int EmptyPieceIndex = 6;

        private const int textureOffsetX = 1;
        private const int textureOffsetY = 1;
        private const int texturePaddingX = 1;
        private const int texturePaddingY = 1;

        private string pieceType = "";
        private string pieceSuffix = "";

        public string PieceType
        {
            get { return pieceType; }
        }

        public string PieceSuffix
        {
            get { return pieceSuffix; }
        }

        public GamePiece(string type, string suffix)
        {
            pieceType = type;
            pieceSuffix = suffix;
        }

        public GamePiece(string type) // If no suffix is specified an empty suffix is assumed
        {
            pieceType = type;
            pieceSuffix = "";
        }

        public void SetPiece(string type, string suffix)
        {
            pieceType = type;
            pieceSuffix = suffix;
        }

        public void SetPiece(string type) // If no suffix is specified an empty suffix is assumed
        {
            SetPiece(type, "");
        }

        public void AddSuffix(string suffix)
        {
            if (!pieceSuffix.Contains(suffix))
                pieceSuffix += suffix;
        }

        public void RemoveSuffix(string suffix)
        {
            pieceSuffix = pieceSuffix.Replace(suffix, "");
        }

        public void RotatePiece(bool clockwise)
        {
            switch (pieceType)
            {
                case "Left,Right":  // Horizontal piece
                    pieceType = "Top,Bottom"; // Vertical piece
                    break;
                case "Top,Bottom": // Vertical piece
                    pieceType = "Left,Right"; // Horizontal piece
                    break;
                case "Left,Top": // __| piece
                    if (clockwise)
                        pieceType = "Top,Right"; // |__ piece
                    else
                        pieceType = "Bottom,Left"; // --| piece
                    break;
                case "Top,Right":
                    if (clockwise)
                        pieceType = "Right,Bottom";
                    else
                        pieceType = "Left,Top";
                    break;
                case "Right,Bottom":
                    if (clockwise)
                        pieceType = "Bottom,Left";
                    else
                        pieceType = "Top,Right";
                    break;
                case "Bottom,Left":
                    if (clockwise)
                        pieceType = "Left,Top";
                    else
                        pieceType = "Right,Bottom";
                    break;
                case "Empty":
                    break;
            }
        }

        /// <summary>
        /// Creates an empty List object for holding the ends we want to
        /// return to the calling code. It then uses the Split() method of the string class to get each
        /// end listed in the pieceType. For example, the Top,Bottom piece will return an array with
        /// two elements. The first element will contain Top, the second element will contain Bottom. The
        /// comma delimiter will not be returned with either string.
        /// </summary>
        /// <param name="startingEnd"></param>
        /// <returns></returns>
        public  string[] GetOtherEnds(string startingEnd)
        {
            List<string> opposites = new List<string>();

            foreach (string end in pieceType.Split(','))
            {
                if (end != startingEnd)
                    opposites.Add(end);
            }
            return opposites.ToArray();
        }

        public bool HasConnector(string direction)
        {
            return pieceType.Contains(direction);
        }

        public Rectangle GetSourceRect()
        {
            int x = textureOffsetX;
            int y = textureOffsetY;

            if (pieceSuffix.Contains("W"))
                x += PieceWidth + texturePaddingX;

            y += (Array.IndexOf(PieceTypes, pieceType) * (PieceHeight + texturePaddingY));

            return new Rectangle(x, y, PieceWidth, PieceHeight);
        }
    }
}
