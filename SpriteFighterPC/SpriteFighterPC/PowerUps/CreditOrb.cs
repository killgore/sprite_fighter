using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpriteFighter
{
    public class CreditOrb : PowerUp
    {
        public static Texture2D CREDIT_ORB_SPRITE;
        
        private const int CREDIT_VALUE = 10;
        private const int CREDITORB_LIFETIME = 5000;
        Vector2 CREDITORB_VELOCITY = new Vector2(0, 7);

        private int m_creditAdd;

        public CreditOrb()
        {
        }

        public CreditOrb(float x, float y, long time)
            : base(x, y, time, CREDITORB_LIFETIME)
        {
            m_velocity = CREDITORB_VELOCITY;
            m_color = Color.Green;
            m_creditAdd = CREDIT_VALUE;
            m_spriteImage = CREDIT_ORB_SPRITE;
            m_spriteOffset = new Vector2(m_spriteImage.Width * 0.5f, m_spriteImage.Height * 0.5f);
        }

        public override void PerformAction(PlayerShip playerShip, long ttms)
        {
            playerShip._credits += m_creditAdd;
            base.PerformAction(playerShip, ttms);
        }
    }
}
