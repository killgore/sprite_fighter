using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpriteFighter
{
    public class StrikerShip : EnemyShip
    {
        public static Texture2D STRIKER_SPRITE;

        protected static int STRIKER_LIFE = 100;
        protected static Color STRIKER_COLOR = Color.Blue;
        protected static Vector2 STRIKER_VELOCITY = new Vector2(3, 6);

        private static int STRIKER_FIRE_RATE = 1000;

        public StrikerShip()
        {
        }

        public StrikerShip(float x, float y, long time)
            : base(x, y, STRIKER_LIFE, time, STRIKER_COLOR, STRIKER_VELOCITY)

        {
            m_spriteImage = STRIKER_SPRITE;
            m_spriteOffset = new Vector2(m_spriteImage.Width * 0.5f, m_spriteImage.Height * 0.5f);
            double ran = Game1.random.NextDouble();
            if( ran > 0.5f )
                m_velocity.X = -m_velocity.X;
            BaseCannon bc = new BaseCannon(this, time);
            bc._projectileVelocity = new Vector2(bc._projectileVelocity.X, -bc._projectileVelocity.Y);
            bc._fireRate = STRIKER_FIRE_RATE;
            bc._color = Color.OrangeRed;
            AddAttachment(bc);
        }

        public override void Update(long ttms)
        {
            foreach (Attachment at in m_attachments)
            {
                at.Update(ttms);
            }
            base.Move(ttms);
            base.Update(ttms);
        }

        public override void Render(SpriteBatch sb)
        {
            sb.Draw(m_spriteImage, m_position, null, m_color, m_rotation,
                 m_spriteOffset, 0.5f, SpriteEffects.None, 0);
            base.Render(sb);
        }

        public override void Restore()
        {
            _spriteImage = STRIKER_SPRITE;
            base.Restore();
        }
    }
}
