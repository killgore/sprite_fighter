using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpriteFighter
{
    public class TitleScreen : GameScreen
    {
        public TitleScreen(int highScore, SpriteBatch sb, SpriteFont sf)
            : base(sb, sf)
        {
            MenuComponent mcTitle = new MenuComponent("Sprite Fighter", new Vector2(240, 200), 2.5f);
            mcTitle._origin = sf.MeasureString(mcTitle._text) / 2;
            MenuComponent mcInstruct = new MenuComponent("Touch Screen To Play!", new Vector2(240, 300));
            mcInstruct._origin = sf.MeasureString(mcInstruct._text) / 2;
            string strhighScore = "High Score: " + Convert.ToString(highScore);
            MenuComponent mcHighScore = new MenuComponent(strhighScore, new Vector2(240, 24), 1.0f, Color.CornflowerBlue);
            mcHighScore._origin = sf.MeasureString(mcHighScore._text) / 2;

            m_menuComponenents.Add(mcTitle);
            m_menuComponenents.Add(mcInstruct);
            m_menuComponenents.Add(mcHighScore);
        }
    }
}
