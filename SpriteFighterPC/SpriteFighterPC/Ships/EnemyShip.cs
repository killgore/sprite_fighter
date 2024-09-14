using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpriteFighter
{
    public class EnemyShip : SpaceShip
    {
        public EnemyShip()
        {
        }

        public EnemyShip(float x, float y, int life, long time, Color color, Vector2 velocity)
            : base(x, y, life, time)

        {
            m_color = color;
            m_velocity = velocity;
        }

        public override void Render(SpriteBatch sb)
        {
            base.Render(sb);
        }
    }
}
