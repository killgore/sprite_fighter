using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpriteFighter
{
    public class PlayerShip : SpaceShip
    {
        public static Texture2D PLAYER_SPRITE;

        public const int PLAYER_LIFE = 1000;

        private static Color PLAYER_COLOR = Color.White;
        //private static uint MAX_ATTACHMENTS = 3;

        private int m_credits;

        public PlayerShip()
        {
        }

        public PlayerShip(float x, float y, long time)
            : base(x, y, PLAYER_LIFE, time)
        {
            m_color = PLAYER_COLOR;
            m_credits = 0;
            m_spriteImage = PLAYER_SPRITE;
            m_spriteOffset = new Vector2(m_spriteImage.Width * 0.5f, m_spriteImage.Height * 0.5f);
            m_rotation = 0;
        }

        public override void AddAttachment(Attachment attachment)
        {
            /*//Max 3 roll off oldest
            if (m_attachments.Count >= MAX_ATTACHMENTS)
            {
                m_attachments.RemoveAt(0);
            }
            m_attachments.Add(attachment);
            */

            /*//Swap each pickup
            m_attachments.Clear();
            m_attachments.Add(attachment);
             */ 
            /*//Keep one of each
            if (!m_currentAttachments.isSet(attachment.GetAttachType()))
            {
                m_currentAttachments.SetField(attachment.GetAttachType());
                m_attachments.Add(attachment);
            }
            */

            m_attachments.Add(attachment);
        }

        public override void Move(long ttms)
        {
        }

        public void SetTouched(bool isTouched)
        {
            foreach (Attachment attachment in m_attachments)
            {
                attachment._touched = isTouched;
                attachment._isActive = isTouched;
            }
            _touched = isTouched;
        }

        public int _credits
        {
            get
            {
                return m_credits;
            }

            set
            {
                m_credits = value;
            }
        }

        public bool HasAttachment(uint type)
        {
            foreach (Attachment a in m_attachments)
            {
                if (m_currentAttachments.isSet(type))
                    return true;
            }

            return false;
        }

        public List<Attachment> GetAttacments()
        {
            return m_attachments;
        }

        public Attachment GetAttachmentByType(uint type)
        {
            foreach (Attachment a in m_attachments)
            {
                if (a.GetAttachType() == type)
                    return a;
            }

            return null;
        }

        private void renderPlayerProjectiles(SpriteBatch sb)
        {
            foreach (Projectile p in GetProjectiles())
            {
                p.Render(sb);
            }
        }

        public override void Update(long ttms)
        {
            for (int i = m_attachments.Count - 1; i >= 0; i--)
            {
                m_attachments[i].Update(ttms);
                if (m_attachments[i].AmDead())
                {
                    m_currentAttachments.ClearField(m_attachments[i].GetAttachType());
                    m_attachments.RemoveAt(i);
                }
            }

            base.Update(ttms);
        }

        public override void Render(SpriteBatch sb)
        {
            sb.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            sb.Draw(m_spriteImage, m_position, null, m_color, m_rotation,
                 m_spriteOffset, 1.0f, SpriteEffects.None, 0);

            renderPlayerProjectiles(sb);
            renderAttachments(sb);
            sb.End();
        }
    }
}
