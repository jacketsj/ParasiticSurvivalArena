using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ParasiticSurvivalArena
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ParasiticSurvivalArena : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        List<Particle> particles;
        List<BloodSpewer> bloodSpewers;
        List<Launcher> launchers;
        List<Larva> larvae;
        List<ScoreGet> scoreGets;
        Pointer poin;

        int ScreenSizeX = 512;
        int ScreenSizeY = 288;

        SpriteFont largeFont;
        SpriteFont defaultFont;

        bool ShotStuck;

        GameState gameState = GameState.LOGO;

        Color BGColor;

        TimeSpan SpawnTimer;
        TimeSpan ReloadTime;
        TimeSpan MaxLifeSpan;
        float drag;
        int wave;
        int waveRemain;

        SoundEffect cry1;
        SoundEffect cry2;
        SoundEffect cry3;
        SoundEffect caw1;
        SoundEffect caw2;
        SoundEffect caw3;
        SoundEffect dis;

        Logo Intro;

        bool togglingFullscreen = false;

        //Score
        int Score = 0;

        public static Random random = new Random();

        public ParasiticSurvivalArena()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = ScreenSizeY;
            graphics.PreferredBackBufferWidth = ScreenSizeX;
            graphics.ApplyChanges();
            //graphics.ToggleFullScreen();
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            ResetGame();
            Intro = new Logo(Content.Load<Texture2D>("IntroBG"), "JACKETSJ", "presents", new TimeSpan(0, 0, 0, 3, 600), 
                new Logo(Content.Load<Texture2D>("TutBG"), "", "", new TimeSpan(0, 0, 0, 8, 400), 
                    new Logo(Content.Load<Texture2D>("TitleBG"), "PARASITIC SURVIVAL", "ARENA", new TimeSpan(0, 0, 4), null,
                        LogoType.FadeInFadeOutText, Content.Load<SpriteFont>("Large"), Content.Load<SpriteFont>("Default"), false),
                    LogoType.FadeInFadeOutText, Content.Load<SpriteFont>("Default"), Content.Load<SpriteFont>("Default"), true),
                    LogoType.SwipeText, Content.Load<SpriteFont>("Baveuse"), Content.Load<SpriteFont>("Default"), true);

            base.Initialize();
        }

        private void ResetGame()
        {
            particles = new List<Particle>();
            bloodSpewers = new List<BloodSpewer>();
            launchers = new List<Launcher>();
            larvae = new List<Larva>();
            scoreGets = new List<ScoreGet>();
            ReloadTime = new TimeSpan(0, 0, 0, 0, 740);
            MaxLifeSpan = new TimeSpan(0, 0, 16);
            drag = 0.15f;
            launchers.Add(new Launcher(ScreenSizeX / 2, ScreenSizeY / 2, 16, 16, 0, drag, Vector2.Zero, Vector2.Zero, Content.Load<Texture2D>("Launcher"), ReloadTime, MaxLifeSpan));
            poin = new Pointer(0, 0);
            wave = 0;
            waveRemain = 1;
            MediaPlayer.IsRepeating = true;
            int musicChoice = random.Next(0, 2);
            if (musicChoice == 0)
            {
                MediaPlayer.Play(Content.Load<Song>("masic"));
            }
            else if (musicChoice == 1)
            {
                MediaPlayer.Play(Content.Load<Song>("mbsic"));
            }
            ShotStuck = false;
            BGColor = new Color(181, 162, 108);
            SpawnTimer = new TimeSpan(0, 0, 6);
            Score = 0;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            Particle.tex = Content.Load<Texture2D>("Particle");
            largeFont = Content.Load<SpriteFont>("Large");
            defaultFont = Content.Load<SpriteFont>("Default");
            SoundEffect.MasterVolume = 0.15f;
            cry1 = Content.Load<SoundEffect>("cry1");
            cry2 = Content.Load<SoundEffect>("cry2");
            cry3 = Content.Load<SoundEffect>("cry3");
            caw1 = Content.Load<SoundEffect>("caw1");
            caw2 = Content.Load<SoundEffect>("caw2");
            caw3 = Content.Load<SoundEffect>("caw3");
            dis = Content.Load<SoundEffect>("dis");
            //graphics.GraphicsDevice.Viewport = new Viewport(0, 0, 512, 288);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            TimeSpan timePassed = gameTime.ElapsedGameTime;

            KeyboardState ks = Keyboard.GetState();
            MouseState ms = Mouse.GetState();

            // Allows the game to exit
            if (ks.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            //Fullscreen
            if (ks.IsKeyDown(Keys.F11) && !togglingFullscreen)
            {
                togglingFullscreen = true;
                if (graphics.IsFullScreen)
                {
                    graphics.PreferredBackBufferWidth = ScreenSizeX;
                    graphics.PreferredBackBufferHeight = ScreenSizeY;
                }
                else if (!graphics.IsFullScreen)
                {
                    graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                    graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                }
                graphics.ToggleFullScreen();
            }
            if (ks.IsKeyUp(Keys.F11))
            {
                togglingFullscreen = false;
            }

            // Updaterino
            if (gameState == GameState.INGAME)
            {
                poin.Update(ms);

                List<Launcher> DeleteLauncher = new List<Launcher>();

                bool canShoot = !ShotStuck;

                List<BloodSpewer> DeleteSpewer = new List<BloodSpewer>();

                bool moving = false;
                foreach (Launcher launcher in launchers)
                {
                    if (ms.LeftButton == ButtonState.Pressed)
                    {
                        launcher.MoveAwayFrom(poin, true);
                        moving = true;
                    }
                    else if (ms.LeftButton == ButtonState.Released)
                    {
                        launcher.MoveAwayFrom(poin, false);
                    }
                    if (ms.RightButton == ButtonState.Pressed && canShoot)
                    {
                        launcher.Shoot(larvae, Content.Load<Texture2D>("Larva"), random);
                        ShotStuck = true;
                    }
                    else if (ms.RightButton == ButtonState.Released)
                    {
                        ShotStuck = false;
                    }
                    launcher.Update(timePassed, poin);

                    bool killed = false;
                    foreach (BloodSpewer spewer in bloodSpewers)
                    {
                        if ((new Rectangle((int)launcher.x, (int)launcher.y, launcher.width, launcher.height)).Intersects(new Rectangle((int)spewer.x, (int)spewer.y, spewer.width, spewer.height)))
                        {
                            DeleteSpewer.Add(spewer);
                            killed = true;
                        }
                    }

                    if (launcher.x < 0 || launcher.x > ScreenSizeX || launcher.y < 0 || launcher.y > ScreenSizeY || killed || launcher.LifeSpan >= launcher.MaxLifeSpan)
                    {
                        DeleteLauncher.Add(launcher);
                    }
                }
                if (moving)
                {
                    int partNo = (int)(0.2f * timePassed.Milliseconds);
                    for (int i = 0; i < partNo; i++)
                    {
                        particles.Add(new Particle(poin.x, poin.y, new Vector2((float)Math.Cos(Math.PI / random.NextDouble() * Math.PI * 2f), (float)Math.Sin(Math.PI / random.NextDouble() * Math.PI * 2f)) * random.Next(80, 140), random.Next(1, 24) - 0.5f, new Color(90, 110, 250, 140), new TimeSpan(0, 0, 0, 0, 500)));
                    }
                }

                foreach (Launcher launcher in DeleteLauncher)
                {
                    launchers.Remove(launcher);
                    int partNo = (int)(2.1f * timePassed.Milliseconds);
                    for (int i = 0; i < partNo; i++)
                    {
                        dis.Play();
                        particles.Add(new Particle(launcher.x, launcher.y, new Vector2((float)Math.Cos(Math.PI / random.NextDouble() * Math.PI * 2f), (float)Math.Sin(Math.PI / random.NextDouble() * Math.PI * 2f)) * random.Next(80, 140), random.Next(1, 24) - 0.5f, Color.LimeGreen, new TimeSpan(0, 0, 0, 0, 500)));
                    }
                }

                List<Particle> DeleteParticle = new List<Particle>();

                foreach (Particle particle in particles)
                {
                    particle.Update(timePassed);

                    if (particle.x < 0 || particle.x > ScreenSizeX || particle.y < 0 || particle.y > ScreenSizeY || (particle.vel.X == 0 && particle.vel.Y == 0) || particle.age >= particle.maxAge)
                    {
                        DeleteParticle.Add(particle);
                    }
                }

                foreach (Particle particle in DeleteParticle)
                {
                    particles.Remove(particle);
                }

                SpawnTimer -= timePassed;
                if (SpawnTimer <= TimeSpan.Zero)
                {
                    int spawnSide = random.Next(1, 5);
                    int spawnX = 0;
                    int spawnY = 0;
                    if (spawnSide == 1)
                    {
                        spawnX = (random.Next(ScreenSizeX - 16)) + 8;
                        spawnY = 8;
                    }
                    else if (spawnSide == 2)
                    {
                        spawnX = ScreenSizeX - 8;
                        spawnY = (random.Next(ScreenSizeY - 16)) + 8;
                    }
                    else if (spawnSide == 3)
                    {
                        spawnX = (random.Next(ScreenSizeX - 16)) + 8;
                        spawnY = ScreenSizeY - 8;
                    }
                    else if (spawnSide == 4)
                    {
                        spawnX = 8;
                        spawnY = (random.Next(ScreenSizeY - 16)) + 8;
                    }
                    bloodSpewers.Add(new BloodSpewer(spawnX, spawnY, 16, 16, 0.0f, 300, drag, 50, Vector2.Zero, Vector2.Zero, Content.Load<Texture2D>("BloodSpewer")));
                    waveRemain -= 1;
                    if (waveRemain <= 0)
                    {
                        wave += 1;
                        SpawnTimer = new TimeSpan(0, 0, random.Next(3, 10));
                        waveRemain = wave * 2;
                        if (waveRemain > 40)
                        {
                            waveRemain = 30;
                        }
                    }
                    else
                    {
                        SpawnTimer = new TimeSpan(0, 0, 0, 0, random.Next(200 / wave, 1999 / wave));
                    }
                }

                List<Larva> DeleteLarvae = new List<Larva>();

                List<Vector2> targets = new List<Vector2>();
                foreach (Launcher launcher in launchers)
                {
                    targets.Add(new Vector2(launcher.x + launcher.width / 2, launcher.y + launcher.height / 2));
                }
                foreach (BloodSpewer spewer in bloodSpewers)
                {
                    spewer.Update(timePassed, targets, particles);
                    foreach (Larva larva in larvae)
                    {
                        if ((new Rectangle((int)larva.x, (int)larva.y, larva.width, larva.height)).Intersects(new Rectangle((int)spewer.x, (int)spewer.y, spewer.width, spewer.height)))
                        {
                            if (!DeleteSpewer.Contains(spewer))
                            {
                                DeleteSpewer.Add(spewer);
                            }
                            DeleteLarvae.Add(larva);
                            launchers.Add(new Launcher(spewer.x, spewer.y, spewer.width, spewer.height, spewer.rot, drag, spewer.vel, spewer.accel, Content.Load<Texture2D>("Launcher"), ReloadTime, MaxLifeSpan));
                        }
                    }
                }
                bool disMore = false;
                foreach (BloodSpewer spewer in DeleteSpewer)
                {
                    bloodSpewers.Remove(spewer);
                    Score += wave * 10;
                    scoreGets.Add(new ScoreGet(wave * 10, new Vector2(spewer.x, spewer.y - 16), new TimeSpan(0, 0, 0, 0, 1400), Color.Black, defaultFont));
                    disMore = true;
                }
                if (disMore)
                {
                    int chosenOne = random.Next(1, 3);
                    if (chosenOne == 1)
                    {
                        caw1.Play();
                    }
                    else if (chosenOne == 2)
                    {
                        caw2.Play();
                    }
                    else if (chosenOne == 3)
                    {
                        caw3.Play();
                    }
                }

                bool cryMore = false;
                foreach (Larva larva in larvae)
                {
                    larva.Update(timePassed);

                    if (larva.x < 0 || larva.x > ScreenSizeX || larva.y < 0 || larva.y > ScreenSizeY || (larva.vel.X == 0 && larva.vel.Y == 0))
                    {
                        if (!DeleteLarvae.Contains(larva))
                        {
                            DeleteLarvae.Add(larva);
                        }
                    }
                }

                foreach (Larva larva in DeleteLarvae)
                {
                    larvae.Remove(larva);
                    for (int i = 0; i < 40; i++)
                    {
                        cryMore = true;
                        particles.Add(new Particle(larva.x, larva.y, new Vector2((float)Math.Cos(Math.PI / random.NextDouble() * Math.PI * 2f), (float)Math.Sin(Math.PI / random.NextDouble() * Math.PI * 2f)) * random.Next(70, 190), random.Next(1, 12) - 0.5f, Color.DarkRed, new TimeSpan(0, 0, 0, 0, random.Next(700, 1500))));
                    }
                }
                if (cryMore)
                {
                    int soundChosen = random.Next(1, 4);
                    if (soundChosen == 1)
                    {
                        cry1.Play();
                    }
                    else if (soundChosen == 2)
                    {
                        cry2.Play();
                    }
                    else if (soundChosen == 3)
                    {
                        cry3.Play();
                    }
                }

                List<ScoreGet> DeleteSG = new List<ScoreGet>();
                foreach (ScoreGet sg in scoreGets)
                {
                    sg.Update(timePassed);
                    if (sg.lifeTime >= sg.totalLifeTime)
                    {
                        DeleteSG.Add(sg);
                    }
                }
                foreach (ScoreGet sg in DeleteSG)
                {
                    scoreGets.Remove(sg);
                }

                if (launchers.Count <= 0)
                {
                    gameState = GameState.GAMEOVER;
                    MediaPlayer.Stop();
                    MediaPlayer.IsRepeating = false;
                    int rand = random.Next(0, 2);
                    if (rand == 0)
                    {
                        MediaPlayer.Play(Content.Load<Song>("GO1"));
                    }
                    else if (rand == 1)
                    {
                        MediaPlayer.Play(Content.Load<Song>("Chaos"));
                    }
                }
            }
            else if (gameState == GameState.GAMEOVER)
            {
                if (ks.IsKeyDown(Keys.Space))
                {
                    gameState = GameState.INGAME;
                    ResetGame();
                }
            }
            else if (gameState == GameState.LOGO)
            {
                Intro.Update(timePassed);
                if (Intro.screenTime >= Intro.totalScreenTime)
                {
                    gameState = GameState.INGAME;
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Finds the amount by which the screen should be scaled. (Uses black bars)
        /// </summary>
        /// <param name="gameW">The game's</param>
        /// <param name="gameH"></param>
        /// <param name="screenW"></param>
        /// <param name="screenH"></param>
        /// <returns></returns>
        private static Vector2 FindScreenScale(int gameW, int gameH, int screenW, int screenH)
        {
            return new Vector2(screenW / gameW * 1.25f, screenH / gameH * 1.25f);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (gameState == GameState.INGAME)
            {
                GraphicsDevice.Clear(BGColor);
            }
            else if (gameState == GameState.GAMEOVER || gameState == GameState.LOGO)
            {
                GraphicsDevice.Clear(Color.DarkGray);
            }
            // Drawing code
            if (graphics.IsFullScreen)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Matrix.CreateScale(new Vector3(FindScreenScale(ScreenSizeX, ScreenSizeY, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height), 1.0f)));
            }
            else
            {
                spriteBatch.Begin();
            }
            if (gameState == GameState.INGAME)
            {
                spriteBatch.Draw(Content.Load<Texture2D>("Arena"), Vector2.Zero, Color.White);
                spriteBatch.DrawString(largeFont, Score.ToString(), new Vector2(ScreenSizeX / 2, ScreenSizeY / 2), Color.DarkBlue, 0.0f, largeFont.MeasureString(Score.ToString()) / 2, 1.0f, SpriteEffects.None, 0.0f);
                foreach (Particle particle in particles)
                {
                    particle.Draw(spriteBatch);
                }
                foreach (BloodSpewer spewer in bloodSpewers)
                {
                    spewer.Draw(spriteBatch);
                }
                spriteBatch.Draw(Content.Load<Texture2D>("SpawnRing"), Vector2.Zero, Color.White);
                foreach (Larva larva in larvae)
                {
                    larva.Draw(spriteBatch);
                }
                foreach (Launcher launcher in launchers)
                {
                    launcher.Draw(spriteBatch);
                }
                foreach (ScoreGet sg in scoreGets)
                {
                    sg.Draw(spriteBatch);
                }
                spriteBatch.Draw(Content.Load<Texture2D>("Crosshair"), new Vector2(poin.x - 8, poin.y - 8), Color.White);
            }
            else if (gameState == GameState.GAMEOVER)
            {
                spriteBatch.DrawString(largeFont, "Game Over", new Vector2(ScreenSizeX, ScreenSizeY) / 2, Color.Black, 0.0f, largeFont.MeasureString("Game Over") / 2, 1.0f, SpriteEffects.None, 0.0f);
                spriteBatch.DrawString(defaultFont, "Press space to try again.", (new Vector2(ScreenSizeX, ScreenSizeY * 1.2f) / 2), Color.Black, 0.0f, defaultFont.MeasureString("Press space to try again.") / 2, 1.0f, SpriteEffects.None, 0.0f);
                spriteBatch.DrawString(defaultFont, "Your Score: " + Score.ToString(), (new Vector2(ScreenSizeX, ScreenSizeY * 1.2f + defaultFont.MeasureString("Press space to try again.").Y * 2) / 2), Color.Black, 0.0f, defaultFont.MeasureString("Your Score: " + Score.ToString()) / 2, 1.0f, SpriteEffects.None, 0.0f);
            }
            else if (gameState == GameState.LOGO)
            {
                Intro.Draw(spriteBatch, ScreenSizeX, ScreenSizeY);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }

    public enum GameState
    {
        INGAME, GAMEOVER, LOGO
    }
}
