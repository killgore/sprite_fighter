using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace SpriteFighter
{
    public class Shield : Attachment
    {
        public static Texture2D SHIELD_SPRITE;
        public static SoundEffect SHIELD_SOUND;

        static Color SHIELD_COLOR = Color.Yellow;
        static Vector2 SHIELD_OFFSET = Vector2.Zero;

        private const long SHIELD_LIFETIME = 10000; // 10 seconds
        private const int STANDARD_DAMAGE = 50;
        
        private long m_shieldStartTime;
        private int m_shieldLife = 500;

        public Shield()
        {
        }

        public Shield(Vector2 offset, GameEntity attachedTo, long time)
            : base(offset, attachedTo, SHIELD_COLOR, time, Attachment.Shield )
        {
            _spriteImage = SHIELD_SPRITE;
            m_soundEffect = null;
            m_shieldStartTime = time;
        }

        public void TakeDamage(int dmg)
        {
            m_shieldLife -= dmg;
        }

        public void TakeDamage()
        {
            m_shieldLife -= STANDARD_DAMAGE;
        }

        public override void Render(SpriteBatch sb)
        {
            sb.Draw(m_spriteImage, m_position, null, m_color, m_rotation,
                 m_spriteOffset, 2.0f, SpriteEffects.None, 0);
            base.Render(sb);
        }

        public override void Update(long ttms)
        {
            /*
            if ((ttms - m_shieldStartTime) > SHIELD_LIFETIME)
                m_isActive = false;
            */
            if (m_shieldLife <= 0)
                DestroyMe();
                //m_isActive = false;

            base.Update(ttms);
        }
    }
}
