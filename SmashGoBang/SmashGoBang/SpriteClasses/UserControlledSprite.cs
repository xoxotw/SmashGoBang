using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace SmashGoBang.SpriteClasses
{
    public class UserControlledSprite : Sprite
    {
        // TO_USE_MOUSE_CONTROL MouseState prevMouseState;

        public SpriteManager spriteManager;
        public SoundManager soundManager;
        
        public UserControlledSprite(Texture2D textureImage, Vector2 position,
            Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize,
            Vector2 speed, SpriteManager sManager)//La till sprite manager
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, 0)
        {
            spriteManager = sManager;
        }

        public UserControlledSprite(Texture2D textureImage, Vector2 position,
            Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize,
            Vector2 speed, int millisecondsPerFrame, SoundManager soundManager)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, millisecondsPerFrame, 0)
        {
        }

        // game Controlls
        public override Vector2 direction
        {
            get
            {
                Vector2 inputDirection = Vector2.Zero;

                // Keyboard Controlls

                // Shoot Bullets
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    spriteManager.Shoot();               
                }

                //UpdateBullets();

                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    inputDirection.X -= 1;
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    inputDirection.X += 1;
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    inputDirection.Y -= 1;
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    inputDirection.Y += 1;

                var boost = 1;

                // GamePad Controlls

                GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
                if (gamepadState.ThumbSticks.Left.X != 0)
                    inputDirection.X += gamepadState.ThumbSticks.Left.X;
                if (gamepadState.ThumbSticks.Left.Y != 0)
                    inputDirection.Y -= gamepadState.ThumbSticks.Left.Y;

                if (gamepadState.Buttons.X == ButtonState.Pressed)
                {
                    spriteManager.Shoot();
                }

                if (gamepadState.Buttons.A == ButtonState.Pressed)
                {
                    GamePad.SetVibration(PlayerIndex.One, 0.4f, 0.4f);
                    boost = 2;   // Speed Boost to be used
                }
                else
                {
                    GamePad.SetVibration(PlayerIndex.One, 0f, 0f);
                }

                return inputDirection * speed * boost;
            }
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            position += direction;

            // MOUSE CONTROL

            //MouseState currMouseState = Mouse.GetState();
            //if (currMouseState.X != prevMouseState.X ||
            //    currMouseState.Y != prevMouseState.Y)
            //{
            //    position = new Vector2(currMouseState.X, currMouseState.Y);
            //}
            //prevMouseState = currMouseState;

            // If sprite is off the screen, move it back within the game window
            if (position.X < 0)
                position.X = 0;
            if (position.Y < 0)
                position.Y = 0;
            if (position.X > clientBounds.Width - frameSize.X)
                position.X = clientBounds.Width - frameSize.X;
            if (position.Y > clientBounds.Height - 80 - frameSize.Y)
                position.Y = clientBounds.Height - 80 - frameSize.Y;

            base.Update(gameTime, clientBounds);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            {
                spriteBatch.Draw(textureImage,
                    position,
                    new Rectangle(currentFrame.X * frameSize.X,
                        currentFrame.Y * frameSize.Y,
                        frameSize.X, frameSize.Y),
                    Color.White, 0, Vector2.Zero,
                    1f, SpriteEffects.None, 1);
            }
            
        }
    }
}
