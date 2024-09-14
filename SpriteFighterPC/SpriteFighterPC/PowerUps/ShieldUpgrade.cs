using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpriteFighter
{
    public class ShieldUpgrade : PowerUp
    {
        public static Texture2D SHIELD_UPGRADE_SPRITE;
        public static int SHIELD_COST = 100;

        private const int SHIELD_UPGRADE_LIFETIME = 5000;
        Vector2 SHIELD_UPGRADE_VELOCITY = new Vector2(0, 7);

        public ShieldUpgrade()
            : base(0, 0, 0, SHIELD_UPGRADE_LIFETIME)
        {
        }

        public ShieldUpgrade(float x, float y, long time)
            : base(x, y, time, SHIELD_UPGRADE_LIFETIME)
        {
            m_velocity = SHIELD_UPGRADE_VELOCITY;
            m_color = Color.PowderBlue;
            _spriteImage = SHIELD_UPGRADE_SPRITE;
        }

        public override void PerformAction(PlayerShip playerShip, long ttms)
        {
            playerShip.AddAttachment(new Shield(Vector2.Zero, playerShip, ttms));
            base.PerformAction(playerShip, ttms);
        }
    }
}
