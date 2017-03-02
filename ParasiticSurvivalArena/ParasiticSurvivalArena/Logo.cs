using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ParasiticSurvivalArena
{
    public class Logo
    {
        public Texture2D bg;
        public string centerText;
        public string subText;
        public TimeSpan screenTime;
        public TimeSpan totalScreenTime;
        private float textAlt;
        public Logo followUp;
        public LogoType lType;
        public SpriteFont sf;
        public SpriteFont subf;
        bool follow = false;

        public Logo(Texture2D bg, string centerText, string subText, TimeSpan screenTime, Logo followUp, LogoType lType, SpriteFont sf, SpriteFont subf, bool follow)
        {
            this.bg = bg;
            this.centerText = centerText;
            this.subText = subText;
            this.screenTime = TimeSpan.Zero;
            this.totalScreenTime = screenTime;
            this.followUp = followUp;
            this.lType = lType;
            this.sf = sf;
            this.subf = subf;
            this.follow = follow;
        }

        public void Update(TimeSpan timePassed)
        {
            screenTime += timePassed;
            textAlt = totalScreenTime.Seconds - screenTime.Seconds;
            if (screenTime >= totalScreenTime && followUp != null && follow)
            {
                this.bg = followUp.bg;
                this.centerText = followUp.centerText;
                this.subText = followUp.subText;
                this.screenTime = TimeSpan.Zero;
                this.totalScreenTime = followUp.totalScreenTime;
                this.lType = followUp.lType;
                this.sf = followUp.sf;
                if (followUp.follow)
                {
                    this.follow = followUp.follow;
                    this.followUp = followUp.followUp;
                }
                else
                {
                    this.follow = false;
                }
            }
        }

        public void Draw(SpriteBatch sb, int ScreenSizeX, int ScreenSizeY)
        {
            sb.Draw(bg, new Rectangle(0, 0, ScreenSizeX, ScreenSizeY), Color.White);
            if (lType == LogoType.FadeInFadeOutText)
            {
                sb.DrawString(sf, centerText, new Vector2(ScreenSizeX / 2 - (0.5f * sf.MeasureString(centerText).X), ScreenSizeY / 2 - (0.5f * sf.MeasureString(centerText).Y)), new Color(0, 0, 0, 255 - (textAlt - totalScreenTime.Seconds / 2)));
                sb.DrawString(sf, subText, new Vector2(ScreenSizeX / 2 - (0.5f * sf.MeasureString(subText).X), ScreenSizeY / 2 + ScreenSizeY / 8 - (0.5f * sf.MeasureString(subText).Y)), new Color(0, 0, 0, 255 - (textAlt - totalScreenTime.Seconds / 2)));
            }
            else if (lType == LogoType.SwipeText)
            {
                sb.DrawString(sf, centerText, new Vector2(ScreenSizeX / 2 - (textAlt - totalScreenTime.Seconds / 2) - (0.5f * sf.MeasureString(centerText).X), ScreenSizeY / 2 - (0.5f * sf.MeasureString(centerText).Y)), Color.Black);
                sb.DrawString(sf, subText, new Vector2(ScreenSizeX / 2 - (textAlt - totalScreenTime.Seconds / 2) - (0.5f * sf.MeasureString(subText).X), ScreenSizeY / 2 + ScreenSizeY / 8 - (0.5f * sf.MeasureString(subText).Y)), Color.Black);
            }
        }
    }

    public enum LogoType
    {
        FadeInFadeOutText, SwipeText
    }
}
