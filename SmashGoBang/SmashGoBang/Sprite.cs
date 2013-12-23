using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SmashGoBang
{
    public class Sprite
    {
        //The rectangular area from the original image that defines the Sprite
        Rectangle mSource;
        public Rectangle Source
        {
            get { return mSource; }
            set
            {
                mSource = value;
                Size = new Rectangle(0, 0,
                    (int)(mSource.Width * Scale), (int)(mSource.Height * Scale));
            }
        }

        //The size of the Sprite (with scale applied)
        public Rectangle Size;

        //Used to size the Sprite up or down from the original image
        public float Scale = 0.5f;

        //the current position of the Sprite
        public Vector2 Position = new Vector2(0, 0);

        //The texture object used when drawing the sprite
        public Texture2D mSpriteTexture;

        //Load the texture for the sprite using the Content Pipeline
        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            mSpriteTexture = theContentManager.Load<Texture2D>(theAssetName);

            Source = new Rectangle(0, 0, mSpriteTexture.Width, mSpriteTexture.Height);
            Size = new Rectangle(0, 0, (int)(mSpriteTexture.Width * Scale),
                (int)(mSpriteTexture.Height * Scale));
        }

        //Update the Sprite and change it'b position based on the passed in speed, direction and elapsed time.
        public void Update(GameTime theGameTime, Vector2 theSpeed, Vector2 theDirection)
        {
            Position += theDirection * theSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
        }

        //Draw the sprite to the screen
        public virtual void Draw(SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.Draw(mSpriteTexture, Position,
                 new Rectangle(0, 0, mSpriteTexture.Width, mSpriteTexture.Height), Color.White,
                 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }
    }
}
