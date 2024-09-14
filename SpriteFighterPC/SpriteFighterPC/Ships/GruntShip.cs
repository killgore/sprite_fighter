using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpriteFighter
{
    public class GruntShip : EnemyShip
    {
        public static Texture2D GRUNT_SPRITE;

        static int GRUNT_LIFE = 100;
        static Color GRUNT_COLOR = Color.White;
        static Vector2 GRUNT_VELOCITY = new Vector2(0, 3);

        public GruntShip()
        {
        }

        public GruntShip(float x, float y, long time)
            : base(x, y, GRUNT_LIFE, time, GRUNT_COLOR, GRUNT_VELOCITY)

        {
            m_spriteImage = GRUNT_SPRITE;
            m_spriteOffset = new Vector2(m_spriteImage.Width * 0.5f, m_spriteImage.Height * 0.5f);
        }

        public override void Update(long ttms)
        {
            base.Move(ttms);
            base.Update(ttms);
        }

        public override void Render(SpriteBatch sb)
        {
            sb.Draw(m_spriteImage, m_position, null, m_color, m_rotation,
                 m_spriteOffset, 0.5f, SpriteEffects.None, 0);
        }

        public override void Restore()
        {
            _spriteImage = GRUNT_SPRITE;
            base.Restore();
        }
    }
}
