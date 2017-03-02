using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ParasiticSurvivalArena
{
    public class ScoreGet
    {
        public string score;
        public Vector2 pos;
        public TimeSpan lifeTime;
        public TimeSpan totalLifeTime;
        public Color colour;
        public SpriteFont sf;

        public ScoreGet(string score, Vector2 pos, TimeSpan lifeTime, Color colour, SpriteFont sf)
        {
            this.score = score;
            this.pos = pos;
            this.totalLifeTime = lifeTime;
            this.lifeTime = TimeSpan.Zero;
            this.colour = colour;
            this.sf = sf;
        }

        public ScoreGet(int score, Vector2 pos, TimeSpan lifeTime, Color colour, SpriteFont sf)
        {
            this.score = score.ToString();
            this.pos = pos;
            this.totalLifeTime = lifeTime;
            this.lifeTime = TimeSpan.Zero;
            this.colour = colour;
            this.sf = sf;
        }

        public ScoreGet(string score, float x, float y, TimeSpan lifeTime, Color colour, SpriteFont sf)
        {
            this.score = score;
            this.pos = new Vector2(x, y);
            this.totalLifeTime = lifeTime;
            this.lifeTime = TimeSpan.Zero;
            this.colour = colour;
            this.sf = sf;
        }

        public ScoreGet(int score, float x, float y, TimeSpan lifeTime, Color colour, SpriteFont sf)
        {
            this.score = score.ToString();
            this.pos = new Vector2(x, y);
            this.totalLifeTime = lifeTime;
            this.lifeTime = TimeSpan.Zero;
            this.colour = colour;
            this.sf = sf;
        }

        public void Update(TimeSpan timePassed)
        {
            lifeTime += timePassed;

            pos.Y -= ((float)timePassed.Milliseconds) / 100f;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.DrawString(sf, score, pos, colour);
        }
    }
}
