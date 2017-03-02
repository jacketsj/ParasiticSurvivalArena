using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ParasiticSurvivalArena
{
    public class Launcher : Entity
    {
        public float rot;
        public TimeSpan Reloading;
        public TimeSpan ReloadTime;
        public TimeSpan LifeSpan;
        public TimeSpan MaxLifeSpan;

        public Launcher(float x, float y, int width, int height, float rot, float drag, Vector2 vel, Vector2 accel, Texture2D tex, TimeSpan ReloadTime, TimeSpan MaxLifeSpan)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.vel = vel;
            this.accel = accel;
            this.tex = tex;
            this.rot = rot;
            this.drag = drag;
            this.ReloadTime = ReloadTime;
            this.Reloading = TimeSpan.Zero;
            this.MaxLifeSpan = MaxLifeSpan;
            this.LifeSpan = TimeSpan.Zero;
        }

        public Launcher() { }

        public void Update(TimeSpan timePassed, Pointer poin)
        {
            if (Reloading >= TimeSpan.Zero)
            {
                Reloading -= timePassed;
            }
            vel += accel * timePassed.Milliseconds;
            Move(vel, timePassed);
            rot = UpdateRot(poin);

            LifeSpan += timePassed;

            Drag(false);
        }

        public void Shoot(List<Larva> larvae, Texture2D larvaTex, Random random)
        {
            if (Reloading <= TimeSpan.Zero)
            {
                larvae.Add(new Larva(x, y, 16, 16, new Vector3((float)Math.Cos(rot + Math.PI / 2) + vel.X * 0.01f, (float)Math.Sin(rot + Math.PI / 2) + vel.Y * 0.01f, (float)Math.PI / 19f), drag / 16, larvaTex, (float)(random.NextDouble() * Math.PI * 2f)));
                Reloading = ReloadTime;
            }
        }

        private float UpdateRot(Pointer poin)
        {
            return (float)(Math.Atan2((y - poin.y) , (x - poin.x)) - Math.PI / 2);
        }

        public new void Draw(SpriteBatch sb)
        {
            int extraSize = 0;
            if (LifeSpan.Seconds >= MaxLifeSpan.Seconds - 6)
            {
                extraSize = (int)((LifeSpan.TotalMilliseconds - MaxLifeSpan.TotalMilliseconds + 6000) / 1000f);
            }
            sb.Draw(tex, new Rectangle((int)x - extraSize, (int)y - extraSize, width + extraSize * 2, height + extraSize * 2), null, Color.White, rot, new Vector2((width / 2), (height / 2)), SpriteEffects.None, 0.0f);
        }
    }
}
