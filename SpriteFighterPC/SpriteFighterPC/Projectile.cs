using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpriteFighter
{
    public class Projectile : GameEntity
    {
        public static Texture2D PROJECTILE_SPRITE;

        public const int INF_LIFETIME = -1;

        private int m_damage;
        private int m_lifetime;

        public Projectile(Vector2 pos, Vector2 vel, Color c, int dmg, long time, int lifetime)
            : base(pos, vel, 0.0f, c, time)
        {
            m_damage = dmg;
            m_lifetime = lifetime;
            _spriteImage = PROJECTILE_SPRITE;
        }

        public Projectile(Vector2 pos, Vector2 vel, Color c, int dmg, long time, int lifetime, Texture2D projImage)
            : base(pos, vel, 0.0f, c, time)
        {
            m_damage = dmg;
            m_lifetime = lifetime;
            _spriteImage = projImage;
        }

        public int GetDamage()
        {
            return m_damage;
        }

        public bool AmDead(double ttms)
        {
            if (m_lifetime == INF_LIFETIME)
                return false;

            return ((ttms - m_birthtime) > m_lifetime) || base.AmDead();
        }

        public virtual void Affect()
        {
        }

        public virtual void Affect(SpaceShip es)
        {
        }

        public override void Render(SpriteBatch sb)
        {
            sb.Draw(m_spriteImage, m_position, null, m_color, m_rotation,
                 m_spriteOffset, 1.0f, SpriteEffects.None, 0);
        }
    }
}
