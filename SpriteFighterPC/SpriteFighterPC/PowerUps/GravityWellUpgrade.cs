using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpriteFighter
{
    class GravityWellUpgrade : PowerUp
    {
        public static Texture2D GRAVITY_WELL_UPGRADE_SPRITE;
        public const int GRAVITY_WELL_UPGRADE_COST = 100;

        private const int GRAVITY_WELL_UPGRADE_LIFETIME = 5000;
        Vector2 GRAVITY_WELL_UPGRADE_VELOCITY = new Vector2(0, 7);
        List<EnemyShip> m_victims;

        public GravityWellUpgrade()
        {
        }

        public GravityWellUpgrade(float x, float y, List<EnemyShip> victims, long time)
            : base(x, y, time, GRAVITY_WELL_UPGRADE_LIFETIME)
        {
            m_velocity = GRAVITY_WELL_UPGRADE_VELOCITY;
            m_color = Color.Orange;
            _spriteImage = GRAVITY_WELL_UPGRADE_SPRITE;
            m_victims = victims;
        }

        public override void PerformAction(PlayerShip playerShip, long ttms)
        {
            playerShip.AddAttachment(new GravityWell(Vector2.Zero, playerShip, m_victims, ttms));       
            base.PerformAction(playerShip, ttms);
        }
    }
}
