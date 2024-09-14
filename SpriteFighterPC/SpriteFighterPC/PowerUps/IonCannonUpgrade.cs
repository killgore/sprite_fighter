using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpriteFighter
{
    public class IonCannonUpgrade : PowerUp
    {
        public static Texture2D ION_CANNON_UPGRADE_SPRITE;
        public static int ION_CANNON_UPGRADE_COST = 100;

        private const int ION_CANNON_UPGRADE_LIFETIME = 5000;
        Vector2 ION_CANNON_UPGRADE_VELOCITY = new Vector2(0, 7);

        public IonCannonUpgrade()
        {
        }

        public IonCannonUpgrade(float x, float y, long time)
            : base(x, y, time, ION_CANNON_UPGRADE_LIFETIME)
        {
            m_velocity = ION_CANNON_UPGRADE_VELOCITY;
            m_color = Color.GreenYellow;
            _spriteImage = ION_CANNON_UPGRADE_SPRITE;
        }

        public override void PerformAction(PlayerShip playerShip, long ttms)
        {
            playerShip.AddAttachment(new IonCannon(Vector2.Zero, playerShip, ttms));      
            base.PerformAction(playerShip, ttms);
        }
    }
}
