using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SmashGoBang.SpriteClasses
{
    class ChasingSprite : Sprite
    {
        SpriteManager spriteManager;

        public override Vector2 direction
        {
            get { return speed; }
        }

        public ChasingSprite(Texture2D textureImage, Vector2 position,
            Point frameSize, int collisionOffset, Point currentFrame,
            Point sheetSize, Vector2 speed, SpriteManager spriteManager,
            int scoreValue)
            : base(textureImage, position, frameSize, collisionOffset,
            currentFrame, sheetSize, speed, scoreValue)
        {
            this.spriteManager = spriteManager;
        }

        public ChasingSprite(Texture2D textureImage, Vector2 position,
            Point frameSize, int collisionOffset, Point currentFrame,
            Point sheetSize, Vector2 speed, int millisecondsPerFrame,
            SpriteManager spriteManager, int scoreValue)
            : base(textureImage, position, frameSize, collisionOffset,
            currentFrame, sheetSize, speed, millisecondsPerFrame, scoreValue)
        {
            this.spriteManager = spriteManager;
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            // Move the sprite in its direction
            position += speed;

            // Use the Player position to Move the Enemy Sprite closer
            Vector2 player = spriteManager.GetPlayerPosition();

            // if ReturnPlayer is moving vertically, chase horisontally
            if (speed.X == 0)
            {
                if (player.X < position.X)
                    position.X -= Math.Abs(speed.Y);
                else if (player.X > position.X)
                    position.X += Math.Abs(speed.Y);
            }
            // if ReturnPlayer moves horisontally, chase vertically
            if (speed.Y == 0)
            {
                if (player.Y < position.Y)
                    position.Y -= Math.Abs(speed.X);
                else if (player.Y > position.Y)
                    position.Y += Math.Abs(speed.X);
            }

            base.Update(gameTime, clientBounds);
        }


    }
}
