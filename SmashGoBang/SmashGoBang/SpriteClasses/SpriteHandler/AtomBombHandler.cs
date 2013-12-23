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
    public class AtomBombHandler
    {
        public int atomBombMinSpeed = 1;
        public int atomBombMaxSpeed = 4;
        public int automatedSpritePointValue = 10;
        public int chasingSpritePointValue = 20;
        public int evadingSpritePointValue = 0;
        public List<Sprite> atomBombList;

        public int nextSpawnTime = 8000;
        public int atomBombSpawnMinMilliSeconds = 5000;
        public int atomBombSpawnMaxMilliSeconds = 8000;

        public AtomBombHandler()
        {
            atomBombList = new List<Sprite>();
        }

        public List<Sprite> SpawnAtomBomb(Game Game, SpriteManager spriteManager)
        {
            Vector2 speed = Vector2.Zero;
            Vector2 position = Vector2.Zero;
            Point frameSize = new Point(80, 80);

            var yBounds = Game.GraphicsDevice.PresentationParameters.BackBufferHeight
                        - frameSize.Y - 80;
            var xBounds = Game.GraphicsDevice.PresentationParameters.BackBufferWidth
                        - frameSize.X;
            var xLine = ((Game1)Game).rnd.Next(atomBombMinSpeed, atomBombMaxSpeed);
            var yLine = ((Game1)Game).rnd.Next(atomBombMinSpeed, atomBombMaxSpeed);

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

                if (random > 50)
                {
                    // Create EvadingSprite
                    atomBombList.Add(
                        new EvadingSprite(Game.Content.Load<Texture2D>(@"Sprites\power\pUp_N"),
                            position, new Point(80, 80), 10, new Point(0, 0),
                            new Point(4, 4), speed, spriteManager, .75f, 150, evadingSpritePointValue));
                }
            }
            return atomBombList;
        }

        public void ResetSpawnTime(Game Game)
        {
            nextSpawnTime = ((Game1)Game).rnd.Next(
                atomBombSpawnMinMilliSeconds,
                atomBombSpawnMaxMilliSeconds);
        }  
    }
}
