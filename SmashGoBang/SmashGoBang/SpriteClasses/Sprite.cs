using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SmashGoBang.SpriteClasses
{
    public abstract class Sprite
    {
        public Texture2D textureImage;
        protected Point frameSize;
        public Point currentFrame;
        Point sheetSize;
        int collisionOffset;
        int timeSinceLastFrame = 0;
        int milliSecondsPerFrame;
        const int defaultMilliSecondsPerFrame = 16;
        protected Vector2 speed;
        protected Vector2 position;
        protected bool animationPlayedOnce;
        private Vector2 position_2;
        private Point frameSize_2;
        private Vector2 speed_2;
        private int p;
        private SoundManager soundManager;
        private int millisecondsPerFrame;

        public abstract Vector2 direction { get; }

        public Vector2 GetPosition
        {
            get { return position; }
        }

        public int scoreValue { get; protected set; }
        
        public Rectangle collisionRect
        {
            get
            {
                return new Rectangle(
                    (int)position.X + collisionOffset,
                    (int)position.Y + collisionOffset,
                    frameSize.X - (collisionOffset * 2),
                    frameSize.Y - (collisionOffset * 2));
            }
        }


        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed,
            int scoreValue)
            : this(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, defaultMilliSecondsPerFrame, scoreValue)
        {
        }

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed,
            int milliSecondsPerFrame, int scoreValue)
        {
            this.textureImage = textureImage;
            this.position = position;
            this.frameSize = frameSize;
            this.collisionOffset = collisionOffset;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.speed = speed;
            this.milliSecondsPerFrame = milliSecondsPerFrame;
            this.scoreValue = scoreValue;
        }

        public Sprite(Texture2D textureImage, Vector2 position_2, Point frameSize_2, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed_2, int p, SoundManager soundManager)
        {
            // TODO: Complete member initialization
            this.textureImage = textureImage;
            this.position_2 = position_2;
            this.frameSize_2 = frameSize_2;
            this.collisionOffset = collisionOffset;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.speed_2 = speed_2;
            this.p = p;
            this.soundManager = soundManager;
        }

        public Sprite(Texture2D textureImage, Vector2 position_2, Point frameSize_2, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed_2, int millisecondsPerFrame, int p, SoundManager soundManager)
        {
            // TODO: Complete member initialization
            this.textureImage = textureImage;
            this.position_2 = position_2;
            this.frameSize_2 = frameSize_2;
            this.collisionOffset = collisionOffset;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.speed_2 = speed_2;
            this.millisecondsPerFrame = millisecondsPerFrame;
            this.p = p;
            this.soundManager = soundManager;
        }

        public bool IsOutOfBounds(Rectangle clientRect)
        {
            if (position.X < -frameSize.X ||
               position.X > clientRect.Width ||
               position.Y < -frameSize.Y ||
               position.Y > clientRect.Height)
            {
                return true;
            }
            return false;
        }

        public virtual void Update(GameTime gameTime, Rectangle clientBounds)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > milliSecondsPerFrame)
            {
                timeSinceLastFrame = 0;
                ++currentFrame.X;
                if (currentFrame.X >= sheetSize.X)
                {
                    currentFrame.X = 0;
                    ++currentFrame.Y;
                    if (currentFrame.Y >= sheetSize.Y)
                    {
                        currentFrame.Y = 0;
                        animationPlayedOnce = true;
                    }

                }              
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureImage,
                position,
                new Rectangle(currentFrame.X * frameSize.X,
                    currentFrame.Y * frameSize.Y,
                    frameSize.X, frameSize.Y),
                Color.White, 0, Vector2.Zero,
                1f, SpriteEffects.None, 0);
        }
    }
}
