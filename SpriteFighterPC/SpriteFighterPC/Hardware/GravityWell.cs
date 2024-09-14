using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace SpriteFighter
{
    public class GravityWell : GunAttachment
    {
        public static Texture2D GRAVITY_WELL_SPRITE;
        public static SoundEffect GRAVITY_WELL_SOUND;

        static int GRAVITY_WELL_RATE = 3000;
        static Vector2 GRAVITY_WELL_SPEED = new Vector2(0, -5);
        static int GRAVITY_WELL_DAMAGE = 500;
        static Color GRAVITY_WELL_COLOR = Color.DarkSlateBlue;
        static int GRAVITY_CONSTANT = 1000;

        List<EnemyShip> m_victims;

        public GravityWell(Vector2 offset, GameEntity attachedTo, List<EnemyShip> victims, long time)
            : base(GRAVITY_WELL_RATE, GRAVITY_WELL_SPEED, GRAVITY_WELL_DAMAGE, offset, attachedTo, GRAVITY_WELL_COLOR, time, GunAttachment.GravityWell)
        {
            _spriteImage = GRAVITY_WELL_SPRITE;
            m_projectileSpriteImage = Projectile.PROJECTILE_SPRITE;
            m_soundEffect = GRAVITY_WELL_SOUND;
            m_victims = victims;
        }

        public GravityWell()
        {
        }

        public override void Update(long ttms)
        {
            foreach (SpaceShip ss in m_victims)
            {
                foreach (Projectile gwp in m_projectiles)
                {
                    float distance = Vector2.Distance(ss._position, gwp._position);
                    Vector2 path = Vector2.Normalize(gwp._position - ss._position);
                    Vector2 posAdjust = (path * (GRAVITY_CONSTANT / distance));
                    Vector2 newPos = ss._position + posAdjust;
                    ss._position = newPos;
                }
            }
            base.Update(ttms);
        }

        protected override void generateProjectile(long ttms)
        {
            float x = m_parent._position.X + m_offset.X;
            float y = m_parent._position.Y + m_offset.Y;
            Vector2 startPosition = new Vector2(x, y);
            GravityWellProjectile p = new GravityWellProjectile(startPosition, GRAVITY_WELL_SPEED, m_color, m_projectileDamage, ttms);
            m_projectiles.Add(p);
        }

        protected override void checkGenerateProjectile(long ttms)
        {
            if (canFire(ttms))
            {
                generateProjectile(ttms);
                m_lastFireTime = ttms;
                PlaySoundEffect();
            }
        }

        public void SetVictims(List<EnemyShip> victims)
        {
            m_victims = victims;
        }
    }
}
