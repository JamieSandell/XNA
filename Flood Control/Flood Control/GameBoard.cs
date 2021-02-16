using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Flood_Control
{
    class GameBoard
    {
        Random rand = new Random();

        public const int GameBoardWidth = 8;
        public const int GameBoardHeight = 10;

        private GamePiece[,] boardSquares = new GamePiece[GameBoardWidth, GameBoardHeight];

        private List<Vector2> waterTracker = new List<Vector2>();

        private int lockedPiecePercentChance = 10;

        public Dictionary<string, FallingPiece> fallingPieces = new Dictionary<string, FallingPiece>();
        public Dictionary<string, RotatingPiece> rotatingPieces = new Dictionary<string, RotatingPiece>();
        public Dictionary<string, FadingPiece> fadingPieces = new Dictionary<string, FadingPiece>();

        public int LockedPiecePercentChance
        {
            get { return lockedPiecePercentChance; }
            set { lockedPiecePercentChance = LockedPiecePercentChance; }
        }

        public GamePiece[,] BoardSquares
        {
            get { return boardSquares; }
            private set { }
        }
 
        public GameBoard()
        {
            ClearBoard();
        }

        public void AddFallingPiece(int x, int y, string pieceName, int verticalOffset)
        {
            fallingPieces[x.ToString() + "_" + y.ToString()] = new FallingPiece(pieceName, verticalOffset);
            fallingPieces[x.ToString() + "_" + y.ToString()].AddSuffix(boardSquares[x, y].PieceSuffix); //Need to add the suffix because our constructor doesn't take one
            // meaning without this code a falling locked piece for example would become a falling non-locked piece
        }

        public void AddRotatingPiece(int x, int y, string pieceName, bool clockwise)
        {
            rotatingPieces[x.ToString() + "_" + y.ToString()] = new RotatingPiece(pieceName, clockwise);
        }

        public void AddFadingPiece(int x, int y, string pieceName)
        {
            fadingPieces[x.ToString() + "_" + y.ToString()] = new FadingPiece(pieceName, boardSquares[x, y].PieceSuffix); //FadingPiece is a scoring piece so it will always be a filled water piece, hence the "W" suffix
        }

        public bool ArePiecesAnimating()
        {
            if ((fallingPieces.Count == 0)
                && (rotatingPieces.Count == 0)
                && (fadingPieces.Count == 0))
                return false;
            else
                return true;
        }

        public void ClearBoard()
        {
            for (int x = 0; x < GameBoardWidth; x++)
                for (int y = 0; y < GameBoardHeight; y++)
                    boardSquares[x, y] = new GamePiece("Empty");
        }

        public void RotatePiece(int x, int y, bool clockwise)
        {
            boardSquares[x, y].RotatePiece(clockwise);
        }

        public Rectangle GetSourecRect(int x, int y)
        {
            return boardSquares[x, y].GetSourceRect();
        }

        public string GetSquare(int x, int y)
        {
            return boardSquares[x, y].PieceType;
        }

        public void SetSquare(int x, int y, string pieceName)
        {
            boardSquares[x, y].SetPiece(pieceName);
        }

        public bool HasConnector(int x, int y, string direction)
        {
            return boardSquares[x, y].HasConnector(direction);
        }

        public void RandomPiece(int x, int y)
        {
            int result = rand.Next(1, 101);
            if (result <= lockedPiecePercentChance)
                boardSquares[x, y].SetPiece(GamePiece.PieceTypes[rand.Next(0, GamePiece.MaxPlayablePieceIndex + 1)], "L");
            else
                boardSquares[x, y].SetPiece(GamePiece.PieceTypes[rand.Next(0, GamePiece.MaxPlayablePieceIndex + 1)]);
        }

        public void FillFromAbove(int x, int y)
        {
            int rowLookup = y - 1; // Look at the piece above the scoring piece

            while (rowLookup >= 0) // Make sure we haven't gone above the gameboard, gameboard top left co-ords are 0,0
            {
                if (GetSquare(x, rowLookup) != "Empty") // Don't want to drop down any Empty pieces
                {
                    SetSquare(x, y, GetSquare(x, rowLookup)); //Set the scoring square/piece to the one above it - i.e. make it fall down
                    SetSquare(x, rowLookup, "Empty"); //Set that board square to the Empty piece

                    AddFallingPiece(x, y, GetSquare(x, y), GamePiece.PieceHeight * (y - rowLookup)); //Create the piece at it's destination, but provide the vertical offset as that is where
                    // it will be first drawn - i.e. so it can "fall down" the game board.

                    rowLookup = -1; //Set the exit flag
                }
                rowLookup--;
            }
        }

        /// <summary>
        /// When GenerateNewPieces is called with "true" passed as dropSquares, the looping
        /// logic processes one column at a time from the bottom up. When it finds an empty square it   
        /// calls FillFromAbove() to pull a filled square from above it into that location.
        /// 
        /// The reason the processing order is important here is that, by filling a lower square from
        /// a higher position, that higher position will become empty. It, in turn, will need to be filled
        /// from above.
        /// 
        /// After the holes are filled (or if dropSquares is set to false) GenerateNewPieces()
        /// examines each square in boardSquares and asks it to generate random pieces for each
        /// square that contains an empty square.
        /// </summary>
        /// <param name="dropSquares">Set to true to drop squares from above after a scoring chain</param>
        public void GenerateNewPieces(bool dropSquares)
        {
            if (dropSquares)
            {
                for (int x = 0; x < GameBoard.GameBoardWidth; x++)
                {
                    for (int y = GameBoard.GameBoardHeight - 1; y >= 0; y--)
                    {
                        if (GetSquare(x, y) == "Empty")
                            FillFromAbove(x, y);
                    }
                }
            }

            for (int y = 0; y < GameBoardHeight; y++)
                for (int x = 0; x < GameBoardWidth; x++)
                {
                    if (GetSquare(x, y) == "Empty")
                    {
                        RandomPiece(x, y);
                        AddFallingPiece(x, y, GetSquare(x, y), GamePiece.PieceHeight * GameBoardHeight); //always start above the playing area and then fall into it
                    }                        
                }
        }

        /// <summary>
        /// Instead of filling and emptying individual pipes it is easier to empty all of the pipes
        /// and then refill the pipes that need to be marked as having water in them.
        /// </summary>
        public void ResetWater()
        {
            for (int y = 0; y < GameBoardHeight; y++)
                for (int x = 0; x < GameBoardWidth; x++)
                    boardSquares[x, y].RemoveSuffix("W");
        }

        /// <summary>
        /// Marks a piece as having water in it.
        /// </summary>
        /// <param name="x">The x co-ords of the piece</param>
        /// <param name="y">The y co-ords of the piece</param>
        public void FillPiece(int x, int y)
        {
            boardSquares[x, y].AddSuffix("W");
        }

        /// <summary>
        /// Logic to determine which pipes should be filled depending on their orientation.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="fromDirection"></param>
        public void PropagateWater(int x, int y, string fromDirection)
        {
            if ((y >= 0) && (y < GameBoardHeight) && (x >= 0) && (x < GameBoardWidth)) //Within the bounds of the actual game board.
            {
                //If the piece has contains that type of direction and the piece is not already filled with water
                if (boardSquares[x,y].HasConnector(fromDirection) && !boardSquares[x,y].PieceSuffix.Contains("W"))
                {
                    FillPiece(x, y); //fill the piece with water
                    waterTracker.Add(new Vector2(x, y)); //add the co-ords of the filled piece to the water tracker
                    foreach (string end in boardSquares[x,y].GetOtherEnds(fromDirection)) //Recursivley call PropagateWater to fill up all other connected pieces with water
                    {
                        switch (end)
                        {
                            case "Left": //Piece contains a left connector so we need to propagate to the left from the right.
                                PropagateWater(x - 1, y, "Right");
                                break;
                            case "Right": //Piece contains a right connector so we need to propagate to the right from the left.
                                PropagateWater(x + 1, y, "Left");
                                break;
                            case "Top": //Piece contains a top connector so we need to propagate to the top from the bottom.
                                PropagateWater(x, y - 1, "Bottom");
                                break;
                            case "Bottom": //Piece contains a bototm connector we need to propagate from the bottom to the top.
                                PropagateWater(x, y + 1, "Top");
                                break;
                        }
                    }
                }
            }
            
        }

        /// <summary>
        /// Calls PropagateWater once for each row
        /// </summary>
        /// <param name="y">The position of the row.</param>
        /// <returns>A list of co-ords representing the game pieces that have water in them</returns>
        public List<Vector2> GetWaterChain(int y)
        {
            waterTracker.Clear();
            PropagateWater(0, y, "Left");
            return waterTracker;
        }

        public void UpdateAnimatedPieces()
        {
            // We want all of the fading pieces to finish animating first.
            // Otherwise it would look weird having new tiles pass through the old fading ones
            if (fadingPieces.Count == 0)
            {
                UpdateFallingPieces();
                UpdateRotatingPieces();
            }
            else
                UpdateFadingPieces();
        }

        private void UpdateFadingPieces()
        {
            Queue<string> RemoveKeys = new Queue<string>(); //Need a queue as you can't modify a dictionary object whilst it is being processed in a foreach loop

            foreach (string thisKey in fadingPieces.Keys)
            {
                fadingPieces[thisKey].UpdatePiece();

                if (fadingPieces[thisKey].alphaLevel == 0.0f)
                    RemoveKeys.Enqueue(thisKey.ToString());
            }

            while (RemoveKeys.Count > 0)
                fadingPieces.Remove(RemoveKeys.Dequeue());
        }

        private void UpdateFallingPieces()
        {
            Queue<string> RemoveKeys = new Queue<string>(); //Need a queue as you can't modify a dictionary object whilst it is being processed in a foreach loop

            foreach (string thisKey in fallingPieces.Keys)
            {
                fallingPieces[thisKey].UpdatePiece();

                if (fallingPieces[thisKey].verticalOffset == 0)
                    RemoveKeys.Enqueue(thisKey.ToString());
            }

            while (RemoveKeys.Count > 0)
                fallingPieces.Remove(RemoveKeys.Dequeue());
        }

        private void UpdateRotatingPieces()
        {
            Queue<string> RemoveKeys = new Queue<string>(); //Need a queue as you can't modify a dictionary object whilst it is being processed in a foreach loop

            foreach (string thisKey in rotatingPieces.Keys)
            {
                rotatingPieces[thisKey].UpdatePiece();

                if (rotatingPieces[thisKey].rotationTicksRemaining == 0)
                    RemoveKeys.Enqueue(thisKey.ToString());
            }

            while (RemoveKeys.Count > 0)
                rotatingPieces.Remove(RemoveKeys.Dequeue());
        }
    }
}
