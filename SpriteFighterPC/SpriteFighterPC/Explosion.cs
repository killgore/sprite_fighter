using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace SpriteFighter
{
    public class Explosion : Projectile
    {
        public static Texture2D EXPLOSION_SPRITE;
        public static SoundEffect EXPLOSION_SOUND;

        public Explosion(Vector2 pos, Vector2 vel, Color c, int dmg, long time, int lifetime)
            : base(pos, vel, c, dmg, time, lifetime)
        {
            _spriteImage = EXPLOSION_SPRITE;
            _sound = EXPLOSION_SOUND;
        }

        public override void Render(SpriteBatch sb)
        {
            sb.Draw(m_spriteImage, m_position, null, m_color, m_rotation,
                 m_spriteOffset, 0.5f, SpriteEffects.None, 0);
        }
    }
}
