using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SmashGoBang.SpriteClasses.SpriteHandler
{
    class EnemyHandler
    {
        public int enemyMinSpeed = 1;
        public int enemyMaxSpeed = 4;
        public int likeliAutomated = 90;
        public int likeliChasing = 5;
        public int automatedSpritePointValue = 10;
        public int chasingSpritePointValue = 20;
        public int evadingSpritePointValue = 0;
        public List<Sprite> enemyList;

        public int nextSpawnTime = 0;
        public int enemySpawnMinMilliSeconds = 200;
        public int enemySpawnMaxMilliSeconds = 800;

        public int timeSinceLastSpawnTimeChange = 0;
        public int nextSpawnTimeChange = 10000;

        public EnemyHandler()
        {
            enemyList = new List<Sprite>();
        }

        public List<Sprite> SpawnEnemy(Game Game, SpriteManager spriteManager)
        {
            Vector2 speed = Vector2.Zero;
            Vector2 position = Vector2.Zero;
            Point frameSize = new Point(80, 80);

            var yBounds = Game.GraphicsDevice.PresentationParameters.BackBufferHeight
                        - frameSize.Y - 80;
            var xBounds = Game.GraphicsDevice.PresentationParameters.BackBufferWidth
                        - frameSize.X;
            var xLine = ((Game1)Game).rnd.Next(enemyMinSpeed, enemyMaxSpeed);
            var yLine = ((Game1)Game).rnd.Next(enemyMinSpeed, enemyMaxSpeed);
            var spedo = ((Game1)Game).rnd.Next(enemyMinSpeed, enemyMaxSpeed);

            var speedLeft2Right = new Vector2(spedo, 0);
            var speedTop2Bottom = new Vector2(0, spedo);

            if (nextSpawnTime <= 0)
            {
                ResetSpawnTime(Game);

                switch (((Game1)Game).rnd.Next(6))
                {
                    //case 0: // LEFT to RIGHT
                    //    position = new Vector2(
                    //        -frameSize.X, ((Game1)Game).rnd.Next(0, yBounds));
                    //    speed = speedLeft2Right;
                    //    break;

                    case 0: // RIGHT to LEFT
                    case 1:
                        position = new Vector2(
                            Game.GraphicsDevice.PresentationParameters.BackBufferWidth,
                            ((Game1)Game).rnd.Next(0, yBounds));
                        speed = -speedLeft2Right;
                        break;

                    case 2: // BOTTOM to TOP
                    case 3:
                        position = new Vector2(((Game1)Game).rnd.Next(0, xBounds),
                            Game.GraphicsDevice.PresentationParameters.BackBufferHeight);
                        speed = -speedTop2Bottom;
                        break;

                    case 4: // TOP to BOTTOM
                    case 5:
                        position = new Vector2(((Game1)Game).rnd.Next(0, xBounds), -frameSize.Y);
                        speed = speedTop2Bottom;
                        break;
                }

                int random = ((Game1)Game).rnd.Next(100);
                if (random < likeliAutomated)
                {
                    // Create AutomatedSprite
                    if (((Game1)Game).rnd.Next(2) == 0)
                    {
                        // Create this type
                        enemyList.Add(
                        new AutomatedSprite(Game.Content.Load<Texture2D>(@"Sprites\Enemy\R_B"),
                            position, new Point(76, 78), 10, new Point(0, 0),
                            new Point(1, 1), speed, automatedSpritePointValue));
                    }
                    else
                    {
                        // Create another type
                        enemyList.Add(
                        new AutomatedSprite(Game.Content.Load<Texture2D>(@"Sprites\Enemy\G_B"),
                            position, new Point(76, 78), 10, new Point(0, 0),
                            new Point(1, 1), speed, automatedSpritePointValue));
                    }
                }
                else if (random < likeliAutomated + likeliChasing)
                {
                    if (((Game1)Game).rnd.Next(2) == 0)
                    {
                        // Create this type
                        enemyList.Add(
                            new ChasingSprite(Game.Content.Load<Texture2D>(@"Sprites\Enemy\P_B"),
                                position, new Point(76, 78), 10, new Point(0, 0),
                                new Point(1, 1), speed, spriteManager, chasingSpritePointValue));
                    }
                    else
                    {
                        // Create another type
                        enemyList.Add(
                            new ChasingSprite(Game.Content.Load<Texture2D>(@"Sprites\Enemy\Y_B"),
                                position, new Point(76, 78), 10, new Point(0, 0),
                                new Point(1, 1), speed, spriteManager, chasingSpritePointValue));
                    }
                }                
            }
            return enemyList;
        }

        public void ResetSpawnTime(Game Game)
        {
            nextSpawnTime = ((Game1)Game).rnd.Next(
                enemySpawnMinMilliSeconds,
                enemySpawnMaxMilliSeconds);
        }        

        public void AdjustSpawnTimes(GameTime gameTime)
        {
            if (enemySpawnMaxMilliSeconds > 150)
            {
                timeSinceLastSpawnTimeChange += gameTime.ElapsedGameTime.Milliseconds;

                if (timeSinceLastSpawnTimeChange > nextSpawnTimeChange)
                {
                    timeSinceLastSpawnTimeChange -= nextSpawnTimeChange;
                    if (enemySpawnMaxMilliSeconds > 500)
                    {
                        enemySpawnMaxMilliSeconds -= 100;
                    }
                    else
                    {
                        enemySpawnMaxMilliSeconds -= 30;
                    }

                    if (enemySpawnMinMilliSeconds > 500)
                    {
                        enemySpawnMinMilliSeconds -= 100;
                    }
                    else
                    {
                        enemySpawnMinMilliSeconds -= 30;
                    }

                }
            }
        }

        public int AdjustMinSpawnTimes(GameTime gameTime, int enemySpawnMaxMilliSeconds, int enemySpawnMinMilliSeconds, int timeSinceLastSpawnTimeChange, int nextSpawnTimeChange)
        {
            if (enemySpawnMinMilliSeconds > 100)
            {
                timeSinceLastSpawnTimeChange += gameTime.ElapsedGameTime.Milliseconds;
                if (timeSinceLastSpawnTimeChange > nextSpawnTimeChange)
                {
                    timeSinceLastSpawnTimeChange -= nextSpawnTimeChange;
                    if (enemySpawnMaxMilliSeconds > 500)
                    {
                        enemySpawnMinMilliSeconds -= 100;
                    }
                    else
                    {
                        enemySpawnMinMilliSeconds -= 30;
                    }
                }
            }
            return enemySpawnMinMilliSeconds;
        }  
    }
}
