using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpriteFighter
{
    public class PowerUp : GameEntity
    {
        public const uint BaseCannon = 1;
        public const uint SpreadCannon = 2;
        public const uint IonCannon = 4;
        public const uint GravityWell = 8;
        public const uint Shield = 16;
        public const uint StasisRay = 32;
        public const uint HealthPack = 64;

        private uint m_lifetime = 1000;

        public PowerUp()
        {
        }

        public PowerUp(float x, float y, long time, uint lifetime)
        {
            m_position = new Vector2(x, y);
            m_rotation = 0;
            m_birthtime = time;
            m_lifetime = lifetime;
        }

        public uint GetLifeTime()
        {
            return m_lifetime;
        }

        public virtual void PerformAction(PlayerShip playerShip, long ttms)
        {
            m_destroyMe = true;
        }

        public bool AmDead(double ttms)
        {
            return base.AmDead() || ((ttms - m_birthtime) >= m_lifetime);
        }

        public override void Render(SpriteBatch sb)
        {
            sb.Draw(m_spriteImage, m_position, null, m_color, m_rotation,
                 m_spriteOffset, 1.0f, SpriteEffects.None, 0);

            base.Render(sb);
        }
    }
}
