using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpriteFighter
{
    public class StasisRayUpgrade : PowerUp
    {
        public static Texture2D STASIS_RAY_UPGRADE_SPRITE;
        public static int STASIS_RAY_UPGRADE_COST = 100;

        private const int STASIS_RAY_UPGRADE_LIFETIME = 5000;
        Vector2 STASIS_RAY_UPGRADE_VELOCITY = new Vector2(0, 7);

        public StasisRayUpgrade()
        {
        }

        public StasisRayUpgrade(float x, float y, long time)
            : base(x, y, time, STASIS_RAY_UPGRADE_LIFETIME)
        {
            m_velocity = STASIS_RAY_UPGRADE_VELOCITY;
            m_color = Color.SteelBlue;
            _spriteImage = STASIS_RAY_UPGRADE_SPRITE;
        }

        public override void PerformAction(PlayerShip playerShip, long ttms)
        {
            playerShip.AddAttachment(new StasisRay(Vector2.Zero, playerShip, ttms));       
            base.PerformAction(playerShip, ttms);
        }
    }
}
