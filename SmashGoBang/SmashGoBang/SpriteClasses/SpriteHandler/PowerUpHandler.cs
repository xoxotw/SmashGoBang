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
    public class PowerUpHandler
    {
        public int powerUpMinSpeed = 1;
        public int powerUpMaxSpeed = 3;
        public int likeliAutomated = 90;
        public int likeliChasing = 5;
        public int automatedSpritePointValue = 10;
        public int chasingSpritePointValue = 20;
        public int evadingSpritePointValue = 0;
        public List<PowerUpSprite> powerUpList;

        public int nextSpawnTime = 10000; //First powerUp after 10sec
        public int powerUpSpawnMinMilliSeconds = 12000; //12 sec
        public int powerUpSpawnMaxMilliSeconds = 16000; //16 sec
        public int timeSinceLastSpawnTimeChange = 0;
        public int nextSpawnTimeChange = 10000;

        public PowerUpHandler()
        {
            powerUpList = new List<PowerUpSprite>();
        }

        public List<PowerUpSprite> SpawnPowerUp(Game Game, SpriteManager spriteManager)
        {
            Vector2 speed = Vector2.Zero;
            Vector2 position = Vector2.Zero;
            Point frameSize = new Point(80, 80);

            var yBounds = Game.GraphicsDevice.PresentationParameters.BackBufferHeight
                        - frameSize.Y - 80;
            var xBounds = Game.GraphicsDevice.PresentationParameters.BackBufferWidth
                        - frameSize.X;
            var xLine = ((Game1)Game).rnd.Next(powerUpMinSpeed, powerUpMaxSpeed);
            var yLine = ((Game1)Game).rnd.Next(powerUpMinSpeed, powerUpMaxSpeed);

            var speedLeft2Right = new Vector2(xLine, 0);
            var speedTop2Bottom = new Vector2(0, yLine);

            if (nextSpawnTime <= 0)
            {

                ResetSpawnTime(Game);

                switch (((Game1)Game).rnd.Next(4))
                {
                    case 0: // LEFT to RIGHT
                        position = new Vector2(
                            -frameSize.X, ((Game1)Game).rnd.Next(0, yBounds));
                        speed = speedLeft2Right;
                        break;

                    case 1: // RIGHT to LEFT
                        position = new Vector2(
                            Game.GraphicsDevice.PresentationParameters.BackBufferWidth,
                            ((Game1)Game).rnd.Next(0, yBounds));
                        speed = -speedLeft2Right;
                        break;

                    case 2: // BOTTOM to TOP
                        position = new Vector2(((Game1)Game).rnd.Next(0, xBounds),
                            Game.GraphicsDevice.PresentationParameters.BackBufferHeight);
                        speed = -speedTop2Bottom;
                        break;

                    case 3: // TOP to BOTTOM
                        position = new Vector2(((Game1)Game).rnd.Next(0, xBounds), -frameSize.Y);
                        speed = speedTop2Bottom;
                        break;
                }

                int random = ((Game1)Game).rnd.Next(100);

                if (random >= 69)  //Var likeliAutomated + likeliChasing
                {
                    // Create EvadingSprite
                    //powerUpList.Add(
                    //    new PowerUpSprite(Game.Content.Load<Texture2D>(@"Sprites\powerUp"),
                    //        position, new Point(80, 80), 10, new Point(0, 0),
                    //        new Point(4, 4), speed, spriteManager, .75f, 150, evadingSpritePointValue, 0, PowerUptype.Laser));

                    powerUpList.Add(
                        new PowerUpSprite(Game.Content.Load<Texture2D>(@"Sprites\power\pUp_L"),
                            position, new Point(80, 80), 10, new Point(0, 0),
                            new Point(4, 4), speed, spriteManager, .75f, evadingSpritePointValue, 0, PowerUptype.Laser));
                }
                if (random >= 39 && random < 69)
                {
                    powerUpList.Add(
                        new PowerUpSprite(Game.Content.Load<Texture2D>(@"Sprites\power\pUp_Sp"),
                            position, new Point(80, 80), 10, new Point(0, 0),
                            new Point(4, 4), speed, spriteManager, .75f, evadingSpritePointValue, 0, PowerUptype.Spread));
                }
                if (random >= 19 && random < 39)
                {
                    powerUpList.Add(
                        new PowerUpSprite(Game.Content.Load<Texture2D>(@"Sprites\power\pUp_H"),
                            position, new Point(80, 80), 10, new Point(0, 0),
                            new Point(4, 4), speed, spriteManager, .75f, evadingSpritePointValue, 0, PowerUptype.Health));
                }
                if (random < 19)
                {
                    powerUpList.Add(
                        new PowerUpSprite(Game.Content.Load<Texture2D>(@"Sprites\power\pUp_Sh"),
                            position, new Point(80, 80), 10, new Point(0, 0),
                            new Point(4, 4), speed, spriteManager, .75f, evadingSpritePointValue, 0, PowerUptype.Armour));
                }
            }

            return powerUpList;
        }



        public void ResetSpawnTime(Game Game)
        {
            nextSpawnTime = ((Game1)Game).rnd.Next(
                powerUpSpawnMinMilliSeconds,
                powerUpSpawnMaxMilliSeconds);
        }

        public void AdjustSpawnTimes(GameTime gameTime)
        {
            if (powerUpSpawnMaxMilliSeconds > 1000)
            {
                timeSinceLastSpawnTimeChange += gameTime.ElapsedGameTime.Milliseconds;
                if (timeSinceLastSpawnTimeChange > nextSpawnTimeChange)
                {
                    //timeSinceLastSpawnTimeChange -= nextSpawnTimeChange;
                    if (powerUpSpawnMaxMilliSeconds > 5000)
                    {
                        powerUpSpawnMaxMilliSeconds -= 250;
                    }
                    else if(powerUpSpawnMaxMilliSeconds > 50)
                    {
                        powerUpSpawnMaxMilliSeconds -= 30;
                    }
                }
            }
            if (powerUpSpawnMinMilliSeconds > 800)
            {
                timeSinceLastSpawnTimeChange += gameTime.ElapsedGameTime.Milliseconds;
                if (timeSinceLastSpawnTimeChange > nextSpawnTimeChange)
                {
                    timeSinceLastSpawnTimeChange -= nextSpawnTimeChange;
                    if (powerUpSpawnMinMilliSeconds > 4000)
                    {
                        powerUpSpawnMinMilliSeconds -= 250;
                    }
                    else if (powerUpSpawnMinMilliSeconds > 50)
                    {
                        powerUpSpawnMinMilliSeconds -= 30;
                    }
                }
            }
        }
    }
}
