using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpriteFighter
{
    public class PlayScreen : GameScreen
    {
        MenuComponent m_mcScore;
        MenuComponent m_mcLife;
        MenuComponent m_mcHighScore;
        MenuComponent m_mcCredits;

        Vector2 scoreFontPos = new Vector2(340, 54);
        Vector2 lifeFontPos = new Vector2(70, 54);
        Vector2 highScoreFontPos = new Vector2(240, 24);
        Vector2 creditsFontPos = new Vector2(0, 64);

        const string scoreText = "Score: ";
        const string lifeText = "Life: ";
        const string highScoreText = "High Score: ";
        const string creditsText = "Credits: ";

        public PlayScreen(int score, int life, int highScore, int credits, SpriteBatch sb, SpriteFont sf)
            : base(sb, sf)
        {
            string strScore = scoreText + Convert.ToString(score);
            m_mcScore = new MenuComponent(strScore, scoreFontPos);
            m_mcScore._origin = sf.MeasureString(m_mcScore._text) / 2;

            string strLife = lifeText + Convert.ToString(life);
            m_mcLife = new MenuComponent(strLife, lifeFontPos, 1.0f, Color.LawnGreen);
            m_mcLife._origin = sf.MeasureString(m_mcLife._text) / 2;

            string strhighScore = highScoreText + Convert.ToString(highScore);
            m_mcHighScore = new MenuComponent(strhighScore, highScoreFontPos, 1.0f, Color.CornflowerBlue);
            m_mcHighScore._origin = sf.MeasureString(m_mcHighScore._text) / 2;

            string strCredits = creditsText + Convert.ToString(credits);
            m_mcCredits = new MenuComponent(strCredits, creditsFontPos, 1.0f, Color.SeaGreen);
            //m_mcCredits._origin = sf.MeasureString(m_mcCredits._text) / 2;

            m_menuComponenents.Add(m_mcScore);
            m_menuComponenents.Add(m_mcLife);
            m_menuComponenents.Add(m_mcHighScore);
            m_menuComponenents.Add(m_mcCredits);
        }

        public void Update(int score, int life, int highScore, int credits, GameTime gameTime)
        {
            m_mcScore._text = scoreText + Convert.ToString(score);
            m_mcLife._text = lifeText + Convert.ToString(life);
            m_mcHighScore._text = highScoreText + Convert.ToString(highScore);
            m_mcCredits._text = creditsText + Convert.ToString(credits);

            float lifeRatio = (float)life / PlayerShip.PLAYER_LIFE;

            if (lifeRatio < .50)
            {
                m_mcLife._color = Color.Yellow;
                if (lifeRatio < .25)
                {
                    m_mcLife._color = Color.Red;
                }
            }

            base.Update(gameTime);
        }
    }
}
