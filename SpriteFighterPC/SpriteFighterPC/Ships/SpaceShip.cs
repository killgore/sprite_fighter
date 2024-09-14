using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpriteFighter
{
    public class SpaceShip : GameEntity
    {
        static Random random = new Random();

        private int m_life;
        private int m_maxLife;

        protected List<Attachment> m_attachments = new List<Attachment>();
        protected BitField m_currentAttachments;      

        public SpaceShip()
        {
        }

        public SpaceShip(float x, float y, int life, long time)
        {
            m_position = new Vector2(x, y);
            m_rotation = 0;//(float)(random.NextDouble() * 2.0 * Math.PI);
            m_birthtime = time;
            m_life = life;
            m_maxLife = life;
            m_currentAttachments = new BitField();
        }

        public int _life
        {
            get
            {
                return m_life;
            }

            set
            {
                m_life = value;
            }
        }

        public int GetMaxLife()
        {
            return m_maxLife;
        }

        public void TakeDamage(int dmg)
        {
            m_life -= dmg;
            if (m_life < 0)
            {
                m_life = 0;
                DestroyMe();
            }
        }

        public virtual void AddAttachment(Attachment attachment)
        {
            m_attachments.Add(attachment);
        }

        public List<Projectile> GetProjectiles()
        {
            List<Projectile> projectiles = new List<Projectile>();
            foreach (Attachment attachment in m_attachments)
            {
                GunAttachment gun = attachment as GunAttachment;
                if (gun != null)
                    projectiles.AddRange(gun.GetProjectiles());
            }

            return projectiles;
        }

        public virtual void renderAttachments(SpriteBatch sb)
        {
            foreach (Attachment a in m_attachments)
            {
                a.Render(sb);
            }
        }

        public override void Render(SpriteBatch sb)
        {
            renderAttachments(sb);
            base.Render(sb);
        }

        public virtual void Render(SpriteBatch sb, float scale)
        {
        }
    }
}
