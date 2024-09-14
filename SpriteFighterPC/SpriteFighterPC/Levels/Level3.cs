using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpriteFighter
{
    public class Level3 : GameLevel
    {
        public Level3(PlayerShip player, Game1 game, Point screenSize)
            : base(player, game, screenSize)
        {

        }

        public static Level3 GetLevel(PlayerShip player, Game1 game, Point screenSize)
        {
            return new Level3(player, game, screenSize);
        }

        public override void checkGenerateEnemy(long ttms)
        {
            int ran = Game1.random.Next(100);

            float x = (float)(m_enemyShipArea.X * Game1.random.NextDouble());
            float y = (float)(m_enemyShipArea.Y * Game1.random.NextDouble());
            EnemyShip eShip = null;

            if (ran > 98)
            {
                eShip = new GruntShip(x, y, ttms);
                m_enemies.Add(eShip);
            }
            else if (ran > 95)
            {
                eShip = new SidewinderShip(x, y, ttms);
                m_enemies.Add(eShip);
            }
            else if (ran > 92)
            {
                eShip = new StrikerShip(x, y, ttms);
                m_enemies.Add(eShip);
            }
        }

        public override bool LevelComplete()
        {
            if (m_levelKills >= 75)
            {
                return true;
            }
            return false;
        }
    }
}
