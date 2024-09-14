using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpriteFighter
{
    public class SidewinderShip : EnemyShip
    {
        public static Texture2D SIDEWINDER_SPRITE;

        static int SIDEWINDER_LIFE = 200;
        static Color SIDEWINDER_COLOR = Color.Magenta;
        static Vector2 SIDEWINDER_VELOCITY = new Vector2(-10, 5);

        static Random random = new Random();

        //offset on the timeline so these guys arent all 
        //riding the same sine wave.
        long m_noise;

        public SidewinderShip()
        {
        }

        public SidewinderShip(float x, float y, long time)
            : base(x, y, SIDEWINDER_LIFE, time, SIDEWINDER_COLOR, SIDEWINDER_VELOCITY)

        {
            m_noise = (long)(random.NextDouble() * 1000);
            m_spriteImage = SIDEWINDER_SPRITE;
            m_spriteOffset = new Vector2(m_spriteImage.Width * 0.5f, m_spriteImage.Height * 0.5f);
        }

        public override void Update(long ttms)
        {
            Move(ttms);
            base.Update(ttms);
        }

        public override void Move(long ttms)
        {
            m_velocity.X = ((float)Math.Sin((ttms+m_noise)/100)) * SIDEWINDER_VELOCITY.X;
            base.Move(ttms);
        }

        public override void Render(SpriteBatch sb)
        {
            sb.Draw(m_spriteImage, m_position, null, m_color, m_rotation,
                 m_spriteOffset, 0.5f, SpriteEffects.None, 0);
        }

        public override void Restore()
        {
            _spriteImage = SIDEWINDER_SPRITE;
            base.Restore();
        }
    }
}
