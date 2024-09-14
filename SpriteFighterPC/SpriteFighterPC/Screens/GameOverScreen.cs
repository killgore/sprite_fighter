using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpriteFighter
{
    class GameOverScreen : GameScreen
    {
        long m_gameOverTime;
        long m_gameOverWait;

        MenuComponent m_mcWait;
        MenuComponent m_mcInstruct;

        Vector2 gameOverFontPos = new Vector2(240, 200);
        Vector2 waitFontPos = new Vector2(240, 400);
        Vector2 instructFontPos = new Vector2(240, 300);

        public GameOverScreen(long gameOverTime, long gameOverWait, SpriteBatch sb, SpriteFont sf)
            : base(sb, sf)
        {
            m_gameOverTime = gameOverTime;
            m_gameOverWait = gameOverWait;

            string gameOver = "GAME OVER";
            MenuComponent mcTitle = new MenuComponent(gameOver, gameOverFontPos, 2.5f);
            mcTitle._origin = sf.MeasureString(mcTitle._text) / 2;

            string wait = "Please wait: " + Convert.ToString(gameOverWait);
            m_mcWait = new MenuComponent(wait, waitFontPos);
            m_mcWait._origin = sf.MeasureString(m_mcWait._text) / 2;

            string instruction = "Touch Screen To Play Again";
            m_mcInstruct = new MenuComponent(instruction, instructFontPos);
            m_mcInstruct._origin = sf.MeasureString(m_mcInstruct._text) / 2;

            m_menuComponenents.Add(mcTitle);
        }

        public override void Draw(GameTime gameTime)
        {
            long ttms = (long)gameTime.TotalGameTime.TotalMilliseconds;
            if ((ttms - m_gameOverTime) < m_gameOverWait)
            {
                long currentWait = m_gameOverWait - (ttms - m_gameOverTime);
                string wait = "Please wait: " + Convert.ToString(currentWait);
                m_mcWait._text = wait;
                m_mcWait.Draw(gameTime, m_sprtieBatch, m_spriteFont);
            }
            else
            {
                m_mcInstruct.Draw(gameTime, m_sprtieBatch, m_spriteFont);
            }

            base.Draw(gameTime);
        }
    }
}
