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

using SmashGoBang.SpriteClasses;
using SmashGoBang.SpriteClasses.SpriteHandler;

namespace SmashGoBang
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        SpriteManager spriteManager;

        Texture2D introScreen;
        Sprite mBackgroundOne;
        Sprite mBackgroundTwo;
        Sprite mBackgroundThree;
        Sprite mBackgroundFour;
        Sprite mBackgroundFive;
        Sprite pause;

        StaticSprite healthBar;
        private int delay = 10;
        PlayerHandler playerHandler = new PlayerHandler();
        ScoreHandler scoreHandler = new ScoreHandler();
        
        public Random rnd { get; private set; }

        int currentScore = 0;
        int currentLife;
        SpriteFont scoreFont;

        enum GameState { Start, Paused, InGame, GameOver };
        GameState currentGameState = GameState.Start;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            rnd = new Random();

            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;

        //Test Ändring
        }

        public void AddScore(int score)
        {
            currentScore += score;
        }   

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            spriteManager = new SpriteManager(this);
            Components.Add(spriteManager);
            spriteManager.Enabled = false;
            spriteManager.Visible = false;

            //Background
            mBackgroundOne = new Sprite();
            mBackgroundOne.Scale = 2.0f;

            mBackgroundTwo = new Sprite();
            mBackgroundTwo.Scale = 2.0f;

            mBackgroundThree = new Sprite();
            mBackgroundThree.Scale = 2.0f;

            mBackgroundFour = new Sprite();
            mBackgroundFour.Scale = 2.0f;

            mBackgroundFive = new Sprite();
            mBackgroundFive.Scale = 2.0f;
            //End of background

            //Paused Screen
            pause = new Sprite();
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

            introScreen = this.Content.Load<Texture2D>(@"Sprites/splash2");

            scoreFont = Content.Load<SpriteFont>(@"Fonts\Score");

            healthBar = playerHandler.LoadHealthBar(this, spriteManager);

            // TODO: use this.Content to load your game content here
            //background
            mBackgroundOne.LoadContent(this.Content, "BackGround/BG_01");
            mBackgroundOne.Position = new Vector2(0, 0);

            mBackgroundTwo.LoadContent(this.Content, "BackGround/BG_02");
            mBackgroundTwo.Position = new Vector2(mBackgroundOne.Position.X + mBackgroundOne.Size.Width, 0);

            mBackgroundThree.LoadContent(this.Content, "BackGround/BG_03");
            mBackgroundThree.Position = new Vector2(mBackgroundTwo.Position.X + mBackgroundTwo.Size.Width, 0);

            mBackgroundFour.LoadContent(this.Content, "BackGround/BG_04");
            mBackgroundFour.Position = new Vector2(mBackgroundThree.Position.X + mBackgroundThree.Size.Width, 0);

            mBackgroundFive.LoadContent(this.Content, "BackGround/BG_05");
            mBackgroundFive.Position = new Vector2(mBackgroundFour.Position.X + mBackgroundFour.Size.Width, 0);
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

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Q) == true)
                this.Exit();

            //Används för att updatera hur många liv spelaren har
            currentLife = spriteManager.GetPlayerHealth();   //Här anropas SpriteManager.cs

            if (Keyboard.GetState().IsKeyDown(Keys.P) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.B))
            {
                currentGameState = GameState.Paused;
                spriteManager.Enabled = false; 
                spriteManager.Visible = false;
            }

            // Gamestates
            switch (currentGameState)
            { 
                case GameState.Start:
                    if (Keyboard.GetState().GetPressedKeys().Length > 0 || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A))
                    {
                        currentGameState = GameState.InGame;
                        currentScore = 0;
                        spriteManager.ResetGame();   //Här anropas SpriteManager.cs
                        currentLife = 3;
                        spriteManager.Enabled = true;
                        spriteManager.Visible = true;
                    }
                    break;
                
                case GameState.InGame:
                    if (currentLife <= 0)
                    {                      
                        if (delay <= 0)
                        {
                            if (scoreHandler.CheckTopScores(currentScore)) 
                            { 
                                scoreHandler.AddHiScore(currentScore); 
                                scoreHandler.hiScoreList = scoreHandler.SortScoreList(scoreHandler.hiScoreList);
                            };

                            currentGameState = GameState.GameOver;
                            spriteManager.Enabled = false;
                            spriteManager.Visible = false;
                        }
                        delay--;
                    }
                    break;
                
                case GameState.Paused:
                    if (currentGameState == GameState.InGame)
                    {
                        pause.Position = Vector2.Zero;
                        spriteManager.Enabled = false;
                        spriteManager.Visible = false;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Back) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Start) && currentGameState == GameState.Paused) 
                    {
                        currentGameState = GameState.InGame;
                        spriteManager.Enabled = true;
                        spriteManager.Visible = true;
                    }
                    break;

                case GameState.GameOver:
                    spriteManager.Enabled = false;
                        spriteManager.Visible = false;
                    if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Back))
                    {
                        Exit();
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.R) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Y))
                    {
                        delay = 10;
                        currentGameState = GameState.Start;
                        //spriteManager.ResetGame();                       
                    }
                    break;
            }

            
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // Update healthBar
            healthBar.Update(gameTime, spriteManager.Game.Window.ClientBounds);
            
            // TODO: Add your update logic here
            //Scrolling background
            if (mBackgroundOne.Position.X < -mBackgroundOne.Size.Width)
            {
                mBackgroundOne.Position.X = mBackgroundFive.Position.X + mBackgroundFive.Size.Width;
            }
            if (mBackgroundTwo.Position.X < -mBackgroundTwo.Size.Width)
            {
                mBackgroundTwo.Position.X = mBackgroundOne.Position.X + mBackgroundOne.Size.Width;
            }

            if (mBackgroundThree.Position.X < -mBackgroundThree.Size.Width)
            {
                mBackgroundThree.Position.X = mBackgroundTwo.Position.X + mBackgroundTwo.Size.Width;
            }

            if (mBackgroundFour.Position.X < -mBackgroundFour.Size.Width)
            {
                mBackgroundFour.Position.X = mBackgroundThree.Position.X + mBackgroundThree.Size.Width;
            }

            if (mBackgroundFive.Position.X < -mBackgroundFive.Size.Width)
            {
                mBackgroundFive.Position.X = mBackgroundFour.Position.X + mBackgroundFour.Size.Width;
            }

            Vector2 aDirection = new Vector2(-1, 0);
            Vector2 aSpeed = new Vector2(160, 0);

            mBackgroundOne.Position += aDirection * aSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            mBackgroundTwo.Position += aDirection * aSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            mBackgroundThree.Position += aDirection * aSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            mBackgroundFour.Position += aDirection * aSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            mBackgroundFive.Position += aDirection * aSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //End of scrolling background

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Draw based on GameState
            switch (currentGameState)
            {
                case GameState.Start:
                    GraphicsDevice.Clear(Color.AliceBlue);

                    // INTRO SCREEN
                    spriteBatch.Begin();
                    
                    spriteBatch.Draw(introScreen, new Vector2(0,0), Color.White);
                                                            
                    spriteBatch.End();
                    break;

                case GameState.Paused:
                    GraphicsDevice.Clear(Color.AliceBlue);

                    // Draw Text for paused screen
                    spriteBatch.Begin();
                    
                    string pausetext = "Paused!!!";
                    spriteBatch.DrawString(scoreFont, pausetext,
                        new Vector2((Window.ClientBounds.Width / 2)
                            - (scoreFont.MeasureString(pausetext).X / 2),
                            (Window.ClientBounds.Height / 2)
                            - (scoreFont.MeasureString(pausetext).Y / 2)),
                            Color.DarkSalmon);

                    string text = "(Press back to return!)";
                    spriteBatch.DrawString(scoreFont, text,
                        new Vector2((Window.ClientBounds.Width / 2)
                            - (scoreFont.MeasureString(text).X / 2),
                            (Window.ClientBounds.Height / 2)
                            - (scoreFont.MeasureString(text).Y / 2) + 30),
                            Color.DarkSalmon);

                    spriteBatch.End();
                    break;

                case GameState.InGame:

                    GraphicsDevice.Clear(Color.CornflowerBlue);

                    // TODO: Add your drawing code here
                    spriteBatch.Begin();

                    mBackgroundOne.Draw(this.spriteBatch);
                    mBackgroundTwo.Draw(this.spriteBatch);
                    mBackgroundThree.Draw(this.spriteBatch);
                    mBackgroundFour.Draw(this.spriteBatch);
                    mBackgroundFive.Draw(this.spriteBatch);

                    spriteBatch.End();

                    // Draw scoreFont
                    spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

                    spriteBatch.DrawString(scoreFont, "Score: " + currentScore,
                        new Vector2(10, 540), Color.LightPink, 0, Vector2.Zero,
                        1, SpriteEffects.None, 1);

                    spriteBatch.DrawString(scoreFont, "Health",  
                        new Vector2(260, 540), Color.LightPink, 0, Vector2.Zero,
                        1, SpriteEffects.None, 1);

                    spriteBatch.DrawString(scoreFont, "Ammo: " + spriteManager.GetPlayerAmmo(),
                        new Vector2(560, 540), Color.LightPink, 0, Vector2.Zero,
                        1, SpriteEffects.None, 1);


                    // HealthBar
                    healthBar.Draw(gameTime, spriteBatch);

                    //Rita black bar här
                    spriteBatch.DrawLine(new Vector2(0, 550), 800.0f, 0.0f, Color.Black, 100.0f);
                    spriteBatch.DrawLine(new Vector2(0, 530), 800.0f, 0.0f, Color.Black, 20.0f);
                       
                    spriteManager.DrawSprites(gameTime);   //Här anropas SpriteManager.cs

                    spriteBatch.End();                   


                    break;

                case GameState.GameOver:        //Här ska gameover screen koden läggas in

                    GraphicsDevice.Clear(Color.Purple);

                    GamePad.SetVibration(PlayerIndex.One, 0f, 0f);

                    // Draw Text for outro screen
                    spriteBatch.Begin();
                    string GameOverText = "You died, too bad :-(";
                    string yourScore = "Your score is " + currentScore;
                    string GpStart = "GamePad: Press Y to start over";
                    string GpClose = "Back to close";
                    string KbStart = "KeyBoard: Press R to Try Again";
                    string KbClose = "Press Q to close";

                    spriteBatch.DrawString(scoreFont, GameOverText,
                        new Vector2((Window.ClientBounds.Width / 2)
                            - (scoreFont.MeasureString(GameOverText).X / 2),
                            (Window.ClientBounds.Height / 2)
                            - (scoreFont.MeasureString(GameOverText).Y / 2) - 250),
                            Color.DarkSalmon);
                    spriteBatch.DrawString(scoreFont, yourScore,
                        new Vector2((Window.ClientBounds.Width / 2)
                            - (scoreFont.MeasureString(yourScore).X / 2),
                            (Window.ClientBounds.Height / 2)
                            - (scoreFont.MeasureString(yourScore).Y / 2) - 200),
                            Color.DarkSalmon);
                    spriteBatch.DrawString(scoreFont, GpStart,
                        new Vector2((Window.ClientBounds.Width / 2)
                            - (scoreFont.MeasureString(GpStart).X / 2)+55,  
                            (Window.ClientBounds.Height / 2)
                            - (scoreFont.MeasureString(GpStart).Y / 2) - 70),
                            Color.DarkSalmon, 0.0f, Vector2.Zero, 
                            0.8f, SpriteEffects.None, 0.0f);
                    spriteBatch.DrawString(scoreFont, GpClose,
                        new Vector2((Window.ClientBounds.Width / 2)
                            - (scoreFont.MeasureString(GpClose).X / 2)+30,
                            (Window.ClientBounds.Height / 2)
                            - (scoreFont.MeasureString(GpClose).Y / 2) - 20),
                            Color.DarkSalmon, 0.0f, Vector2.Zero,
                            0.8f, SpriteEffects.None, 0.0f);
                    spriteBatch.DrawString(scoreFont, KbStart,
                        new Vector2((Window.ClientBounds.Width / 2)
                            - (scoreFont.MeasureString(KbStart).X / 2) +55,
                            (Window.ClientBounds.Height / 2)
                            - (scoreFont.MeasureString(KbStart).Y / 2) + 30),
                            Color.DarkSalmon, 0.0f, Vector2.Zero,
                            0.8f, SpriteEffects.None, 0.0f);
                    spriteBatch.DrawString(scoreFont, KbClose,
                        new Vector2((Window.ClientBounds.Width / 2)
                            - (scoreFont.MeasureString(KbClose).X / 2) + 45,
                            (Window.ClientBounds.Height / 2)
                            - (scoreFont.MeasureString(KbClose).Y / 2) + 80),
                            Color.DarkSalmon, 0.0f, Vector2.Zero,
                            0.8f, SpriteEffects.None, 0.0f);
                    spriteBatch.DrawString(scoreFont, "HI-SCORE LIST",
                        new Vector2((Window.ClientBounds.Width / 2)
                            - (scoreFont.MeasureString(KbClose).X / 2) + 60,
                            (Window.ClientBounds.Height / 2)
                            - (scoreFont.MeasureString(KbClose).Y / 2) + 120),
                            Color.DarkSalmon, 0.0f, Vector2.Zero,
                            0.8f, SpriteEffects.None, 0.0f);

                    int scoreCount = 1;
                    string whatToWrite;

                    foreach (Score s in scoreHandler.hiScoreList)
                    {
                        whatToWrite = "nr " + scoreCount + ": "+ s.name + " " + s.score;
                        if (s.score != currentScore)
                        {
                            spriteBatch.DrawString(scoreFont, whatToWrite,
                                new Vector2((Window.ClientBounds.Width / 2)
                                    - (scoreFont.MeasureString(KbClose).X / 2) + 20,
                                    (Window.ClientBounds.Height / 2)
                                    - (scoreFont.MeasureString(KbClose).Y / 2) + (130 + (30 * scoreCount))),
                                    Color.DarkSalmon, 0.0f, Vector2.Zero,
                                    0.8f, SpriteEffects.None, 0.0f);
                        }
                        else
                        {
                            spriteBatch.DrawString(scoreFont, whatToWrite,
                               new Vector2((Window.ClientBounds.Width / 2)
                                   - (scoreFont.MeasureString(KbClose).X / 2) + 20,
                                   (Window.ClientBounds.Height / 2)
                                   - (scoreFont.MeasureString(KbClose).Y / 2) + (130 + (30 * scoreCount))),
                                   Color.Red, 0.0f, Vector2.Zero,
                                   0.8f, SpriteEffects.None, 0.0f);
                        }

                        scoreCount++;
                    }

                    spriteBatch.End();
                    break;
            }
            
            base.Draw(gameTime);
        }
    }
}
