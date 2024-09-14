using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace SpriteFighter
{
    public abstract class GameEntity
    {
        protected Vector2 m_position;
        protected Vector2 m_velocity;
        protected float m_rotation;  // in radians
        protected Color m_color;
        protected long m_birthtime;
        protected Texture2D m_spriteImage;
        protected Vector2 m_spriteOffset;
        protected SoundEffect m_soundEffect;

        private bool m_isTouched = false;

        protected bool m_destroyMe = false;
        protected bool m_canDestroy = true;

        public GameEntity()
        {

        }

        public GameEntity(Vector2 pos, Vector2 vel, float rot, Color c, long time)
        {
            m_position = pos;
            m_velocity = vel;
            m_color = c;
            m_birthtime = time;
            m_rotation = rot;
        }

        public Vector2 _position
        {
            get
            {
                return m_position;
            }

            set
            {
                m_position = value;
            }
        }

        public Vector2 _velocity
        {
            get
            {
                return m_velocity;
            }

            set
            {
                m_velocity = value;
            }
        }

        public Color _color
        {
            get
            {
                return m_color;
            }

            set
            {
                m_color = value;
            }
        }

        public float _rotation
        {
            get
            {
                return m_rotation;
            }

            set
            {
                m_rotation = value;
            }
        }

        public Texture2D _spriteImage
        {
            get
            {
                return m_spriteImage;
            }

            set
            {
                m_spriteImage = value;
                _spriteOffset = new Vector2(m_spriteImage.Width * 0.5f, m_spriteImage.Height * 0.5f);
            }
        }

        public SoundEffect _sound
        {
            get
            {
                return m_soundEffect;
            }

            set
            {
                m_soundEffect = value;
            }
        }

        public virtual void PlaySoundEffect()
        {
            if(m_soundEffect != null)
                m_soundEffect.Play();
        }

        public Vector2 _spriteOffset
        {
            get
            {
                return m_spriteOffset;
            }

            set
            {
                m_spriteOffset = value;
            }
        }

        public virtual void ApplyVelocityUpdate()
        {
            m_position = m_position + m_velocity;
        }

        public bool _touched
        {
            get
            {
                return m_isTouched;
            }

            set
            {
                m_isTouched = value;
            }
        }

        public virtual BoundingBox GetBoundingBox()
        {
            int sw = m_spriteImage.Bounds.Width;
            int sh = m_spriteImage.Bounds.Height;
            BoundingBox bb = new BoundingBox(new Vector3(m_position.X - (sw / 2), m_position.Y - (sh / 2), 0), new Vector3(m_position.X + (sw / 2), m_position.Y + (sh / 2), 0));
            return bb;
        }

        public virtual BoundingBox GetBoundingBox(float scale)
        {
            float sw = m_spriteImage.Bounds.Width * scale;
            float sh = m_spriteImage.Bounds.Height * scale;
            BoundingBox bb = new BoundingBox(new Vector3(m_position.X - (sw / 2), m_position.Y - (sh / 2), 0), new Vector3(m_position.X + (sw / 2), m_position.Y + (sh / 2), 0));
            return bb;
        }

        public virtual void DestroyMe()
        {
            if(m_canDestroy)
                m_destroyMe = true;
        }

        public virtual bool AmDead()
        {
            return m_destroyMe;
        }

        public virtual void Move(long ttms)
        {
            ApplyVelocityUpdate();
        }

        public virtual void Update(long ttms)
        {
        }

        public virtual void Render(SpriteBatch sb)
        {
        }

        public virtual void CleanUp(long ttms)
        {
        }

        public virtual void Restore()
        {
        }
    }
}
