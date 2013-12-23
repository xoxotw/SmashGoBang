using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SmashGoBang.SpriteClasses
{
    public class StaticSprite
    {
        private SpriteManager spriteManager;

        Texture2D textureImage;
        Point frameSize;
        Rectangle selectedFrame;
        Vector2 position;

        // 0 ,  30 , 60 or 90
        //public int selectedFrameOffset = 0;

        public StaticSprite(Texture2D textureImage,
            Point frameSize, Rectangle selectedFrame,
            Vector2 position, SpriteManager sManager)
        {
            spriteManager = sManager;
            this.textureImage = textureImage;
            this.frameSize = frameSize;
            this.selectedFrame = selectedFrame;
            this.position = position;
        }

        public virtual void Update(GameTime gameTime, Rectangle clientBounds)
        {

        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureImage,
                position,
                new Rectangle(0, spriteManager.selectedFrameOffset, 150, 30),
                Color.White, 0, Vector2.Zero,
                1f, SpriteEffects.None, 1);

            // HealthBar
            spriteManager.selectedFrameOffset = GetFrameOffset(spriteManager.PlayerHealth);
                        
        }
        
        private int GetFrameOffset(int playerHealth)
        {
            return (5 - playerHealth) * 30;
        }
    }
}
                