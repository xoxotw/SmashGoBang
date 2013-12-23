using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SmashGoBang.SpriteClasses.SpriteHandler;
using SmashGoBang.SpriteClasses;

namespace SmashGoBang.SpriteClasses
{
    public class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        UserControlledSprite player;
        UserControlledSprite playerShield;
        UserControlledSprite playerGotHit;
        
        List<Explosion> explosions = new List<Explosion>();
        SoundManager soundManager;

        PlayerHandler playerHandler = new PlayerHandler();
        EnemyHandler enemyHandler = new EnemyHandler();
        WeaponHandler weaponHandler = new WeaponHandler();
        PowerUpHandler powerUpHandler = new PowerUpHandler();
        AtomBombHandler atomBombHandler = new AtomBombHandler();
        bool isImmortal = false;

        DateTime startTime, immortalStartTime;

        private bool shieldPowerUp = false;
        private bool playerIsHit = false;
        
        public int PlayerHealth = 3;
        public int selectedFrameOffset = 60;

        public SpriteManager(Game game)
            : base(game)
        {

        }

        public bool ResetTrue2False(DateTime startTime, int duration)
        {
            var stopTime = (DateTime.Now - startTime).TotalSeconds;
            if (stopTime >= duration)
            {
                return false;
            }
            return true;
        }

        public override void Initialize()
        {
            enemyHandler.ResetSpawnTime(Game);

            base.Initialize();
        }

        //*
        public void ResetGame()
        {
            enemyHandler.enemyMinSpeed = 1;
            enemyHandler.enemyMaxSpeed = 4;

            powerUpHandler.powerUpMinSpeed = 1;
            powerUpHandler.powerUpMaxSpeed = 4;

            enemyHandler.enemySpawnMinMilliSeconds = 200;
            enemyHandler.enemySpawnMaxMilliSeconds = 800;

            enemyHandler.enemyList.Clear();

            powerUpHandler.powerUpList.Clear();
            atomBombHandler.atomBombList.Clear();
            weaponHandler.BulletList.Clear();
            weaponHandler.ammo = 0;
            weaponHandler.currentWeapon = weaponType.regular;
            explosions.Clear();
            PlayerHealth = 3;
            selectedFrameOffset = 60;

            isImmortal = false;
            shieldPowerUp = false;
            playerIsHit = false;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            soundManager = new SoundManager(Game.Content);

            soundManager.playBackGroundMusic();

            player = playerHandler.LoadPlayer(Game, this);
            playerGotHit = playerHandler.LoadBlinkingShip(Game, this);
            playerShield = playerHandler.ActivateShield(Game, this);
            
            base.LoadContent();
        }

        public Vector2 GetPlayerPosition()
        {
            return player.GetPosition;
        }

        //*
        public int GetPlayerHealth()
        {
            return PlayerHealth;
        }

        //*
        public int GetPlayerAmmo()
        {
            return weaponHandler.ammo;
        }

        public void Shoot()
        {
            weaponHandler.SpawnBullet(Game, player);
        }

        void PlayerIsHit(GameTime gameTime, SpriteBatch spriteBatch)
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Start();

            playerGotHit.Draw(gameTime, spriteBatch);
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var timer = (System.Timers.Timer)sender;
            timer.Stop();

