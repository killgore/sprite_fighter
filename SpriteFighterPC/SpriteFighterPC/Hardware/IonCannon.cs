using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace SpriteFighter
{
    public class IonCannon : GunAttachment
    {
        public static Texture2D ION_CANNON_SPRITE;
        public static SoundEffect ION_CANNON_SOUND;

        static int ION_CANNON_RATE = 1000;
        static Vector2 ION_CANNON_SPEED = new Vector2(0, -20);
        static int ION_CANNON_DAMAGE = 100;
        static Color ION_CANNON_COLOR = Color.GreenYellow;
        static Vector2 ION_CANNON_OFFSET = new Vector2(50, 0);

        public IonCannon()
        {
        }

        public IonCannon(Vector2 offset, GameEntity attachedTo, long time)
            : base(ION_CANNON_RATE, ION_CANNON_SPEED, ION_CANNON_DAMAGE, offset, attachedTo, ION_CANNON_COLOR, time, GunAttachment.IonCannon)
        {
            _spriteImage = ION_CANNON_SPRITE;
            m_projectileSpriteImage = Projectile.PROJECTILE_SPRITE;
            m_soundEffect = ION_CANNON_SOUND;
        }

        protected override void checkGenerateProjectile(long ttms)
        {
            if (canFire(ttms))
            {
                generateProjectileSpeedAndOffset(ttms, ION_CANNON_SPEED, -ION_CANNON_OFFSET);
                generateProjectileSpeedAndOffset(ttms, ION_CANNON_SPEED, ION_CANNON_OFFSET);
                m_lastFireTime = ttms;
                PlaySoundEffect();
            }
        }

        public override void Update(long ttms)
        {
            base.Update(ttms);
        }
    }
}
