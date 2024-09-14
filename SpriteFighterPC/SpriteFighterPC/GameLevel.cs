using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

#if WINPHONE
using Microsoft.Devices;
#endif

namespace SpriteFighter
{
    public class GameLevel
    {
        const int ENEMYSHIPTIME = 2000; //ms
        const int EXPLOSIONTIME = 500; //ms
        const long POWERUP_GENERATE_WAIT = 2000; //ms

        protected List<EnemyShip> m_enemies;
        protected PlayerShip m_player;
        protected List<Projectile> m_explosions;
        protected List<PowerUp> m_powerUps;
        protected Vector2 m_enemyShipArea = new Vector2(475, 300); //slightly inside edge;
        protected Vector2 m_powerUpArea = new Vector2(480, 600);
        protected long m_powerUpGenerateTime;
        protected Rectangle m_playArea;
        protected Point m_screenSize;
        protected SoundEffectInstance m_levelMusic;
        protected Game1 m_game;
        protected int m_levelKills;
        protected StarField m_starField;

#if WINPHONE
        protected VibrateController m_vibrator;
#endif

        public GameLevel(PlayerShip player, Game1 game, Point screenSize)
        {
            m_game = game;
            m_enemies = new List<EnemyShip>();
            m_player = player;
            m_explosions = new List<Projectile>();
            m_powerUps = new List<PowerUp>();
            m_screenSize = screenSize;
            m_playArea = new Rectangle(-100, -100, m_screenSize.X + 200, m_screenSize.Y + 200);
            m_levelKills = 0;
            m_starField = new StarField(m_screenSize);
#if WINPHONE
            m_vibrator = VibrateController.Default;
#endif
        }

        public List<EnemyShip> GetEnemies()
        {
            return m_enemies;
        }

        public void RestoreEnemies(List<EnemyShip> enemies)
        {
            m_enemies = enemies;
            foreach (EnemyShip ship in m_enemies)
                ship.Restore();
        }

        public List<Projectile> GetExplosions()
        {
            return m_explosions;
        }

        public List<PowerUp> GetPowerUps()
        {
            return m_powerUps;
        }

        public virtual void checkEnemyHitByProjectile(long ttms)
        {
            foreach (Projectile p in m_player.GetProjectiles())
            {
                foreach (EnemyShip ss in m_enemies)
                {
                    BoundingBox pb = p.GetBoundingBox(0.5f);
                    BoundingBox ssb = ss.GetBoundingBox();
                    if (pb.Intersects(ssb))
                    {
                        ss.TakeDamage(p.GetDamage());
                        p.Affect(ss);
                        generateExplosion(ss._position, ttms);
                        p.DestroyMe();
                    }
                }
            }
        }

        public virtual void checkPlayerHitByEnemyProjectile(long ttms)
        {
            foreach (EnemyShip es in m_enemies)
            {
                foreach (Projectile p in es.GetProjectiles())
                {
                    if (p.AmDead())
                        continue;
                    BoundingBox pb = p.GetBoundingBox(0.5f);
                    BoundingBox playb = m_player.GetBoundingBox();
                    if (pb.Intersects(playb))
                    {
                        m_player.TakeDamage(p.GetDamage());
                        p.Affect(m_player);
                        generateExplosion(m_player._position, ttms);
#if WINPHONE
                        m_vibrator.Start(TimeSpan.FromMilliseconds(150));
#endif
                        p.DestroyMe();
                    }
                }
            }
        }

        public virtual void checkEnemyHitPlayerShield(long ttms)
        {
            Shield shield = (Shield)m_player.GetAttachmentByType(Attachment.Shield);
            if (shield != null)
            {
                foreach (EnemyShip es in m_enemies)
                {
                    if (Math.Abs(shield._position.Y - es._position.Y) < 64) //only check if its close
                    {
                        checkShieldImpact(shield, es, ttms);
                    }

                    List<Projectile> esProjectiles = es.GetProjectiles();
                    foreach (Projectile p in esProjectiles)
                    {
                        if (Math.Abs(shield._position.Y - p._position.Y) < 64) //only check if its close
                        {
                            checkShieldImpact(shield, p, ttms);
                        }
                    }
                }
            }
        }

