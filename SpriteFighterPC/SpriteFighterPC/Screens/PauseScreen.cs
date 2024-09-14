using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpriteFighter
{
    class PauseScreen : GameScreen
    {
        Vector2 pauseFontPos = new Vector2(240, 200);
        Vector2 instructionPos = new Vector2(240, 300);

        public PauseScreen(SpriteBatch sb, SpriteFont sf)
            : base(sb, sf)
        {
            string paused = "PAUSED";
            MenuComponent mcPause = new MenuComponent(paused, pauseFontPos, 2.5f);
            mcPause._origin = sf.MeasureString(mcPause._text) / 2;

            string instruction = "Press Back To Resume";
            MenuComponent mcInstruct = new MenuComponent(instruction, instructionPos);
            mcInstruct._origin = sf.MeasureString(mcInstruct._text) / 2;

            m_menuComponenents.Add(mcPause);
            m_menuComponenents.Add(mcInstruct);
        }
    }
}
