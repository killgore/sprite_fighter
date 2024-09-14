using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpriteFighter
{
    public class Attachment : GameEntity
    {
        public const uint BaseCannon = 1;
        public const uint SpreadCannon = 2;
        public const uint IonCannon = 4;
        public const uint GravityWell = 8;
        public const uint Shield = 16;
        public const uint StasisRay = 32;

        protected GameEntity m_parent;
        protected Vector2 m_offset;
        protected uint m_type;
        protected bool m_isActive;

        public Attachment()
        {
        }

        public Attachment(Vector2 offset, GameEntity attachedTo, Color color, long time, uint type)
        {
            m_parent = attachedTo;
            m_offset = offset;
            m_color = color;
            m_velocity = new Vector2(0, 0);
            m_position = attachedTo._position + offset;
            m_rotation = attachedTo._rotation;
            m_birthtime = time;
            m_type = type;
            m_isActive = true;
        }

        public bool _isActive
        {
            get
            {
                return m_isActive;
            }

            set
            {
                m_isActive = value;
            }
        }

        public uint GetAttachType()
        {
            return m_type;
        }

        public override void Move(long ttms)
        {
            m_position = m_parent._position + m_offset;
            base.Move(ttms);
        }

        public override void Update(long ttms)
        {
            Move(ttms);
            base.Update(ttms);
        }

        public override void Render(SpriteBatch sb)
        {
            base.Render(sb);
        }

        public virtual void Reset()
        {

        }
    }
}