        private void checkShieldImpact(Shield shield, GameEntity entity, long ttms)
        {
            BoundingBox shieldb = shield.GetBoundingBox();
            BoundingBox eb = entity.GetBoundingBox();
            if (shieldb.Intersects(eb))
            {
                generateExplosion(entity._position, ttms);
                shield.TakeDamage();
                entity.DestroyMe();
            }
        }

        public virtual void checkPlayerHitByEnemyShip(long ttms)
        {
            foreach (EnemyShip ss in m_enemies)
            {
                BoundingBox pb = m_player.GetBoundingBox(0.5f);
                BoundingBox ssb = ss.GetBoundingBox(0.5f);
                if (pb.Intersects(ssb))
                {
                    m_player.TakeDamage(ss._life);
                    ss.DestroyMe();
                    generateExplosion(ss._position, ttms);
#if WINPHONE
                    m_vibrator.Start(TimeSpan.FromMilliseconds(150));
#endif
                    //soundEffect.Play();
                }
            }
        }

        public virtual void checkPlayerHitPowerUp(long ttms)
        {
            foreach (PowerUp pu in m_powerUps)
            {
                BoundingBox pb = m_player.GetBoundingBox();
                BoundingBox pub = pu.GetBoundingBox();
                if (pb.Intersects(pub))
                {
                    pu.PerformAction(m_player, ttms);
                    //soundEffect.Play();
                }
            }
        }

        public virtual void generateExplosion(Vector2 position, long ttms)
        {
            Explosion boom = new Explosion(position, Vector2.Zero, Color.Orange, 0, ttms, EXPLOSIONTIME);
            boom.PlaySoundEffect();
            m_explosions.Add(boom);
        }

        public virtual void generateCreditOrb(Vector2 position, long ttms)
        {
            CreditOrb pu = new CreditOrb(position.X, position.Y, ttms);
            m_powerUps.Add(pu);
        }

        public virtual void checkGenerateEnemy(long ttms)
        {
            int ran = Game1.random.Next(100);

            float x = (float)(m_enemyShipArea.X * Game1.random.NextDouble());
            float y = (float)(m_enemyShipArea.Y * Game1.random.NextDouble());
            EnemyShip eShip = null;

            if (ran > 90)
            {
                eShip = new GruntShip(x, y, ttms);
                m_enemies.Add(eShip);
            }
            else if (ran > 80 && ran <= 90)
            {
                eShip = new SidewinderShip(x, y, ttms);
                m_enemies.Add(eShip);
            }
            else if (ran > 70 && ran <= 80)
            {
                if (!JuggernautShip.IsJuggyActive())
                {
                    eShip = new JuggernautShip(x, y, ttms, m_screenSize);
                    m_enemies.Add(eShip);
                }
            }
        }

        public virtual void checkGeneratePowerUp(long ttms)
        {
            if ((ttms - m_powerUpGenerateTime) > POWERUP_GENERATE_WAIT)
            {
                m_powerUpGenerateTime = ttms;
                double odds = Game1.random.NextDouble();
                float x = (float)(m_powerUpArea.X * Game1.random.NextDouble());
                float y = (float)(m_powerUpArea.Y * Game1.random.NextDouble());

                //odds = 0.03f;

                if (odds > 0.80f)
                {
                    CreditOrb pu = new CreditOrb(x, y, ttms);
                    m_powerUps.Add(pu);
                }
                else if (odds > 0.60f)
                {
                    int life = 100;
                    HealthPack pu = new HealthPack(x, y, life, ttms);
                    m_powerUps.Add(pu);
                }
                else if (odds > 0.40f)
                {
                    SpreadCannonUpgrade pu = new SpreadCannonUpgrade(x, y, ttms);
                    m_powerUps.Add(pu);
                }
                else if (odds > 0.20f)
                {
                    IonCannonUpgrade pu = new IonCannonUpgrade(x, y, ttms);
                    m_powerUps.Add(pu);
                }
                else if (odds > 0.10f)
                {
                    GravityWellUpgrade pu = new GravityWellUpgrade(x, y, m_enemies, ttms);
                    m_powerUps.Add(pu);
                }
                else if (odds > 0.05f)
                {
                    ShieldUpgrade pu = new ShieldUpgrade(x, y, ttms);
                    m_powerUps.Add(pu);
                }
                else
                {
                    StasisRayUpgrade pu = new StasisRayUpgrade(x, y, ttms);
                    m_powerUps.Add(pu);
                }
            }
        }

