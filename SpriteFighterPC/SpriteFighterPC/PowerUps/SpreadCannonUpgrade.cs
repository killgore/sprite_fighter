using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpriteFighter
{
    public class SpreadCannonUpgrade : PowerUp
    {
        public static Texture2D SPREAD_CANNON_UPGRADE_SPRITE;
        public static int SPREAD_CANNON_UPGRADE_COST = 100;

        private const int SPREAD_CANNON_UPGRADE_LIFETIME = 5000;
        Vector2 SPREAD_CANNON_UPGRADE_VELOCITY = new Vector2(0, 7);

        public SpreadCannonUpgrade()
        {
        }

        public SpreadCannonUpgrade(float x, float y, long time)
            : base(x, y, time, SPREAD_CANNON_UPGRADE_LIFETIME)
        {
            m_velocity = SPREAD_CANNON_UPGRADE_VELOCITY;
            m_color = Color.PaleVioletRed;
            _spriteImage = SPREAD_CANNON_UPGRADE_SPRITE;
        }

        public override void PerformAction(PlayerShip playerShip, long ttms)
        {
            playerShip.AddAttachment(new SpreadCannon(Vector2.Zero, playerShip, ttms));       
            base.PerformAction(playerShip, ttms);
        }
    }
}
