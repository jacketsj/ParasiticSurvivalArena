using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ParasiticSurvivalArena
{
    public class Larva
    {
        public float x;
        public float y;
        public int width;
        public int height;
        public Vector3 vel; //z coordinate represents rotation
        public float drag;
        public Texture2D tex;
        public float rot;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="vel">X and Y are positional velocity, Z is rotational velocity.</param>
        /// <param name="drag"></param>
        /// <param name="tex"></param>
        /// <param name="rot"></param>
        public Larva(float x, float y, int width, int height, Vector3 vel, float drag, Texture2D tex, float rot)
        {
            this.x = x;
            this.y = y;
            this.vel = vel;
            this.drag = drag;
            this.tex = tex;
            this.rot = rot;
            this.width = width;
            this.height = height;

        }

        public void Update(TimeSpan timePassed)
        {
            Move(vel, timePassed);
            rot += (int)vel.Z * timePassed.Milliseconds;

            Drag();
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
            if (vel.Z >= drag / 5)
            {
                vel.Z -= drag / 5;
            }
            else if (vel.Z <= -drag / 5)
            {
                vel.Z += drag / 5;
            }
            else
            {
                vel.Z = 0;
            }
        }

        public void Move(Vector3 vel, TimeSpan timePassed)
        {
            x += (vel.X * timePassed.Milliseconds) * 0.15f;
            y += (vel.Y * timePassed.Milliseconds) * 0.15f;
            rot += vel.Z;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(tex, new Rectangle((int)x, (int)y, width, height), null, Color.White, rot, new Vector2(width / 2, height / 2), SpriteEffects.None, 0.0f);
        }
    }
}