        public virtual void updateExplosions(long ttms)
        {
            for (int i = m_explosions.Count - 1; i >= 0; i--)
            {
                Projectile e = m_explosions[i];
                if (e.AmDead(ttms))
                {
                    m_explosions.RemoveAt(i);
                }
            }
        }

        public virtual void updateEnemyShips(long ttms)
        {
            for (int i = m_enemies.Count - 1; i >= 0; i--)
            {
                SpaceShip ss = m_enemies[i];
                if (ss.AmDead())
                {
                    m_enemies.RemoveAt(i);
                    generateCreditOrb(ss._position, ttms);
                    ss = null;
                    m_game.AddPlayerPoints(1);
                    m_levelKills++;
                    if (m_game._playerPoints >= m_game._highScore)
                    {
                        m_game._highScore = m_game._playerPoints;
                    }
                }
                else if (!InBounds(ss._position))
                {
                    m_enemies.RemoveAt(i);
                    ss.DestroyMe();
                    ss = null;
                }
                else
                {
                    ss.Update(ttms);
                }
            }
        }

        public void updatePowerUps(long ttms)
        {
            for (int i = m_powerUps.Count - 1; i >= 0; i--)
            {
                PowerUp p = m_powerUps[i];
                if (p.AmDead(ttms))
                {
                    m_powerUps.RemoveAt(i);
                }
                else
                {
                    p.Move(ttms);
                }
            }
        }

        public virtual void renderEnemyShips(SpriteBatch sb)
        {
            foreach (EnemyShip s in GetEnemies())
            {
                s.Render(sb);
            }
        }

        public virtual void renderExplosions(SpriteBatch sb)
        {
            foreach (Projectile e in GetExplosions())
            {
                e.Render(sb);
            }
        }

        public virtual void renderPowerUps(SpriteBatch sb)
        {
            foreach (PowerUp pu in GetPowerUps())
            {
                pu.Render(sb);
            }
        }

        public bool InBounds(Vector2 position)
        {
            bool isInBounds = true;

            if ((position.X > m_playArea.Right) || (position.X < m_playArea.Left))
                isInBounds = false;

            if ((position.Y < m_playArea.Top) || (position.Y > m_playArea.Bottom))
                isInBounds = false;

            return isInBounds;
        }

        public virtual bool LevelComplete()
        {
            return false;
        }

        public void UpdateLevel(long ttms)
        {
            updateEnemyShips(ttms);
            updateExplosions(ttms);
            updatePowerUps(ttms);

            checkGenerateEnemy(ttms);
            //checkGeneratePowerUp(ttms);

            checkEnemyHitByProjectile(ttms);
            checkEnemyHitPlayerShield(ttms);
            checkPlayerHitByEnemyShip(ttms);
            checkPlayerHitByEnemyProjectile(ttms);
            checkPlayerHitPowerUp(ttms);

            m_starField.Update(ttms);
        }

        public void Render(SpriteBatch sb)
        {
            sb.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            renderEnemyShips(sb);
            renderExplosions(sb);
            renderPowerUps(sb);
            m_game.GraphicsDevice.BlendState = BlendState.Opaque;
            m_starField.Render(sb);
            sb.End();
        }

        public void Init()
        {
            foreach (Attachment a in m_player.GetAttacments())
            {
                if (a.GetAttachType() == Attachment.GravityWell)
                {
                    ((GravityWell)a).SetVictims(m_enemies);
                }
            }

            m_starField.InitStarField();
        }
    }
}
