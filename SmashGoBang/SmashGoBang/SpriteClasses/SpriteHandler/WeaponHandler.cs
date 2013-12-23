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
    public enum weaponType {regular, rapid, laser, spread };

    public class WeaponHandler
    {
        public List<BulletSprite> BulletList;
        public int bulletdelay = 0;
        public int maxBullets;
        public weaponType currentWeapon;    //Start weapon is determind in SpriteManager.ReasetGame();
        public int ammo = 10;

        public WeaponHandler()
        {
            BulletList = new List<BulletSprite>();
        }

        public List<BulletSprite> SpawnBullet(Game game, UserControlledSprite player)
        {
            if (ammo <= 0) { currentWeapon = weaponType.regular; };

            //Weapon Reagular
            if (currentWeapon == weaponType.regular)
            {
                maxBullets = 2;

                if (BulletList.Count < maxBullets && bulletdelay <= 0)
                {
                    BulletList.Add(
                        new BulletSprite(game.Content.Load<Texture2D>(@"Sprites\Bullet\Reg"),
                            new Vector2(player.GetPosition.X + 40, player.GetPosition.Y + 25 ), new Point(25, 25), 0, new Point(0, 0),
                            new Point(1, 1), new Vector2(20, 0), 0));

                    bulletdelay = 300;
                }
            }

            //Weapon Laser
            if (currentWeapon == weaponType.laser)
            {
                maxBullets = 1000;

                if (BulletList.Count < maxBullets && bulletdelay <= 0)
                {
                    BulletList.Add(
                        new BulletSprite(game.Content.Load<Texture2D>(@"Sprites\Bullet\Las"),
                            new Vector2(player.GetPosition.X + 40, player.GetPosition.Y + 35), new Point(20, 3), 0, new Point(0, 0),
                            new Point(1, 1), new Vector2(20, 0), 0));

                    ammo -= 1;

                    bulletdelay = 5;
                }
            }

            if(currentWeapon == weaponType.spread)
            {
                maxBullets = 9;

                if (BulletList.Count < maxBullets && bulletdelay <= 0)
                {
                    BulletList.Add(
                        new BulletSprite(game.Content.Load<Texture2D>(@"Sprites\Bullet\Spr"),
                            new Vector2(player.GetPosition.X + 20, player.GetPosition.Y + 35 ), new Point(25, 25), 0, new Point(0, 0),
                            new Point(1, 1), new Vector2(20, 0), 0));

                    BulletList.Add(
                        new BulletSprite(game.Content.Load<Texture2D>(@"Sprites\Bullet\Spr"),
                            new Vector2(player.GetPosition.X + 20, player.GetPosition.Y + 35), new Point(25, 25), 0, new Point(0, 0),
                            new Point(1, 1), new Vector2(20, -3), 0));

                    BulletList.Add(
                        new BulletSprite(game.Content.Load<Texture2D>(@"Sprites\Bullet\Spr"),
                            new Vector2(player.GetPosition.X + 20, player.GetPosition.Y + 35), new Point(25, 25), 0, new Point(0, 0),
                            new Point(1, 1), new Vector2(20, 3), 0));

                        ammo -= 1;

                        bulletdelay = 200;
                }
            }

            return BulletList;
        }
    }
}
