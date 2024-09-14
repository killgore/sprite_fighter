using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpriteFighter
{
    class HardwareMenuComponent : MenuComponent
    {
        private uint m_type;
        private int m_cost;

        public HardwareMenuComponent(uint type, int cost, string text, Vector2 position)
            : base(text, position)
        {
            m_type = type;
            m_cost = cost;
        }

        public HardwareMenuComponent(uint type, int cost, string text, Vector2 position, float scale)
            : base(text, position, scale)
        {
            m_type = type;
            m_cost = cost;
        }

        public HardwareMenuComponent(uint type, int cost, string text, Vector2 position, float scale, Color color)
            : base(text, position, scale, color)
        {
            m_type = type;
            m_cost = cost;
        }

        public uint _type
        {
            get
            {
                return m_type;
            }

            set
            {
                m_type = value;
            }
        }

        public int _cost
        {
            get
            {
                return m_cost;
            }

            set
            {
                m_cost = value;
            }
        }
    }
}