            playerIsHit = false;
        }
        
        public void SpawnExplosion(Vector2 explosionPosition)
        {
            explosions.Add(
                new Explosion(Game.Content.Load<Texture2D>("Photos/explosion_v2"),
                    explosionPosition, new Point(100, 100), 10, new Point(0, 0),
                    new Point(3, 3), new Vector2(0, 0), 40, 10));
        }


        public override void Update(GameTime gameTime)
        {
            enemyHandler.nextSpawnTime -= gameTime.ElapsedGameTime.Milliseconds;
            powerUpHandler.nextSpawnTime -= gameTime.ElapsedGameTime.Milliseconds;
            atomBombHandler.nextSpawnTime -= gameTime.ElapsedGameTime.Milliseconds;

            enemyHandler.enemyList = enemyHandler.SpawnEnemy(Game, this);
            powerUpHandler.powerUpList = powerUpHandler.SpawnPowerUp(Game, this);
            atomBombHandler.atomBombList = atomBombHandler.SpawnAtomBomb(Game, this);

            enemyHandler.AdjustSpawnTimes(gameTime);
            powerUpHandler.AdjustSpawnTimes(gameTime);

            UpdateSprites(gameTime);

            base.Update(gameTime);
        }

        protected void UpdateSprites(GameTime gameTime)
        {
            // UPDATE ReturnPlayer
            player.Update(gameTime, Game.Window.ClientBounds);
            playerGotHit.Update(gameTime, Game.Window.ClientBounds);
            playerShield.Update(gameTime, Game.Window.ClientBounds);
            
            //Reset shieldPowerUp to false
            if (shieldPowerUp == true)
            {
                shieldPowerUp = ResetTrue2False(startTime, 4);
            }

            if (isImmortal == true)
            {
                isImmortal = ResetTrue2False(immortalStartTime, 1);
            }

            // UPDATE sprites
            for (int i = 0; i < enemyHandler.enemyList.Count; ++i)
            {
                Sprite s = enemyHandler.enemyList[i];
                s.Update(gameTime, Game.Window.ClientBounds);

                // Vid kollision forsvinner Objektet EXEMPEL
                if (s.collisionRect.Intersects(player.collisionRect))
                {
                    if (shieldPowerUp == false)
                    {
                        playerIsHit = true;
                        if (!isImmortal)
                        {
                        soundManager.playExplosionSound();
                        SpawnExplosion(enemyHandler.enemyList[i].GetPosition);
                        enemyHandler.enemyList.RemoveAt(i);
                        --i;
                        
                        immortalStartTime = DateTime.Now;
                        
                            PlayerHealth = playerHandler.PlayerIsHit(PlayerHealth);
                            isImmortal = true;
                        }
                    }
                    else
                    {
                        soundManager.playExplosionSound();
                        SpawnExplosion(enemyHandler.enemyList[i].GetPosition);
                        ((Game1)Game).AddScore(enemyHandler.enemyList[i].scoreValue);
                        enemyHandler.enemyList.RemoveAt(i);
                        --i;
                    }

                }


                // REMOVES Objects that are out of bounds
                if (s.IsOutOfBounds(Game.Window.ClientBounds))
                {
                    enemyHandler.enemyList.RemoveAt(i);
                    --i;
                }
            }

            //Update powerUps
            for (int i = 0; i < powerUpHandler.powerUpList.Count; ++i)
            {
                PowerUpSprite s = powerUpHandler.powerUpList[i];
                s.Update(gameTime, Game.Window.ClientBounds);

                // Vid kollision forsvinner Objektet EXEMPEL
                if (s.collisionRect.Intersects(player.collisionRect))
                {
                    if (s.powerUpType == PowerUptype.Laser)
                    {
                        soundManager.playPowerUpSound();
                        weaponHandler.ammo = 200;
                        weaponHandler.currentWeapon = weaponType.laser;
                        powerUpHandler.powerUpList.RemoveAt(i);
                        --i;
                    }

                    if (s.powerUpType == PowerUptype.Spread)
                    {
                        soundManager.playPowerUpSound();
                        weaponHandler.ammo = 20;
                        weaponHandler.currentWeapon = weaponType.spread;
                        powerUpHandler.powerUpList.RemoveAt(i);
                        i--;
                    }

                    if (s.powerUpType == PowerUptype.Health)
                    {
                        soundManager.playPowerUpSound();
                        PlayerHealth = playerHandler.HealthRestore(PlayerHealth);
                        powerUpHandler.powerUpList.RemoveAt(i);
                        --i;
                    }

                    if (s.powerUpType == PowerUptype.Armour)
                    {
                        soundManager.playPowerUpSound();
                        shieldPowerUp = true;
                        startTime = DateTime.Now;
                        powerUpHandler.powerUpList.RemoveAt(i);
                        --i;                       
                    }
                }
                // REMOVES Objects that are out of bounds
                if (s.IsOutOfBounds(Game.Window.ClientBounds))
                {
                    powerUpHandler.powerUpList.RemoveAt(i);
                    --i;
                }
            }

            //Update atombomb
            for (int i = 0; i < atomBombHandler.atomBombList.Count; ++i)
            {
                Sprite s = atomBombHandler.atomBombList[i];
                s.Update(gameTime, Game.Window.ClientBounds);

                // Vid kollision forsvinner Objektet EXEMPEL
                if (s.collisionRect.Intersects(player.collisionRect))
                {
                    //soundManager.playPowerUpSound();
                    soundManager.playExplosionSound();
                    for (int e = 0; e < enemyHandler.enemyList.Count; e++)
                    {
                        SpawnExplosion(enemyHandler.enemyList[e].GetPosition);
                        ((Game1)Game).AddScore(enemyHandler.enemyList[e].scoreValue);
                    }
                    enemyHandler.enemyList.Clear();
                    atomBombHandler.atomBombList.RemoveAt(i);
                    --i;
                }

                // REMOVES Objects that are out of bounds
                if (s.IsOutOfBounds(Game.Window.ClientBounds))
                {
                    atomBombHandler.atomBombList.RemoveAt(i);
                    --i;
                }
            }
            // Update Bullet Delay
            weaponHandler.bulletdelay -= gameTime.ElapsedGameTime.Milliseconds;

            //UpdateBullet
            for (int i = 0; i < weaponHandler.BulletList.Count; ++i)
            {
                //soundManager.playShotSound();
                BulletSprite b = weaponHandler.BulletList[i];
                b.Update(gameTime, Game.Window.ClientBounds);

                // Kollison Med fiender
                for (int z = 0; z < enemyHandler.enemyList.Count; ++z)
                {
                    Sprite s = enemyHandler.enemyList[z];

                    // Vid kollision forsvinner Objektet och Bullet
                    if (s.collisionRect.Intersects(b.collisionRect))
                    {
                        ((Game1)Game).AddScore(enemyHandler.enemyList[z].scoreValue);
                        soundManager.playExplosionSound();
                        SpawnExplosion(enemyHandler.enemyList[z].GetPosition);
                        enemyHandler.enemyList.RemoveAt(z);
                        --z;

                        weaponHandler.BulletList.Remove(b); //Lösnig a, verkar tveksam?

                        //game.Exit();
                    }
                }

                // REMOVES Objects that are out of bounds
                if (b.IsOutOfBounds(Game.Window.ClientBounds))
                {
                    if (i < weaponHandler.BulletList.Count) //Lösning b
                    {
                        weaponHandler.BulletList.RemoveAt(i);
                    }
                    --i;
                }
            }

            //Update Explosion
            for (int i = 0; i < explosions.Count; i++)
            {
                explosions[i].Update(gameTime, Game.Window.ClientBounds);
                if (explosions[i].IsDone())
                {
                    explosions.Remove(explosions[i]);
                }
            }

            base.Update(gameTime);

        }

        //*
        public void DrawSprites(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            if (playerIsHit == true) 
                // method Draws a blinking ship for a second
                PlayerIsHit(gameTime, spriteBatch); 
            else if (shieldPowerUp == false) 
                // Draws the Original ship
                player.Draw(gameTime, spriteBatch); 
                           
            else if (shieldPowerUp == true)                
                // Draws a ship with a Shield for 5 seconds
                playerShield.Draw(gameTime, spriteBatch);                

            foreach (Sprite s in enemyHandler.enemyList)
            {
                s.Draw(gameTime, spriteBatch);
            }
            //Draw powerUp
            foreach (Sprite s in powerUpHandler.powerUpList)
            {
                s.Draw(gameTime, spriteBatch);
            }
            //Draw atomBomb
            foreach (Sprite s in atomBombHandler.atomBombList)
            {
                s.Draw(gameTime, spriteBatch);
            }
            //Draw Bullets
            foreach (BulletSprite b in weaponHandler.BulletList)
            {
                b.Draw(gameTime, spriteBatch);
            }

            //Draw Explosion
            foreach (Explosion explsn in explosions)
            {
                explsn.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
