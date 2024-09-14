using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpriteFighter
{
    public class MenuComponent
    {
        Vector2 m_position;
        string m_text;
        float m_textScale = 1.0f;
        Color m_color = Color.White;
        Vector2 m_origin = Vector2.Zero;
        bool m_draw = true;

        public MenuComponent(string text, Vector2 position)
        {
            m_text = text;
            m_position = position;
        }

        public MenuComponent(string text, Vector2 position, float scale)
        {
            m_text = text;
            m_position = position;
            m_textScale = scale;
        }

        public MenuComponent(string text, Vector2 position, float scale, Color color)
        {
            m_text = text;
            m_position = position;
            m_textScale = scale;
            m_color = color;
        }

        public Vector2 _position
        {
            get
            {
                return m_position;
            }

            set
            {
                m_position = value;
            }
        }

        public string _text
        {
            get
            {
                return m_text;
            }

            set
            {
                m_text = value;
            }
        }

        public float _textScale
        {
            get
            {
                return m_textScale;
            }

            set
            {
                m_textScale = value;
            }
        }

        public Color _color
        {
            get
            {
                return m_color;
            }

            set
            {
                m_color = value;
            }
        }

        public Vector2 _origin
        {
            get
            {
                return m_origin;
            }

            set
            {
                m_origin = value;
            }
        }

        public bool _draw
        {
            get
            {
                return m_draw;
            }

            set
            {
                m_draw = value;
            }
        }

        /// <summary>
        /// Event raised when the menu entry is selected.
        /// </summary>
        public event EventHandler<EventArgs> Selected;

        /// <summary>
        /// Method for raising the Selected event.
        /// </summary>
        protected internal virtual void OnSelectEntry()
        {
            if (Selected != null)
                Selected(this, new EventArgs());
        }

        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(GameTime gameTime, SpriteBatch sb, SpriteFont sf) 
        {
            if (m_draw)
                sb.DrawString(sf, m_text, m_position, m_color, 0, m_origin, m_textScale, SpriteEffects.None, 0);
        }

    }
}
