using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Flood_Control
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D playingPieces;
        Texture2D backgroundScreen;
        Texture2D gameOverScreen;
        Texture2D titleScreen;

        GameBoard gameBoard;

        Vector2 gameBoardDisplayOrigin = new Vector2(70, 89);
        Vector2 gameOverLocation = new Vector2(200, 260);
        Vector2 levelTexPosition = new Vector2(512, 215);
        Vector2 scorePosition = new Vector2(605, 215);
        Vector2 waterOverlayStart = new Vector2(85, 245);
        Vector2 waterPosition = new Vector2(478, 338);

        int currentLevel = 0;
        int scoringLockedPieces = 0;
        int linesCompletedThisLevel = 0;
        const int maxWaterHeight = 244;
        int playerScore = 0;
        const int waterWidth = 297;

        SpriteFont pericles36Font;

        KeyboardState prevKeyboardState;

        enum GameStates
        {
            TitleScreen,
            Playing,
            GameOver,
            Paused
        };

        GameStates gameState;

        Rectangle emptyPiece = new Rectangle(1, 247, 40, 40);

        float gameOverTimer;
        const float FloodAccelerationLevel = 0.5f;
        float floodCount = 0.0f;
        float floodIncreaseAmount = 0.5f;
        const float minTimeSinceLastInput = 0.25f;
        const float maxFloodCounter = 100.0f;
        float timeBetweenFloodIncreases = 1.0f;
        float timeSinceLastFloodIncrease = 0.0f;
        float timeSinceLastInput = 0.0f;

        Queue<ScoreZoom> scoreZooms = new Queue<ScoreZoom>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();
            gameBoard = new GameBoard();
            gameState = GameStates.TitleScreen;

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
            playingPieces = Content.Load<Texture2D>(@"Textures\Tile_Sheet");
            backgroundScreen = Content.Load<Texture2D>(@"Textures\Background");
            titleScreen = Content.Load<Texture2D>(@"Textures\TitleScreen");
            gameOverScreen = Content.Load<Texture2D>(@"Textures\floodedLab");
            pericles36Font = Content.Load<SpriteFont>(@"Fonts\PEricles36");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            switch (gameState)
            {
                case GameStates.Paused:
                    timeSinceLastInput += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (timeSinceLastInput >= minTimeSinceLastInput)
                    {
                        HandleKeyboardInput(Keyboard.GetState());
                    }
                    break;
                case GameStates.TitleScreen:
                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        gameBoard.ClearBoard();
                        gameBoard.GenerateNewPieces(false);
                        playerScore = 0;
                        currentLevel = 0;
                        floodIncreaseAmount = 0.0f;
                        StartNewLevel();
                        gameState = GameStates.Playing;
                    }
                    break;
                case GameStates.Playing:
                    timeSinceLastInput += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    timeSinceLastFloodIncrease += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (timeSinceLastFloodIncrease >= timeBetweenFloodIncreases)
                    {
                        floodCount += floodIncreaseAmount;
                        timeSinceLastFloodIncrease = 0.0f;
                        if (floodCount >= maxFloodCounter)
                        {
                            gameOverTimer = 8.0f;
                            gameState = GameStates.GameOver;
                        }
                    }

                    if (gameBoard.ArePiecesAnimating())
                    {
                        gameBoard.UpdateAnimatedPieces();
                    }
                    else
                    {
                        if (timeSinceLastInput >= minTimeSinceLastInput)
                        {
                            HandleMouseInput(Mouse.GetState());
                            HandleKeyboardInput(Keyboard.GetState());
                        }

                        gameBoard.ResetWater();

                        for (int y = 0; y < GameBoard.GameBoardHeight; y++)
                            CheckScoringChain(gameBoard.GetWaterChain(y));
                        scoringLockedPieces = 0; //Reset the scoring locked pieces multiplier to 0 as we've finished using it now.
                        gameBoard.GenerateNewPieces(true);
                    }

                    UpdateScoreZooms();
                    break;
                case GameStates.GameOver:
                    gameOverTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (gameOverTimer <= 0)
                        gameState = GameStates.TitleScreen;
                    break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Iterate through the queue of ScoreZooms and updates them.
        /// Removes them if they have completed their zooming.
        /// </summary>
        private void UpdateScoreZooms()
        {
            int dequeueCounter = 0;
            foreach (ScoreZoom zoom in scoreZooms)
            {
                zoom.Update();
                if (zoom.IsCompleted())
                    dequeueCounter++;
            }

            for (int d = 0; d < dequeueCounter; d++)
                scoreZooms.Dequeue();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            if (gameState == GameStates.TitleScreen)
            {
                spriteBatch.Begin();

                spriteBatch.Draw(titleScreen,
                    new Rectangle(0, 0,
                        this.Window.ClientBounds.Width,
                        this.Window.ClientBounds.Height),
                    Color.White);

                spriteBatch.End();
            }

            if (gameState == GameStates.Paused)
            {
                spriteBatch.Begin();

                spriteBatch.Draw(backgroundScreen,
                    new Rectangle(0, 0,
                        this.Window.ClientBounds.Width,
                        this.Window.ClientBounds.Height),
                    Color.White);

                for (int x = 0; x < GameBoard.GameBoardWidth; x++)
                    for (int y = 0; y < GameBoard.GameBoardHeight; y++)
                    {
                        int pixelX = (int)gameBoardDisplayOrigin.X + (x * GamePiece.PieceWidth);
                        int pixelY = (int)gameBoardDisplayOrigin.Y + (y * GamePiece.PieceHeight);

                        DrawEmptyPiece(pixelX, pixelY);
                    }

                spriteBatch.DrawString(pericles36Font, playerScore.ToString(), scorePosition, Color.Black); //Draw the player's score

                spriteBatch.DrawString(pericles36Font,
                    currentLevel.ToString(),
                    levelTexPosition,
                    Color.Black);

                //Draw the water level
                int waterHeight = (int)(maxWaterHeight * (floodCount / 100));
                spriteBatch.Draw(backgroundScreen,
                    new Rectangle((int)waterPosition.X, (int)waterPosition.Y + (maxWaterHeight - waterHeight), waterWidth, waterHeight),
                    new Rectangle((int)waterOverlayStart.X, (int)waterOverlayStart.Y + (maxWaterHeight - waterHeight), waterWidth, waterHeight),
                    new Color(255, 255, 255, 180));

                spriteBatch.DrawString(pericles36Font, "P A U S E D", gameOverLocation, Color.Yellow);

                spriteBatch.End();
            }

            if ((gameState == GameStates.Playing) || (gameState == GameStates.GameOver))
            {
                spriteBatch.Begin();

                spriteBatch.Draw(backgroundScreen,
                    new Rectangle(0, 0,
                        this.Window.ClientBounds.Width,
                        this.Window.ClientBounds.Height),
                    Color.White);                

                for (int x = 0; x <  GameBoard.GameBoardWidth; x++)
                    for (int y = 0; y < GameBoard.GameBoardHeight; y++)
                    {
                        int pixelX = (int)gameBoardDisplayOrigin.X + (x * GamePiece.PieceWidth);
                        int pixelY = (int)gameBoardDisplayOrigin.Y + (y * GamePiece.PieceHeight);

                        DrawEmptyPiece(pixelX, pixelY);

                        bool pieceDrawn = false;

                        string positionName = x.ToString() + "_" + y.ToString();

                        if (gameBoard.rotatingPieces.ContainsKey(positionName))
                        {
                            DrawRotatingPiece(pixelX, pixelY, positionName);
                            pieceDrawn = true;
                        }

                        if (gameBoard.fadingPieces.ContainsKey(positionName))
                        {
                            DrawFadingPiece(pixelX, pixelY, positionName);
                            pieceDrawn = true;
                        }

                        if (gameBoard.fallingPieces.ContainsKey(positionName))
                        {
                            DrawFallingPiece(pixelX, pixelY, positionName);
                            pieceDrawn = true;
                        }

                        if (!pieceDrawn) //if the piece is not animated then draw the underlying piece
                            DrawStandardPiece(x, y, pixelX, pixelY);
                    }                

                //Draw all of the score zooms effects
                foreach (ScoreZoom zoom in scoreZooms)
                {
                    spriteBatch.DrawString(pericles36Font, //Texture
                        zoom.Text, //Text
                        new Vector2(this.Window.ClientBounds.Width / 2, this.Window.ClientBounds.Height / 2), //Position
                        zoom.DrawColour, //Colour
                        0.0f, //Rotation
                        new Vector2(pericles36Font.MeasureString(zoom.Text).X / 2, pericles36Font.MeasureString(zoom.Text).Y / 2), //Origin - Centre
                        zoom.Scale, //Scale
                        SpriteEffects.None, //No sprite effects
                        0.0f); //No layering
                }
                spriteBatch.DrawString(pericles36Font, playerScore.ToString(), scorePosition, Color.Black); //Draw the player's score

                spriteBatch.DrawString(pericles36Font,
                    currentLevel.ToString(),
                    levelTexPosition,
                    Color.Black);

                //Draw the water level
                int waterHeight = (int)(maxWaterHeight * (floodCount / 100));
                spriteBatch.Draw(backgroundScreen,
                    new Rectangle((int)waterPosition.X, (int)waterPosition.Y + (maxWaterHeight - waterHeight), waterWidth, waterHeight),
                    new Rectangle((int)waterOverlayStart.X, (int)waterOverlayStart.Y + (maxWaterHeight - waterHeight), waterWidth, waterHeight),
                    new Color(255, 255, 255, 180));


                spriteBatch.End();
            }

            if (gameState == GameStates.GameOver)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(gameOverScreen,
                    new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height),
                    Color.White);

                spriteBatch.DrawString(
                    pericles36Font,
                    "G A M E O V E R !",
                    gameOverLocation,
                    Color.Yellow
                    );
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }

        private void DrawEmptyPiece(int pixelX, int pixelY)
        {
            spriteBatch.Draw(
                playingPieces,
                new Rectangle(pixelX, pixelY,
                GamePiece.PieceWidth, GamePiece.PieceHeight),
                emptyPiece,
                Color.White
                );
        }

        private void DrawStandardPiece(int x, int y, int pixelX, int pixelY)
        {
            spriteBatch.Draw(
                playingPieces,
                new Rectangle(pixelX, pixelY, GamePiece.PieceWidth, GamePiece.PieceHeight),
                gameBoard.GetSourecRect(x, y),
                Color.White
                );
        }

        private void DrawFallingPiece(int pixelX, int pixelY, string positionName)
        {
            spriteBatch.Draw(
                playingPieces,
                new Rectangle(pixelX, pixelY - gameBoard.fallingPieces[positionName].verticalOffset, GamePiece.PieceWidth, GamePiece.PieceHeight),
                gameBoard.fallingPieces[positionName].GetSourceRect(),
                Color.White
                );
        }

        private void DrawFadingPiece(int pixelX, int pixelY, string positionName)
        {
            spriteBatch.Draw(
                playingPieces,
                new Rectangle(pixelX, pixelY, GamePiece.PieceWidth, GamePiece.PieceHeight),
                gameBoard.fadingPieces[positionName].GetSourceRect(),
                Color.White * gameBoard.fadingPieces[positionName].alphaLevel
                );
        }

        private void DrawRotatingPiece(int pixelX, int pixelY, string positionName)
        {
            spriteBatch.Draw(
                playingPieces, //texture
                new Rectangle(pixelX + (GamePiece.PieceHeight / 2), pixelY + (GamePiece.PieceWidth / 2), GamePiece.PieceWidth, GamePiece.PieceHeight),//destination
                gameBoard.rotatingPieces[positionName].GetSourceRect(),//source
                Color.White,//colour
                gameBoard.rotatingPieces[positionName].RotationAmount,//rotation
                new Vector2(GamePiece.PieceWidth / 2, GamePiece.PieceHeight / 2),//rotation origin = centre
                SpriteEffects.None, //no sprite effects
                0.0f//no layer depth
                );
        }

        private int DetermineScore(int squareCount)
        {
            return (int)((Math.Pow((squareCount / 5), 2) + squareCount + scoringLockedPieces) * 10);
        }

        private void CheckScoringChain(List<Vector2> waterChain)
        {
            if (waterChain.Count > 0)
            {
                Vector2 lastPipe = waterChain[waterChain.Count - 1];

                // Make sure the last piece in the scoring chain is the last game square on a row.
                // If it is is then make sure the game piece has a "Right" connector, indicating a completed chain/row from left to right across the game board.
                if (lastPipe.X == GameBoard.GameBoardWidth - 1)
                {
                    if (gameBoard.HasConnector((int)lastPipe.X, (int)lastPipe.Y, "Right"))
                    {
                        playerScore += DetermineScore(waterChain.Count);
                        linesCompletedThisLevel++;
                        floodCount = MathHelper.Clamp(floodCount - (DetermineScore(waterChain.Count) / 10), 0.0f, 100.0f);
                        scoreZooms.Enqueue(new ScoreZoom("+" + DetermineScore(waterChain.Count).ToString(), new Color(1.0f, 0.0f, 0.0f, 0.4f)));

                        foreach (Vector2 scoringSquare in waterChain)
                        {
                            //Add a fading piece by getting the piece type (GetSquare)
                            //Then add an empty square in the scoring square position, this will then become visible as the fading piece begins to fall down
                            gameBoard.AddFadingPiece((int)scoringSquare.X,
                                (int)scoringSquare.Y,
                                gameBoard.GetSquare((int)scoringSquare.X, (int)scoringSquare.Y));
                            gameBoard.SetSquare((int)scoringSquare.X, (int)scoringSquare.Y, "Empty");

                            string piece = scoringSquare.X.ToString() + "_" + scoringSquare.Y.ToString();
                            if (gameBoard.fadingPieces[piece].PieceSuffix.Contains("L"))
                                scoringLockedPieces++; //Got a locked scoring piece? Use it as a multiplier

                        }

                        if (linesCompletedThisLevel >= 10)
                            StartNewLevel();
                    }

                    
                }
            }
        }

        private void HandleKeyboardInput(KeyboardState keyboardState)
        {
            //Has the key been pressed and released?
            if ( (prevKeyboardState.IsKeyDown(Keys.P)) && (keyboardState.IsKeyUp(Keys.P)) )
            {
                if (gameState == GameStates.Playing)
                    gameState = GameStates.Paused;
                else if (gameState == GameStates.Paused)
                    gameState = GameStates.Playing;
            }
            prevKeyboardState = keyboardState;
        }

        /// <summary>
        /// Helper class to handle the input of the user for the mouse
        /// </summary>
        /// <param name="mouseState">The state of the mouse</param>
        private void HandleMouseInput(MouseState mouseState)
        {
            //MouseState class reports the X and Y position of the mouse relative to the upper left corner of the window.
            //What we really need to know is which game square/game piece is being clicked.
            int x = ((mouseState.X - (int)gameBoardDisplayOrigin.X) / GamePiece.PieceWidth);
            int y = ((mouseState.Y - (int)gameBoardDisplayOrigin.Y) / GamePiece.PieceHeight);

            //Does the mouse click fall within the bounds of the game board?
            if ((x >= 0) && (x < GameBoard.GameBoardWidth) && (y >= 0) && (y < GameBoard.GameBoardHeight))
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    if (gameBoard.BoardSquares[x,y].PieceSuffix.Contains("L") == false)
                    {
                        gameBoard.AddRotatingPiece(x, y, gameBoard.GetSquare(x, y), false);
                        gameBoard.RotatePiece(x, y, false);                        
                    }
                    timeSinceLastInput = 0.0f;

                }

                if (mouseState.RightButton == ButtonState.Pressed)
                {
                    if (gameBoard.BoardSquares[x, y].PieceSuffix.Contains("L") == false)
                    {
                        gameBoard.AddRotatingPiece(x, y, gameBoard.GetSquare(x, y), true);
                        gameBoard.RotatePiece(x, y, true);
                    }
                    timeSinceLastInput = 0.0f;
                }
            }
        }

        private void StartNewLevel()
        {
            currentLevel++;
            floodCount = 0.0f;
            linesCompletedThisLevel = 0;
            floodIncreaseAmount += FloodAccelerationLevel;
            gameBoard.ClearBoard();
            gameBoard.GenerateNewPieces(false);
        }
    }
}
