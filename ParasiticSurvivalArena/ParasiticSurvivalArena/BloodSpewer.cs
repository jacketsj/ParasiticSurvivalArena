using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ParasiticSurvivalArena
{
    public class BloodSpewer : Entity
    {
        public float rot;
        public static Random random = new Random();
        TimeSpan PulseCounter;
        TimeSpan pulseFreq;
        public float partSpeed;
        public float partAccel;
        public float maxDist;
        public float maxVel;

        public BloodSpewer(int x, int y, int width, int height, float rot, float maxDist, float drag, float maxVel, Vector2 vel, Vector2 accel, Texture2D tex)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.vel = vel;
            this.accel = accel;
            this.tex = tex;
            this.rot = rot;
            this.maxDist = maxDist;
            this.maxVel = maxVel;
            PulseCounter = new TimeSpan();
            pulseFreq = new TimeSpan(0, 0, 0, 0, 700);
            partSpeed = 10;
            partAccel = 2;
        }

        public BloodSpewer() { }

        public void Update(TimeSpan timePassed, List<Vector2> targets, List<Particle> particles)
        {
            vel += accel * timePassed.Milliseconds;
            if (vel.X > maxVel)
            {
                vel.X = maxVel;
            }
            if (vel.X < -maxVel)
            {
                vel.X = -maxVel;
            }
            if (vel.Y > maxVel)
            {
                vel.Y = maxVel;
            }
            if (vel.Y < -maxVel)
            {
                vel.Y = -maxVel;
            }
            Move(vel, timePassed);
            Vector2 target = DetermineTarget(targets);
            rot = UpdateRot(target);
            accel += new Vector2((float)Math.Cos(rot), (float)Math.Sin(rot)) * 0.001f;

            Drag(false);

            PulseCounter += timePassed;
            while (PulseCounter > pulseFreq)
            {
                PulseCounter -= pulseFreq;
                GenerateParticles(particles);
            }
        }

        public Vector2 DetermineTarget(List<Vector2> targets)
        {
            float lowestDist = maxDist;
            Vector2 lowestDistV = new Vector2(x, y);
            foreach (Vector2 tar in targets)
            {
                float currentDist = (float)Math.Sqrt(Math.Pow(x - tar.X, 2) + Math.Pow(y - tar.Y, 2));
                if (currentDist < lowestDist)
                {
                    lowestDist = currentDist;
                    lowestDistV = tar;
                }
            }
            return lowestDistV;
        }

        private float UpdateRot(Vector2 target)
        {
            return (float)(Math.Atan2(target.Y - y, target.X - x));
        }

        public void GenerateParticles(List<Particle> particles)
        {
            int im = random.Next(4, 9);
            for (int i = 0; i < im; i++)
            {
                particles.Add(new Particle(x, y, (new Vector2((float)Math.Cos(Math.PI / random.NextDouble() * Math.PI * 2f), (float)Math.Sin(Math.PI / random.NextDouble() * Math.PI * 2f)) * random.Next(70, 190)) - (new Vector2((float)Math.Cos(rot) * 30, (float)Math.Sin(rot)) * 30), random.Next(1, 12) - 0.5f, Color.DarkRed, new TimeSpan(0, 0, 0, 0, random.Next(700, 1500))));
            }
        }

        public new void Draw(SpriteBatch sb)
        {
            sb.Draw(tex, new Rectangle((int)x, (int)y, width, height), null, Color.White, rot + (float)(Math.PI / 2), new Vector2(width / 2, height / 2), SpriteEffects.None, 0.0f);
        }
    }
}
