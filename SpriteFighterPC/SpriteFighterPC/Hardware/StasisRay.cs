using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace SpriteFighter
{
    public class StasisRay : GunAttachment
    {
        public static SoundEffect STASIS_RAY_SOUND;

        static int STASIS_RAY_RATE = 500;
        static Vector2 STASIS_RAY_SPEED_RIGHT = new Vector2(3, -10);
        static Vector2 STASIS_RAY_SPEED_LEFT = new Vector2(-3, -10);
        static int STASIS_RAY_DAMAGE = 0;
        static Color STASIS_RAY_COLOR = Color.SteelBlue;
        static Vector2 STASIS_RAY_SCALE = new Vector2(1, 10);

        public StasisRay()
        {
        }

        public StasisRay(Vector2 offset, GameEntity attachedTo, long time)
            : base(STASIS_RAY_RATE, STASIS_RAY_SPEED_RIGHT, STASIS_RAY_DAMAGE, offset, attachedTo, STASIS_RAY_COLOR, time, GunAttachment.StasisRay)
        {
            _spriteImage = StasisRayProjectile.STASIS_RAY_SPRITE;
            m_projectileSpriteImage = StasisRayProjectile.STASIS_RAY_SPRITE;
            m_soundEffect = STASIS_RAY_SOUND;
        }

         protected override void checkGenerateProjectile(long ttms)
        {
            if (canFire(ttms))
            {
                float x = m_parent._position.X + m_offset.X;
                float y = m_parent._position.Y - PlayerShip.PLAYER_SPRITE.Height / 2;
                Vector2 startPosition = new Vector2(x, y);
                StasisRayProjectile sp = new StasisRayProjectile(startPosition, STASIS_RAY_SPEED_RIGHT, m_color, m_projectileDamage, ttms, PROJECTILETIME);
                addProjectile(sp);
                sp = new StasisRayProjectile(startPosition, STASIS_RAY_SPEED_LEFT, m_color, m_projectileDamage, ttms, PROJECTILETIME);
                addProjectile(sp);
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
