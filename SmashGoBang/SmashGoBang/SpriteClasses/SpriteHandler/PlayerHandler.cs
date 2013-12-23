using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SmashGoBang.SpriteClasses.SpriteHandler
{
    public class PlayerHandler
    {
        private int maxHealth = 5;

        public int PlayerIsHit(int PlayerHealth)
        {
            return PlayerHealth -= 1;
        }

        public int HealthRestore(int PlayerHealth)
        {
            if (PlayerHealth < maxHealth)
            {
                PlayerHealth += 1;
            }
            return PlayerHealth;
        }
                
        public UserControlledSprite LoadPlayer(Game game, SpriteManager spriteManager)
        {
            var ReturnPlayer = new UserControlledSprite(
                     game.Content.Load<Texture2D>(@"Sprites/ship"),
                     new Vector2(210, 250), new Point(90, 72), 10, new Point(0, 0),
                     new Point(1, 1), new Vector2(6, 6), spriteManager);

                return ReturnPlayer;
        }

        public UserControlledSprite LoadBlinkingShip(Game game, SpriteManager spriteManager)
        {
            var ReturnPlayer = new UserControlledSprite(
                     game.Content.Load<Texture2D>(@"Sprites/shipHit"),
                     new Vector2(210, 250), new Point(90, 72), 10, new Point(0, 0),
                     new Point(4, 4), new Vector2(6, 6), spriteManager);

            return ReturnPlayer;
        }

        public UserControlledSprite ActivateShield(Game game, SpriteManager spriteManager)
        {
            var ReturnPlayer = new UserControlledSprite(
                     game.Content.Load<Texture2D>(@"Sprites/ship88"),
                     new Vector2(210, 250), new Point(90, 72), 10, new Point(0, 0),
                     new Point(4, 4), new Vector2(6, 6), spriteManager);

            return ReturnPlayer;
        }
        
        public StaticSprite LoadHealthBar(Game game, SpriteManager spriteManager)
        {
            var ReturnHealthBar = new StaticSprite(
                        game.Content.Load<Texture2D>(@"Sprites/H_Bar"),
                        new Point(150, 180), new Rectangle(),
                        new Vector2(390, 550), spriteManager);

            return ReturnHealthBar;
        }
    }
}
