using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace SmashGoBang.SpriteClasses
{
    class Explosion : Sprite
    {
        List<Explosion> explosions = new List<Explosion>();

        public override Vector2 direction
        {
            get { return speed; }
        }

        public Explosion(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed,
            int millisecondsPerFrame, int scoreValue)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, millisecondsPerFrame, scoreValue)
        {

        }
       
        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            position += direction;

            base.Update(gameTime, clientBounds);
        }

        public bool IsDone()
        {
            return animationPlayedOnce;
        }

    }
}
