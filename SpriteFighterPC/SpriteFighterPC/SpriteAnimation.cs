using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpriteFighter
{
    public class SpriteAnimation
    {
        private Texture2D m_texture;
        private Vector2 m_offset;
        private Point m_frameSize;
        private Point m_currentFrame;
        private Point m_sheetSize;
        private Rectangle m_currentFrameArea;

        public SpriteAnimation()
        {
        }

        public SpriteAnimation(Texture2D texture, Point frameSize, Point sheetSize, Vector2 offset)
        {
            m_texture = texture;
            m_offset = offset;
            m_frameSize = frameSize;
            m_currentFrame = new Point(0,0);
            m_sheetSize = sheetSize;
            m_currentFrameArea = new Rectangle(m_currentFrame.X * m_frameSize.X,
                                               m_currentFrame.Y * m_frameSize.Y,
                                               m_frameSize.X, m_frameSize.Y);
        }

        public void Update()
        {
            m_currentFrame.X++;
            if (m_currentFrame.X >= m_sheetSize.X)
            {
                m_currentFrame.X = 0;
                m_currentFrame.Y++;
                if (m_currentFrame.Y >= m_sheetSize.Y)
                    m_currentFrame.Y = 0;                
            }

            m_currentFrameArea = new Rectangle(m_currentFrame.X * m_frameSize.X,
                                   m_currentFrame.Y * m_frameSize.Y,
                                   m_frameSize.X, m_frameSize.Y);
        }

        public void Draw(SpriteBatch sb, Vector2 position, Color color)
        {
            sb.Draw(m_texture, position, m_currentFrameArea, color, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
        }

        public void Draw(SpriteBatch sb, Vector2 position, Color color, float scale, float layer)
        {
            sb.Draw(m_texture, position, m_currentFrameArea, color, 0, Vector2.Zero, scale, SpriteEffects.None, layer); 
        }
    }
}
