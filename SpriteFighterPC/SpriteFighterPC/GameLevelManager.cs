using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace SpriteFighter
{
    public class GameLevelManager : DrawableGameComponent
    {
        SpriteBatch m_spriteBatch;

        Game1 m_game;
        GameLevel m_currentLevel;

        protected long m_gameTime = 0; //tick time in milliseconds
        //long m_pauseTime = 0;
        long m_pauseOffset = 0;

        public GameLevelManager(Game1 game, GameLevel gameLevel)
            : base((Game)game)
        {
            m_game = game;
            m_currentLevel = gameLevel;
            m_spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            m_currentLevel.Init();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            long actualTime = (long)gameTime.TotalGameTime.TotalMilliseconds;
            m_gameTime = actualTime - m_pauseOffset;

            if (m_game._gameState == Game1.GameState.PLAYING)
            {
                m_currentLevel.UpdateLevel(m_gameTime);
                base.Update(gameTime);
                if (m_currentLevel.LevelComplete())
                {
                    m_game.m_levelCompleteEvent.CreateLevelCompleteEvent(m_currentLevel, m_gameTime);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (m_game._gameState == Game1.GameState.PLAYING)
            {
                //m_spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque);
                m_currentLevel.Render(m_spriteBatch);
                base.Draw(gameTime);
            }
        }

        public void ChangeLevel(GameLevel gameLevel)
        {
            m_currentLevel = gameLevel;
            m_currentLevel.Init();
        }

        public GameLevel GetCurrentLevel()
        {
            return m_currentLevel;
        }
/*
        public virtual void renderEnemyShips()
        {
            foreach (EnemyShip s in m_currentLevel.GetEnemies())
            {
                s.Render(m_spriteBatch);
            }
        }

        public virtual void renderExplosions()
        {
            foreach (Projectile e in m_currentLevel.GetExplosions())
            {
                e.Render(m_spriteBatch);
            }
        }

        public virtual void renderPowerUps()
        {
            foreach (PowerUp pu in m_currentLevel.GetPowerUps())
            {
                pu.Render(m_spriteBatch);
            }
        }
*/
        public void RestoreLevel(List<EnemyShip> enemies)
        {
            if (m_currentLevel != null)
            {
                m_currentLevel.RestoreEnemies(enemies);
            }
        }
    }
}
