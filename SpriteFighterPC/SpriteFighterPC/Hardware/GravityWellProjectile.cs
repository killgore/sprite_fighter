using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpriteFighter
{
    public class GravityWellProjectile : Projectile
    {
        const int GRAVITY_WELL_LIFETIME = 10000;

        public GravityWellProjectile(Vector2 pos, Vector2 vel, Color c, int dmg, long time)
            : base(pos, vel, c, dmg, time, GRAVITY_WELL_LIFETIME)
        {
            m_canDestroy = false;
        }

    }
}
