using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ParasiticSurvivalArena
{
    public class Entity
    {
        public float x;
        public float y;
        public int width;
        public int height;
        public float drag;
        public Vector2 vel;
        public Vector2 accel;
        public Texture2D tex;

        public Entity(int x, int y, int width, int height, float drag, Vector2 vel, Vector2 accel, Texture2D tex)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.vel = vel;
            this.accel = accel;
            this.tex = tex;
            this.drag = drag;
        }

        public Entity() { }

        public void Update(TimeSpan timePassed)
        {
            vel += accel * timePassed.Milliseconds;
            Move(vel, timePassed);

            Drag(false);
        }

        public void Drag(bool UseAccel)
        {
            if (!UseAccel)
            {
                if (vel.X >= drag)
                {
                    vel.X -= drag;
                }
                else if (vel.X <= -drag)
                {
                    vel.X += drag;
                }
                else
                {
                    vel.X = 0;
                }
                if (vel.Y >= drag)
                {
                    vel.Y -= drag;
                }
                else if (vel.Y <= -drag)
                {
                    vel.Y += drag;
                }
                else
                {
                    vel.Y = 0;
                }
            }
            else
            {
                if (accel.X >= drag)
                {
                    accel.X -= drag;
                }
                else if (accel.X <= -drag)
                {
                    accel.X += drag;
                }
                else
                {
                    accel.X = 0;
                }
                if (accel.Y >= drag)
                {
                    accel.Y -= drag;
                }
                else if (accel.Y <= -drag)
                {
                    accel.Y += drag;
                }
                else
                {
                    accel.Y = 0;
                }
            }
        }

        public void Move(Vector2 vel, TimeSpan timePassed)
        {
            x += (vel.X * timePassed.Milliseconds) * 0.001f;
            y += (vel.Y * timePassed.Milliseconds) * 0.001f;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(tex, new Rectangle((int)x, (int)y, width, height), Color.White);
        }

        public void MoveAwayFrom(Pointer poin, bool Enabled)
        {
            if (Enabled && (!(x == poin.x) || !(y == poin.y)))
            {
                double angle = Math.Atan2(y - poin.y, x - poin.x);
                accel.X += 0.3f * (float)Math.Cos(angle) / (float)Math.Sqrt(Math.Pow(x - poin.x, 2) + Math.Pow(y - poin.y, 2));
                accel.Y += 0.3f * (float)Math.Sin(angle) / (float)Math.Sqrt(Math.Pow(x - poin.x, 2) + Math.Pow(y - poin.y, 2));
            }
            else if (!Enabled)
            {
                accel = Vector2.Zero;
            }
        }
    }
}
