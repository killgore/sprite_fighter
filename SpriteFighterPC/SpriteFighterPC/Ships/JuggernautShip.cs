using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpriteFighter
{
    public class JuggernautShip : EnemyShip
    {
        public static Texture2D JUGGERNAUT_SPRITE;

        static int JUGGERNAUT_LIFE = 500;
        static Color JUGGERNAUT_COLOR = Color.White;
        static Vector2 JUGGERNAUT_VELOCITY = new Vector2(5,5);
        static int JUGGERNAUT_LIFESPAN = 10000;
        static int SPRITE_ANIM_FRAME_WIDTH = 75;
        static int SPRITE_ANIM_FRAME_HEIGHT = 75;

        private static bool m_active = false;
        private SpriteAnimation m_spriteAnim;
        private Point m_bounds;

        public JuggernautShip()
        {
        }

        public JuggernautShip(float x, float y, long time, Point bounds)
            : base(x, y, JUGGERNAUT_LIFE, time, JUGGERNAUT_COLOR, JUGGERNAUT_VELOCITY)

        {
            m_active = true;
            m_bounds = bounds;
            m_spriteImage = JUGGERNAUT_SPRITE;
            m_spriteOffset = new Vector2(SPRITE_ANIM_FRAME_WIDTH * 0.5f, SPRITE_ANIM_FRAME_HEIGHT * 0.5f);
            m_spriteAnim = new SpriteAnimation(m_spriteImage, new Point(SPRITE_ANIM_FRAME_WIDTH, SPRITE_ANIM_FRAME_HEIGHT), new Point(6, 8), m_spriteOffset);
        }


        static public bool IsJuggyActive()
        {
            return m_active;
        }

        static public void Reset()
        {
            m_active = false;
        }

        public override void Update(long ttms)
        {
            if ((ttms - m_birthtime) < JUGGERNAUT_LIFESPAN)
            {
                Move(ttms);
                m_spriteAnim.Update();
            }
            else
            {
                DestroyMe();
            }

            base.Update(ttms);
        }

        public override void DestroyMe()
        {
            m_active = false;
            base.DestroyMe();
        }

        public override void Move(long ttms)
        {            
            base.Move(ttms);

            if ((m_position.X > (m_bounds.X - SPRITE_ANIM_FRAME_WIDTH)) && (m_velocity.X > 0))
                m_velocity.X = m_velocity.X * -1;

            if (m_position.X < 0)
                m_velocity.X = m_velocity.X * -1;

            if ((m_position.Y > (m_bounds.Y - SPRITE_ANIM_FRAME_HEIGHT)) && (m_velocity.Y > 0))
                m_velocity.Y = m_velocity.Y * -1; 

            if (m_position.Y < 0)
                m_velocity.Y = m_velocity.Y * -1; 
        }

        public override void Render(SpriteBatch sb)
        {
            m_spriteAnim.Draw(sb, m_position, m_color);
        }

        public override void Restore()
        {
            _spriteImage = JUGGERNAUT_SPRITE;
            m_spriteAnim = new SpriteAnimation(m_spriteImage, new Point(SPRITE_ANIM_FRAME_WIDTH, SPRITE_ANIM_FRAME_HEIGHT), new Point(6, 8), m_spriteOffset);
            base.Restore();
        }
    }
}
