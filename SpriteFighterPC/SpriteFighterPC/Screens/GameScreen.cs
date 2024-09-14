using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

#if WINPHONE
using Microsoft.Xna.Framework.Input.Touch;
#endif

namespace SpriteFighter
{
    public abstract class GameScreen
    {
        protected List<MenuComponent> m_menuComponenents = new List<MenuComponent>();

        protected SpriteBatch m_sprtieBatch;
        protected SpriteFont m_spriteFont;

        public GameScreen(SpriteBatch sb, SpriteFont sf)
        {
            m_sprtieBatch = sb;
            m_spriteFont = sf;
        }

#if WINPHONE
        public virtual void HandleInput(TouchLocation tl) 
        {
            foreach (MenuComponent mc in m_menuComponenents)
            {
                if (GetMenuComponentHitBounds(mc).Contains((int)tl.Position.X, (int)tl.Position.Y))
                {
                    mc.OnSelectEntry();
                }
            }
        }
#endif
        public virtual void HandleInput(MouseState ms)
        {
            foreach (MenuComponent mc in m_menuComponenents)
            {
                if (GetMenuComponentHitBounds(mc).Contains((int)ms.X, (int)ms.Y))
                {
                    mc.OnSelectEntry();
                }
            }
        }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(GameTime gameTime) 
        { 
            foreach( MenuComponent mc in m_menuComponenents )
            {
                mc.Draw(gameTime, m_sprtieBatch, m_spriteFont);
                //Debuggery
                //Rectangle rect = GetMenuComponentHitBounds(mc);
                //m_sprtieBatch.Draw(StasisRayProjectile.STASIS_RAY_SPRITE, rect, Color.Lavender);
            }
        }

        public virtual Rectangle GetMenuComponentHitBounds(MenuComponent mc)
        {
            Rectangle rect;
            int width = (int)(m_spriteFont.MeasureString(mc._text).X * mc._textScale);
            int height = (int)(m_spriteFont.MeasureString(mc._text).Y); //Scale doesnt seem to matter as much for Y
            int x = (int)mc._position.X - (int)(width / 2);
            int y = (int)mc._position.Y - (int)(height / 2);
            rect = new Rectangle(x, y, width, height);

            return rect;
        }
    }
}
