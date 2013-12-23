using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace SmashGoBang.SpriteClasses
{
    public class SoundManager
    {
        private Song backgroundMusic;
        private SoundEffect laserEffect;
        private SoundEffect explosionEffect;
        private SoundEffect powerUpEffect;

        public SoundManager(ContentManager content)
        {
            backgroundMusic = content.Load<Song>(@"sounds\background");
            laserEffect = content.Load<SoundEffect>(@"sounds\shot");
            explosionEffect = content.Load<SoundEffect>(@"sounds\explosion");
            powerUpEffect = content.Load<SoundEffect>(@"sounds\reload");
        }

        public void playBackGroundMusic()
        {
            if (MediaPlayer.GameHasControl)
            {
                MediaPlayer.Play(backgroundMusic);
                MediaPlayer.IsRepeating = true;
            }
        }

        public void playShotSound()
        {
            laserEffect.Play();
        }

        public void playExplosionSound()
        {
            explosionEffect.Play();
        }

        public void playPowerUpSound()
        {
            powerUpEffect.Play();
        }
    }
}
