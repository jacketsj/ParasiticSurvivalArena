using System;

namespace ParasiticSurvivalArena
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (ParasiticSurvivalArena game = new ParasiticSurvivalArena())
            {
                game.Run();
            }
        }
    }
#endif
}

