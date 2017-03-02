using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace ParasiticSurvivalArena
{
    public class Pointer
    {
        public int x;
        public int y;

        public Pointer(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Pointer() { }

        public void Update(MouseState ms)
        {
            x = ms.X;
            y = ms.Y;
        }
    }
}
