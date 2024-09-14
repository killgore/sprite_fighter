using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace SpriteFighter
{
    class BaseCannon : GunAttachment
    {
        public static Texture2D BASE_CANNON_SPRITE;
        public static SoundEffect BASE_CANNON_SOUND;

        static int BASE_CANNON_RATE = 100;
        static Vector2 BASE_CANNON_SPEED = new Vector2(0, -20);
        static int BASE_CANNON_DAMAGE = 50;
        static Color BASE_CANNON_COLOR = Color.DarkCyan;

        public BaseCannon()
        {
        }

        public BaseCannon(GameEntity attachedTo, long time)
            : base(BASE_CANNON_RATE, BASE_CANNON_SPEED, BASE_CANNON_DAMAGE, new Vector2(0, 0), attachedTo, BASE_CANNON_COLOR, time, GunAttachment.BaseCannon)
        {
            _spriteImage = BASE_CANNON_SPRITE;
            m_soundEffect = BASE_CANNON_SOUND;
        }

        public override void Update(long ttms)
        {
            base.Update(ttms);
        }
    }
}
