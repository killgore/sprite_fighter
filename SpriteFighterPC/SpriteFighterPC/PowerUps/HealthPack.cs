using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
 
namespace SpriteFighter
{
    public class HealthPack : PowerUp
    {
        public static Texture2D HEALTH_PACK_SPRITE;
        public const int HEALTH_PACK_COST = 100;

        private const int HEALTHPACK_LIFETIME = 5000;
        Vector2 HEALTHPACK_VELOCITY = new Vector2(0, 7);

        private int m_heathAdd;

        public HealthPack()
        {
            m_heathAdd = 100;
        }

        public HealthPack(float x, float y, int health, long time)
            : base(x, y, time, HEALTHPACK_LIFETIME)
        {
            m_heathAdd = health;
            m_velocity = HEALTHPACK_VELOCITY;
            m_color = Color.WhiteSmoke;
            m_spriteImage = HEALTH_PACK_SPRITE;
            m_spriteOffset = new Vector2(m_spriteImage.Width * 0.5f, m_spriteImage.Height * 0.5f);
        }

        public void SetHealthPackAmount(int healthAdd)
        {
            m_heathAdd = healthAdd;
        }

        public override void PerformAction(PlayerShip playerShip, long ttms)
        {
            playerShip._life = playerShip._life + m_heathAdd;
            base.PerformAction(playerShip, ttms);
        }

    }
}
