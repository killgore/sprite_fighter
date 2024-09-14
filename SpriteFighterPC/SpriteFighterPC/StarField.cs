using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#if WINPHONE
using Microsoft.Devices.Sensors;
#endif

namespace SpriteFighter
{
    public class Star : Projectile
    {
        public static Texture2D STAR_SPRITE;

        public enum StarLayer
        {
            fore = 1,
            mid,
            back,
        }

        private StarLayer m_layer;
        private float m_scale;

        public Star(Vector2 pos, Vector2 vel, Color c, long time, StarLayer layer)
            : base(pos, vel, c, 0, time, INF_LIFETIME)
        {
            m_layer = layer;
            _spriteImage = STAR_SPRITE;
            switch (m_layer)
            {
                case StarLayer.fore:
                    {
                        m_scale = 1.0f;
                        break;
                    }
                case StarLayer.mid:
                    {
                        m_scale = 0.5f;
                        break;
                    }
                case StarLayer.back:
                    {
                        m_scale = 0.25f;
                        break;
                    }
                default:
                    {
                        m_scale = 1.0f;
                        break;
                    }
            }
        }

        public override void Render(SpriteBatch sb)
        {
            sb.Draw(m_spriteImage, m_position, null, m_color, m_rotation,
                 m_spriteOffset, m_scale, SpriteEffects.None, 1);
        }

        public override void Update(long ttms)
        {
            base.Update(ttms);
        }
    }

    public class StarField
    {
        private const int FOREGROUND_COUNT = 15;
        private const int MIDGROUND_COUNT = 20;
        private const int BACKGROUND_COUNT = 35;

        private const int MAX_ACCEL_X = 10;
        private const int MAX_ACCEL_Y = 10;

        private Vector2 FOREGROUND_VELOCITY = new Vector2(0, 8);
        private Vector2 MIDGROUND_VELOCITY = new Vector2(0, 4);
        private Vector2 BACKGROUND_VELOCITY = new Vector2(0, 2);

        private List<Star> m_stars;
        private Rectangle m_area;

        private static float accel_x;
        private static float accel_y;

        public StarField(Point screenSize)
        {
            m_stars = new List<Star>();
            m_area = new Rectangle(-25, -25, screenSize.X + 25, screenSize.Y + 25);
            accel_x = 0;
            accel_y = 0;
        }

        public void InitStarField()
        {
            float x = 0;
            float y = 0; 
            Star star = null;

            for (int i = 0; i < FOREGROUND_COUNT; i++)
            {
                x = (float)(m_area.Right * Game1.random.NextDouble());
                y = (float)(m_area.Bottom * Game1.random.NextDouble());

                star = new Star(new Vector2(x, y), FOREGROUND_VELOCITY, Color.White, 0, Star.StarLayer.fore);

                m_stars.Add(star);
            }

            for (int i = 0; i < MIDGROUND_COUNT; i++)
            {
                x = (float)(m_area.Right * Game1.random.NextDouble());
                y = (float)(m_area.Bottom * Game1.random.NextDouble());

                star = new Star(new Vector2(x, y), MIDGROUND_VELOCITY, Color.White, 0, Star.StarLayer.mid);

                m_stars.Add(star);
            }

            for (int i = 0; i < BACKGROUND_COUNT; i++)
            {
                x = (float)(m_area.Right * Game1.random.NextDouble());
                y = (float)(m_area.Bottom * Game1.random.NextDouble());

                star = new Star(new Vector2(x, y), BACKGROUND_VELOCITY, Color.White, 0, Star.StarLayer.back);

                m_stars.Add(star);
            }
        }

        public void Update(long ttms)
        {
            foreach (Star star in m_stars)
            {
                float pos_x = star._position.X;
                float pos_y = star._position.Y;
                if( pos_y < m_area.Bottom )
                {
                    if (pos_x < m_area.Left)
                    {
                        pos_x = m_area.Right + accel_x * MAX_ACCEL_X;
                    }
                    else if (pos_x > m_area.Right)
                    {
                        pos_x = m_area.Left + accel_x * MAX_ACCEL_X;
                    }
                    else
                    {
                        pos_x += accel_x * MAX_ACCEL_X;
                    }
                    star._position = new Vector2(pos_x , pos_y);
                    star.Move(ttms);
                }
                else
                {
                    float x = (float)(m_area.Right * Game1.random.NextDouble());

                    star._position = new Vector2(x, m_area.Top);
                    return;
                }
            }
        }

        public void Render(SpriteBatch sb)
        {
            foreach (Star star in m_stars)
            {
                star.Render(sb);
            }
        }

        #region Accelerometer Event Handling
        /// <summary>
        /// The event handler for the accelerometer ReadingChanged event.
        /// BeginInvoke is used to pass this event args object to the UI thread.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
#if WINPHONE 
        public static void accelerometer_ReadingChanged(object sender, AccelerometerReadingEventArgs e)
        {
            if (Game1.accelerometer != null)
            {
                accel_x = (float)e.X;
                accel_y = (float)e.Y;
            }
            //Deployment.Current.Dispatcher.BeginInvoke(() => MyReadingChanged(e));
        }
#endif
/*
        /// <summary>
        /// Method for handling the ReadingChanged event on the UI thread.
        /// This sample just displays the reading value.
        /// </summary>
        /// <param name="e"></param>
        void MyReadingChanged(AccelerometerReadingEventArgs e)
        {
            if (Game1.accelerometer != null)
            {
                statusTextBlock.Text = Game1.accelerometer.State.ToString();
                XTextBlock.Text = e.X.ToString("0.00");
                YTextBlock.Text = e.Y.ToString("0.00");
                ZTextBlock.Text = e.Z.ToString("0.00");
            }
        }
*/
        #endregion
    }
}
