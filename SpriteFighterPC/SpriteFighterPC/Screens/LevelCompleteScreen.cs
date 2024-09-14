using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpriteFighter
{
    public class LevelCompleteScreen : GameScreen
    {
        NextLevelEvent m_nextLevelEvent;
        OpenStoreEvent m_openStoreEvent;

        Vector2 titleFontPos = new Vector2(240, 200);
        Vector2 continueFontPos = new Vector2(240, 500);
        Vector2 storeFontPos = new Vector2(240, 600);

        public LevelCompleteScreen(NextLevelEvent nextLevelEvent, OpenStoreEvent openStoreEvent, SpriteBatch sb, SpriteFont sf)
            : base(sb, sf)
        {
            m_nextLevelEvent = nextLevelEvent;
            m_openStoreEvent = openStoreEvent;

            MenuComponent mcTitle = new MenuComponent("Level Complete!", titleFontPos, 1.5f);
            mcTitle._origin = sf.MeasureString(mcTitle._text) / 2;

            MenuComponent mcContinue = new MenuComponent("Next Level >>", continueFontPos, 1.5f, Color.CornflowerBlue);
            mcContinue._origin = sf.MeasureString(mcContinue._text) / 2;
            mcContinue.Selected += ContinueSelected;

            MenuComponent mcStore = new MenuComponent("Ship Upgrades >>", storeFontPos, 1.5f, Color.LawnGreen);
            mcStore._origin = sf.MeasureString(mcStore._text) / 2;
            mcStore.Selected += StoreSelected;

            m_menuComponenents.Add(mcTitle);
            m_menuComponenents.Add(mcStore);
            m_menuComponenents.Add(mcContinue);
       }

        private void ContinueSelected(object sender, EventArgs e)
        {
            m_nextLevelEvent.CreateNextLevelEvent();
        }

        private void StoreSelected(object sender, EventArgs e)
        {
            m_openStoreEvent.CreateOpenStoreEvent();
        }
    }
}
