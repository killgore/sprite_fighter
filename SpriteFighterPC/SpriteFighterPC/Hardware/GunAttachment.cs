using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpriteFighter
{
    public class GunAttachment : Attachment
    {
        protected int m_fireRate;
        protected int m_projectileDamage;
        protected Vector2 m_projectileVelocity;
        protected long m_lastFireTime;
        public const int PROJECTILETIME = 3000; //ms
        protected List<Projectile> m_projectiles;
        protected Texture2D m_projectileSpriteImage;

        public GunAttachment()
        {
        }

        public GunAttachment(int firerate, Vector2 projSpeed, int projDamage, Vector2 offset, GameEntity attachedTo, Color color, long time, uint type)
            : base(offset, attachedTo, color, time, type)
        {
            m_fireRate = firerate;
            m_velocity = Vector2.Zero;
            m_projectileVelocity = projSpeed;
            m_projectileDamage = projDamage;
            m_lastFireTime = time;
            m_projectiles = new List<Projectile>();
        }

        public int _fireRate
        {
            get
            {
                return m_fireRate;
            }

            set
            {
                m_fireRate = value;
            }
        }

        public int _projectileDamage
        {
            get
            {
                return m_projectileDamage;
            }

            set
            {
                m_projectileDamage = value;
            }
        }

        public Vector2 _projectileVelocity
        {
            get
            {
                return m_projectileVelocity;
            }

            set
            {
                m_projectileVelocity = value;
            }
        }

        protected bool canFire(long time)
        {
            return ((time - m_lastFireTime) > m_fireRate) && _isActive;
        }

        public List<Projectile> GetProjectiles()
        {
            return m_projectiles;
        }

        protected void updateProjectiles(long ttms)
        {
            for (int i = m_projectiles.Count - 1; i >= 0; i--)
            {
                Projectile p = m_projectiles[i];
                if (p.AmDead(ttms))
                {
                    m_projectiles.RemoveAt(i);
                }
                else
                {
                    p.Move(ttms);
                }
            }
        }

        public override void Update(long ttms)
        {
            checkGenerateProjectile(ttms);
            updateProjectiles(ttms);
            base.Update(ttms);
        }

        public int GetDamage()
        {
            return m_projectileDamage;
        }

        protected virtual void checkGenerateProjectile(long ttms)
        {
            if (canFire(ttms))
            {
                generateProjectile(ttms);
                m_lastFireTime = ttms;
                PlaySoundEffect();
            }
        }

        protected virtual void generateProjectile(long ttms)
        {
            float x = m_parent._position.X + m_offset.X;
            float y = m_parent._position.Y + m_offset.Y;
            Vector2 startPosition = new Vector2(x, y);
            Projectile p = new Projectile(startPosition, m_projectileVelocity, m_color, m_projectileDamage, ttms, PROJECTILETIME);
            m_projectiles.Add(p);
        }

        protected virtual void generateProjectile(long ttms, Texture2D projectileImage)
        {
            float x = m_parent._position.X + m_offset.X;
            float y = m_parent._position.Y + m_offset.Y;
            Vector2 startPosition = new Vector2(x, y);
            Projectile p = new Projectile(startPosition, m_projectileVelocity, m_color, m_projectileDamage, ttms, PROJECTILETIME, projectileImage);
            m_projectiles.Add(p);
        }

        protected virtual void generateProjectileOffset(long ttms, Vector2 extraOffset)
        {
            float x = m_parent._position.X + m_offset.X;
            float y = m_parent._position.Y + m_offset.Y; 
            Vector2 startPosition = new Vector2(x, y);
            startPosition = startPosition + extraOffset;
            Projectile p = new Projectile(startPosition, m_projectileVelocity, m_color, m_projectileDamage, ttms, PROJECTILETIME);
            p._velocity = m_projectileVelocity;
            m_projectiles.Add(p);
        }

        protected virtual void generateProjectileSpeedAndOffset(long ttms, Vector2 speedAdjust, Vector2 extraOffset)
        {
            float x = m_parent._position.X + m_offset.X;
            float y = m_parent._position.Y + m_offset.Y;
            Vector2 startPosition = new Vector2(x, y);
            startPosition = startPosition + extraOffset;
            Projectile p = new Projectile(startPosition, m_projectileVelocity, m_color, m_projectileDamage, ttms, PROJECTILETIME);
            p._velocity = speedAdjust;
            m_projectiles.Add(p);
        }

        protected virtual void generateProjectileSpeedAdjust(long ttms, Vector2 speedAdjust)
        {

            Vector2 startPosition = new Vector2(m_parent._position.X, m_parent._position.Y);
            Projectile p = new Projectile(startPosition, m_projectileVelocity, m_color, m_projectileDamage, ttms, PROJECTILETIME);
            p._velocity = speedAdjust;
            m_projectiles.Add(p);
        }

        protected virtual void addProjectile(Projectile proj)
        {
            m_projectiles.Add(proj);
        }

        private void renderProjectiles(SpriteBatch sb)
        {
            foreach (Projectile p in m_projectiles)
            {
                p.Render(sb);
            }
        }

        public override void Render(SpriteBatch sb)
        {
            renderProjectiles(sb);
            base.Render(sb);
        }
    }
}
