using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SmashGoBang.SpriteClasses
{
    public enum PowerUptype {Health, Laser, Spread, Armour};

    public class PowerUpSprite : Sprite
    {
        SpriteManager spriteManager;

        public override Vector2 direction
        {
            get { return speed; }
        }

        float evasionSpeedModifier;
        int evasionRange;
        bool evade = false;

        public PowerUptype powerUpType;

        public PowerUpSprite(Texture2D textureImage, Vector2 position,
            Point frameSize, int collisionOffset, Point currentFrame,
            Point sheetSize, Vector2 speed, SpriteManager spriteManager,
            float evasionSpeedModifier, int evasionRange, int scoreValue, PowerUptype _powerUptype)
            : base(textureImage, position, frameSize, collisionOffset,
            currentFrame, sheetSize, speed, scoreValue)
        {
            this.spriteManager = spriteManager;
            this.evasionSpeedModifier = evasionSpeedModifier;
            this.evasionRange = evasionRange;
            this.powerUpType = _powerUptype;
        }

        public PowerUpSprite(Texture2D textureImage, Vector2 position,
            Point frameSize, int collisionOffset, Point currentFrame,
            Point sheetSize, Vector2 speed, int millisecondsPerFrame,
            SpriteManager spriteManager, float evasionSpeedModifier,
            int evasionRange, int scoreValue, PowerUptype _powerUptype)
            : base(textureImage, position, frameSize, collisionOffset,
            currentFrame, sheetSize, speed, millisecondsPerFrame, scoreValue)
        {
            this.spriteManager = spriteManager;
            this.evasionSpeedModifier = evasionSpeedModifier;
            this.evasionRange = evasionRange;
            this.powerUpType = _powerUptype;
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            // Move the sprite in its direction
            position += speed;

            // Use the Player position to Move the Enemy Sprite closer
            Vector2 player = spriteManager.GetPlayerPosition();

            if (evade)
            {
                // Move away from ReturnPlayer horisontally
                if (player.X < position.X)
                    position.X += Math.Abs(speed.Y);
                else if (player.X > position.X)
                    position.X -= Math.Abs(speed.Y);

                // Move away from ReturnPlayer vertically

                if (player.Y < position.Y)
                    position.Y += Math.Abs(speed.X);
                else if (player.Y > position.Y)
                    position.Y -= Math.Abs(speed.X);
            }
            else
            {
                if (Vector2.Distance(position, player) < evasionRange)
                {
                    // Player is in evasion range
                    // reverse direction and change Speed
                    speed *= -evasionSpeedModifier;
                    evade = true;
                }
            }

            base.Update(gameTime, clientBounds);
        }


    }
}
