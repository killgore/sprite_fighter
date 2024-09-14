using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpriteFighter
{
    public class StasisRayProjectile : Projectile
    {
        public static Texture2D STASIS_RAY_SPRITE;

        public StasisRayProjectile(Vector2 pos, Vector2 vel, Color c, int dmg, long time, int lifetime)
            : base(pos, vel, c, dmg, time, lifetime)
        {
            _spriteImage = STASIS_RAY_SPRITE;
        }

        public override void Affect(SpaceShip es)
        {
            es._velocity = Vector2.Zero;
            base.Affect(es);
        }
    }
}
