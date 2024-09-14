using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpriteFighter
{
    public class StarFieldProjectile : Projectile
    {
        public static Texture2D STAR_FIELD_SPRITE;

        const int STAR_LIFETIME = 10000;

        public StarFieldProjectile(Vector2 pos, Vector2 vel, Color c, int dmg, long time)
            : base(pos, vel, c, dmg, time, STAR_LIFETIME)
        {
            m_canDestroy = false;
        }
    }
}
