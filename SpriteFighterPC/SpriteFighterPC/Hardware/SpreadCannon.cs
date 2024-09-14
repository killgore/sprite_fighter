using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace SpriteFighter
{
    public class SpreadCannon : GunAttachment
    {
        public static Texture2D SPREAD_CANNON_SPRITE;
        public static SoundEffect SPREAD_CANNON_SOUND;

        static int SPREAD_CANNON_RATE = 500;
        static Vector2 SPREAD_CANNON_SPEED = new Vector2(0, -40);
        static Vector2 SPREAD_CANNON_SPEED_LEFT = new Vector2(-3, -40);
        static Vector2 SPREAD_CANNON_SPEED_RIGHT = new Vector2(3, -40);
        static int SPREAD_CANNON_DAMAGE = 50;
        static Color SPREAD_CANNON_COLOR = Color.PaleVioletRed;

        static int SPREAD_CANNON_OFFSET = 60;

        public SpreadCannon()
        {
        }

        public SpreadCannon(Vector2 offset, GameEntity attachedTo, long time)
            : base(SPREAD_CANNON_RATE, SPREAD_CANNON_SPEED, SPREAD_CANNON_DAMAGE, offset, attachedTo, SPREAD_CANNON_COLOR, time, GunAttachment.SpreadCannon)
        {
            _spriteImage = SPREAD_CANNON_SPRITE;
            m_projectileSpriteImage = Projectile.PROJECTILE_SPRITE;
            m_soundEffect = SPREAD_CANNON_SOUND;
        }

        protected override void checkGenerateProjectile(long ttms)
        {
            if (canFire(ttms))
            {
                generateProjectileSpeedAdjust(ttms, SPREAD_CANNON_SPEED);
                generateProjectileSpeedAndOffset(ttms, SPREAD_CANNON_SPEED_LEFT, new Vector2(-SPREAD_CANNON_OFFSET, 0));
                generateProjectileSpeedAndOffset(ttms, SPREAD_CANNON_SPEED_RIGHT, new Vector2(SPREAD_CANNON_OFFSET, 0));
                m_lastFireTime = ttms;
                PlaySoundEffect();
            }
        }

        public override void Update(long ttms)
        {
            updateProjectiles(ttms);
            checkGenerateProjectile(ttms);
        }
    }
}
