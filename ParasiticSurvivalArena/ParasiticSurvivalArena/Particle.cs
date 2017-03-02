using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ParasiticSurvivalArena
{
    public class Particle
    {
        public float x;
        public float y;
        public Vector2 vel;
        public float drag;
        public Color col;
        public static Texture2D tex;
        public TimeSpan age;
        public TimeSpan maxAge;

        public Particle(float x, float y, Vector2 vel, float drag, Color col, TimeSpan maxAge)
        {
            this.x = x;
            this.y = y;
            this.vel = vel;
            this.drag = drag;
            this.col = col;
            this.age = new TimeSpan();
            this.maxAge = maxAge;
            if (this.drag < 0)
            {
                this.drag /= -1;
            }
        }

        public Particle() { }

        public void Update(TimeSpan timePassed)
        {
            Drag();
            Move(vel, timePassed);
            age += timePassed;
        }

        public void Drag()
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

        public void Move(Vector2 vel, TimeSpan timePassed)
        {
            x += (float)Math.Cos(Math.Atan2(vel.Y, vel.X)) * timePassed.Milliseconds * 0.06f;
            y += (float)Math.Sin(Math.Atan2(vel.Y, vel.X)) * timePassed.Milliseconds * 0.06f;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(tex, new Vector2(x, y), col);
        }
    }
}
