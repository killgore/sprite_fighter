using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.IO.IsolatedStorage;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

#if WINPHONE
using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Devices.Sensors;
#endif

namespace SpriteFighter
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        const int ENEMYSHIPTIME = 2000; //ms
        const int EXPLOSIONTIME = 500; //ms
        const int MAX_Y = 600;
        const int MIN_Y = 720;
        const int THUMB_OFFSET = -80;
        const long POWERUP_GENERATE_WAIT = 2000; //ms
        const string ENEMY_FILE = "enemy.dat";
        const string PLAYER_FILE = "player.dat";
        const long GAME_OVER_WAIT = 3000;
        const long LEVEL_COMPLETE_WAIT = 3000;

        public enum GameState
        {
            START_MENU,
            PLAYING,
            LEVEL_COMPLETE,
            STORE,
            PAUSED,
            GAME_OVER,
            EXITING
        }

        GameState m_currentState = GameState.START_MENU;
        
        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public SpriteFont spriteFont;

#if WINPHONE
        public static Accelerometer accelerometer;
#endif

        public static Random random = new Random();

        List<EnemyShip> m_enemyShips;

        Type[] ShipTypes = new Type[] { typeof(EnemyShip), typeof(GruntShip), typeof(SidewinderShip), typeof(JuggernautShip), typeof(StrikerShip) };

        PlayerShip m_playerShip = null;

        Texture2D m_explosionImage;
        Vector2 m_explosionOffset;

        Point m_screenSize;
        Rectangle m_playArea;

        int m_playerPoints;
        int m_highScore = 0;

        long m_gameOverTime;
        long m_levelCompleteTime;

        SoundEffectInstance m_levelMusic;

        long ttms = 0; //tick time in milliseconds
        long pauseTime = 0;
        long pauseOffset = 0;

        const string highScoreTag = "highscore";
        const string playerLifeTag = "playerlife";
        const string playerScoreTag = "playerscore";
        const string enemyShipsTag = "enemyships";
        const string gameStateTag = "gamestate";

        GameLevelManager m_gameLevelManager;
        public LevelCompleteEvent m_levelCompleteEvent = new LevelCompleteEvent();
        NextLevelEvent m_nextLevelEvent = new NextLevelEvent();
        OpenStoreEvent m_openStoreEvent = new OpenStoreEvent();
        BuyHardwareEvent m_buyHardwareEvent = new BuyHardwareEvent();

        FrameRateCounter m_frameCounter;

        GameScreen m_currentScreen;

        Dictionary<uint, Func<PlayerShip, Game1, Point, GameLevel>> m_gameLevels = new Dictionary<uint, Func<PlayerShip, Game1, Point, GameLevel>>();
        uint m_currentLevel = 1;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.SupportedOrientations = DisplayOrientation.Portrait;

            m_screenSize = new Point(480, 800);
            m_playArea = new Rectangle(-100, -100, m_screenSize.X + 200,  m_screenSize.Y + 200);

            graphics.PreferredBackBufferWidth = m_screenSize.X;
            graphics.PreferredBackBufferHeight = m_screenSize.Y;

            Guide.IsScreenSaverEnabled = false;

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            bindEvents();
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
            graphics.IsFullScreen = true;
#if WINPHONE
            if (accelerometer == null)
            {
                // Instantiate the accelerometer sensor object
                accelerometer = new Accelerometer();

                // Add an event handler for the ReadingChanged event.
                accelerometer.ReadingChanged += new EventHandler<AccelerometerReadingEventArgs>(StarField.accelerometer_ReadingChanged);

                // The Start method could throw and exception, so use a try block
                try
                {
                   Trace("starting accelerometer");
                    accelerometer.Start();
                }
                catch (AccelerometerFailedException e)
                {
                    Trace("error starting accelerometer: "+e.Message);
                }
            }
#endif
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            GruntShip.GRUNT_SPRITE = this.Content.Load<Texture2D>(@"Images\EnemyShipMonster");
            SidewinderShip.SIDEWINDER_SPRITE = this.Content.Load<Texture2D>(@"Images\EnemyShip");
            JuggernautShip.JUGGERNAUT_SPRITE = this.Content.Load<Texture2D>(@"Images\JuggernautShip");
            StrikerShip.STRIKER_SPRITE = this.Content.Load<Texture2D>(@"Images\EnemyShip");
            BaseCannon.BASE_CANNON_SPRITE = this.Content.Load<Texture2D>(@"Images\Projectile");
            IonCannon.ION_CANNON_SPRITE = this.Content.Load<Texture2D>(@"Images\Projectile");
            PlayerShip.PLAYER_SPRITE = this.Content.Load<Texture2D>(@"Images\PlayerShip");
            Projectile.PROJECTILE_SPRITE = this.Content.Load<Texture2D>(@"Images\Projectile");
            HealthPack.HEALTH_PACK_SPRITE = this.Content.Load<Texture2D>(@"Images\Projectile");
            IonCannonUpgrade.ION_CANNON_UPGRADE_SPRITE = this.Content.Load<Texture2D>(@"Images\Projectile");
            CreditOrb.CREDIT_ORB_SPRITE = this.Content.Load<Texture2D>(@"Images\Projectile");
            SpreadCannonUpgrade.SPREAD_CANNON_UPGRADE_SPRITE = this.Content.Load<Texture2D>(@"Images\Projectile");
            SpreadCannon.SPREAD_CANNON_SPRITE = this.Content.Load<Texture2D>(@"Images\Projectile");
            GravityWellUpgrade.GRAVITY_WELL_UPGRADE_SPRITE = this.Content.Load<Texture2D>(@"Images\Projectile");
            GravityWell.GRAVITY_WELL_SPRITE = this.Content.Load<Texture2D>(@"Images\Projectile");
            ShieldUpgrade.SHIELD_UPGRADE_SPRITE = this.Content.Load<Texture2D>(@"Images\Projectile");
            Shield.SHIELD_SPRITE = this.Content.Load<Texture2D>(@"Images\Shield");
            Star.STAR_SPRITE = this.Content.Load<Texture2D>(@"Images\Star");
            Explosion.EXPLOSION_SPRITE = this.Content.Load<Texture2D>(@"Images\Explosion");
            StasisRayProjectile.STASIS_RAY_SPRITE = this.Content.Load<Texture2D>(@"Images\StasisRay");
            StasisRayUpgrade.STASIS_RAY_UPGRADE_SPRITE = this.Content.Load<Texture2D>(@"Images\Projectile");

            m_explosionImage = Explosion.EXPLOSION_SPRITE;
            m_explosionOffset = new Vector2(
                    m_explosionImage.Width * 0.5f, m_explosionImage.Height * 0.5f);

            spriteFont = this.Content.Load<SpriteFont>(@"Font\Kootenay");

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            BaseCannon.BASE_CANNON_SOUND = Content.Load<SoundEffect>(@"Audio\BaseCannon");
            IonCannon.ION_CANNON_SOUND = Content.Load<SoundEffect>(@"Audio\IonCannon");
            SpreadCannon.SPREAD_CANNON_SOUND = Content.Load<SoundEffect>(@"Audio\SpreadCannon");
            Explosion.EXPLOSION_SOUND = Content.Load<SoundEffect>(@"Audio\Explosion");
            StasisRay.STASIS_RAY_SOUND = Content.Load<SoundEffect>(@"Audio\IonCannon");

            m_levelMusic = Content.Load<SoundEffect>(@"Audio\Spritefighter").CreateInstance();
            m_levelMusic.IsLooped = true;
            m_levelMusic.Play();

            initGame();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        public GameState _gameState
        {
            get
            {
                return m_currentState;
            }

            set
            {
                m_currentState = value;
            }
        }

        public void AddPlayerPoints(int points)
        {
            m_playerPoints += points;
        }

        public int _playerPoints
        {
            get
            {
                return m_playerPoints;
            }
        }

        public int _highScore
        {
            get
            {
                return m_highScore;
            }

            set
            {
                m_highScore = value;
                saveHighScore(m_highScore);
            }
        }

        protected void nextLevel()
        {
            m_currentState = GameState.PLAYING;
            if( !((m_currentLevel + 1) > m_gameLevels.Count()))
                m_currentLevel++;
            m_gameLevelManager.ChangeLevel(m_gameLevels[m_currentLevel](m_playerShip, this, m_screenSize));
            m_currentScreen = new PlayScreen(m_playerPoints, m_playerShip._life, m_highScore, m_playerShip._credits, spriteBatch, spriteFont);
        }

        protected void levelComplete(long completionTime)
        {
            m_currentState = GameState.LEVEL_COMPLETE;
            m_levelCompleteTime = completionTime;
            m_currentScreen = new LevelCompleteScreen(m_nextLevelEvent, m_openStoreEvent, spriteBatch, spriteFont);
        }

        protected void pauseGame(long actualTime)
        {
            pauseTime = actualTime;
            m_currentState = GameState.PAUSED;
            m_currentScreen = new PauseScreen(spriteBatch, spriteFont);
        }

        protected void resumeGame(long actualTime)
        {
            pauseOffset = pauseOffset + (actualTime - pauseTime);
            m_currentState = GameState.PLAYING;
            m_currentScreen = new PlayScreen(m_playerPoints, m_playerShip._life, m_highScore, m_playerShip._credits, spriteBatch, spriteFont);
        }

        protected void openStore()
        {
            m_currentState = GameState.STORE;
            m_currentScreen = new StoreScreen(m_nextLevelEvent, m_playerShip, m_buyHardwareEvent, spriteBatch, spriteFont);
        }

        protected void buyHardware(uint type, int cost)
        {
            if (m_playerShip._credits >= cost)
            {
                m_playerShip._credits -= cost;
                PowerUp pu;
                switch (type)
                {
                    case PowerUp.HealthPack:
                        {
                            pu = new HealthPack();
                            pu.PerformAction(m_playerShip, 0);
                            break;
                        }
                    case PowerUp.IonCannon:
                        {
                            pu = new IonCannonUpgrade();
                            pu.PerformAction(m_playerShip, 0);
                            break;
                        }
                    case PowerUp.SpreadCannon:
                        {
                            pu = new SpreadCannonUpgrade();
                            pu.PerformAction(m_playerShip, 0);
                            break;
                        }
                    case PowerUp.GravityWell:
                        {
                            pu = new GravityWellUpgrade();
                            pu.PerformAction(m_playerShip, 0);
                            break;
                        }
                    case PowerUp.Shield:
                        {
                            pu = new ShieldUpgrade();
                            pu.PerformAction(m_playerShip, 0);
                            break;
                        }
                    case PowerUp.StasisRay:
                        {
                            pu = new StasisRayUpgrade();
                            pu.PerformAction(m_playerShip, 0);
                            break;
                        }

                    default:
                        {
                            m_playerShip._credits += cost;
                            Trace("Unknown type for buyHardware()");
                            break;
                        }
                }
            }
        }

        protected void initGame()
        {
            m_playerShip = new PlayerShip(240, 750, 0);
            m_playerShip.AddAttachment(new BaseCannon(m_playerShip, ttms));
            m_playerPoints = 0;
#if WINPHONE
            m_highScore = loadHighScore();
#endif
            loadLevels();

            m_gameLevelManager = new GameLevelManager(this, m_gameLevels[m_currentLevel](m_playerShip, this, m_screenSize));
            Components.Add(m_gameLevelManager);
            m_frameCounter = new FrameRateCounter(this, spriteFont);
            Components.Add(m_frameCounter);

            m_currentScreen = new TitleScreen(m_highScore, spriteBatch, spriteFont);
        }

        protected void loadLevels()
        {
            m_gameLevels[1] = Level1.GetLevel;
            m_gameLevels[2] = Level2.GetLevel;
            m_gameLevels[3] = Level3.GetLevel;
            /*
             m_gameLevels.Add(1, new Level1(m_playerShip, this, m_screenSize));
            m_gameLevels.Add(2, new Level2(m_playerShip, this, m_screenSize));
            m_gameLevels.Add(3, new Level3(m_playerShip, this, m_screenSize));
             */ 
        }

        protected void newGame()
        {
            m_playerShip = new PlayerShip(240, 750, 0);
            m_playerShip.AddAttachment(new BaseCannon(m_playerShip, ttms));
            m_playerPoints = 0;
            m_highScore = loadHighScore();
            m_currentLevel = 1;

            m_gameLevelManager.ChangeLevel(m_gameLevels[1](m_playerShip, this, m_screenSize));

            JuggernautShip.Reset();
        }

        ///
        /// Update
        /// 
        #region UpdateLoop

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            long actualTime = (long)gameTime.TotalGameTime.TotalMilliseconds;
            bool backPressed = false;

            // Allows the game to exit
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            //    this.Exit();

            GamePadState pad = GamePad.GetState(PlayerIndex.One);
            if (pad.Buttons.Back == ButtonState.Pressed)
            {
                backPressed = true;
            }

            ttms = actualTime - pauseOffset; //This is to correct simulation timeline because of pausing.

            bool touched = processInput();

            switch( m_currentState )
            {
                case GameState.START_MENU:
                    {
                        if (backPressed)
                            Exit();

                        if (touched)
                        {
                            m_currentState = GameState.PLAYING;
                            m_currentScreen = new PlayScreen(m_playerPoints, m_playerShip._life, m_highScore, m_playerShip._credits, spriteBatch, spriteFont);
                        }
             
                        break;
                    }
                case GameState.PLAYING:
                    {
                        if (backPressed)
                        {
                            pauseGame(actualTime);
                            break;
                        }

                        m_playerShip.Update(ttms);
                        ((PlayScreen)m_currentScreen).Update(m_playerPoints, m_playerShip._life, m_highScore, m_playerShip._credits, gameTime);

                        if (m_playerShip._life <= 0)
                        {
                            m_currentState = GameState.GAME_OVER;
                            m_gameOverTime = actualTime;
                            m_currentScreen = new GameOverScreen(m_gameOverTime, GAME_OVER_WAIT, spriteBatch, spriteFont);
                        }

                        break;
                    }
                case GameState.LEVEL_COMPLETE:
                    {
                        /*
                        if (touched && ((actualTime - m_levelCompleteTime) > LEVEL_COMPLETE_WAIT))
                        {
                            m_currentState = GameState.PLAYING;
                            m_gameLevelManager.ChangeLevel(new Level2(m_playerShip, this, m_screenSize));
                            m_currentScreen = new PlayScreen(m_playerPoints, m_playerShip._life, m_highScore, m_playerShip._credits, spriteBatch, spriteFont);
                        }
                        */
                        break;
                    }
                case GameState.STORE:
                    {

                        break;
                    }
                case GameState.PAUSED:
                    {
                        if(backPressed)
                            resumeGame(actualTime);

                        break;
                    }
                case GameState.GAME_OVER:
                    {
                        if (touched && ((actualTime - m_gameOverTime) > GAME_OVER_WAIT))
                        {
                            m_currentState = GameState.START_MENU;
                            newGame();
                        }

                        break;
                    }
                case GameState.EXITING:
                    {
                        break;
                    }
                default:
                    break;
            }
            base.Update(gameTime);
        }

        #endregion

        /// 
        /// Input
        /// 
        #region Input



        public bool processInput()
        {
            bool isTouched = false;
            //checkGenerateProjectile(ttms);
            // TODO: Add your update logic here

            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {

                float y = mouseState.Y;
                if (mouseState.Y < MAX_Y)
                    y = MAX_Y;
                y += THUMB_OFFSET;
                if (y > MIN_Y)
                    y = MIN_Y;
                m_playerShip._position = new Vector2(mouseState.X, y);
                isTouched = true;

                m_currentScreen.HandleInput(mouseState);
            }

#if WINPHONE
            TouchCollection touchCollection = TouchPanel.GetState();
            foreach (TouchLocation tl in touchCollection)
            {
                if (tl.State == TouchLocationState.Pressed ||
                    tl.State == TouchLocationState.Moved)
                {
                    float y = tl.Position.Y;
                    if (tl.Position.Y < MAX_Y)
                        y = MAX_Y;
                    y += THUMB_OFFSET;
                    if (y > MIN_Y)
                        y = MIN_Y;
                    m_playerShip._position = new Vector2(tl.Position.X, y);
                    isTouched = true;
                }

                m_currentScreen.HandleInput(tl);
            }
#endif
            m_playerShip.SetTouched(isTouched);
            return isTouched;
        }

        #endregion

        /// 
        /// Render
        /// 
        #region RenderLoop
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            long actualTime = (long)gameTime.TotalGameTime.TotalMilliseconds;

            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here


            /*
            spriteBatch.Draw(SidewinderShip.SIDEWINDER_SPRITE, Vector2.Zero, null, Color.Aquamarine, 0,
                new Vector2(SidewinderShip.SIDEWINDER_SPRITE.Width * 0.5f), 0.5f, SpriteEffects.None, 0);

            spriteBatch.Draw(SidewinderShip.SIDEWINDER_SPRITE, new Vector2(m_screenSize.X, m_screenSize.Y), null, Color.Bisque, 0,
                new Vector2(SidewinderShip.SIDEWINDER_SPRITE.Width * 0.5f, SidewinderShip.SIDEWINDER_SPRITE.Height * 0.5f), 0.5f, SpriteEffects.None, 0);
            */

            switch (m_currentState)
            {
                case GameState.START_MENU:
                    {
                        spriteBatch.Begin();
                        m_currentScreen.Draw(gameTime);
                        spriteBatch.End();
                        break;
                    }
                case GameState.PLAYING:
                    {
                        m_playerShip.Render(spriteBatch);
                        spriteBatch.Begin();
                        m_currentScreen.Draw(gameTime);
                        spriteBatch.End();
                        break;
                    }
                case GameState.LEVEL_COMPLETE:
                    {
                        spriteBatch.Begin();
                        m_currentScreen.Draw(gameTime);
                        spriteBatch.End();
                        break;
                    }
                case GameState.STORE:
                    {
                        spriteBatch.Begin();
                        m_currentScreen.Draw(gameTime);
                        spriteBatch.End();
                        break;
                    }
                case GameState.PAUSED:
                    {
                        spriteBatch.Begin();
                        m_currentScreen.Draw(gameTime);
                        spriteBatch.End();
                        break;
                    }
                case GameState.GAME_OVER:
                    {
                        spriteBatch.Begin();
                        m_currentScreen.Draw(gameTime);
                        spriteBatch.End();
                        break;
                    }
                case GameState.EXITING:
                    {
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            base.Draw(gameTime);
        }
        #endregion

        #region LifeCycle

        private void saveEnemiesToIsolatedStorage()
        {
            Trace("Saving Enemies...");
#if WINPHONE
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isf.FileExists(ENEMY_FILE))
                    isf.DeleteFile(ENEMY_FILE);
                //If user choose to save, create a new file
                using (IsolatedStorageFileStream fs = isf.CreateFile(ENEMY_FILE))
                {
                    //and serialize data
                    XmlSerializer ser = new XmlSerializer(typeof(List<EnemyShip>), ShipTypes);
                    //ser.Serialize(fs, m_enemyShips);
                    GameLevel gl = m_gameLevelManager.GetCurrentLevel();
                    if (gl != null)
                    {
                        ser.Serialize(fs, gl.GetEnemies());
                        fs.Close();
                    }
                }
            }
#endif
            Trace("Finished Saving Enemies");
        }

        private bool loadEnemiesFromIsolatedStorage()
        {
            //Trace the event for debug purposes
            Trace("Loading Enemy Data...");

            bool newGame = true;
#if WINPHONE
        //Try to load previously saved data from IsolatedStorage
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {            
                //Check if file exits
                if (isf.FileExists(ENEMY_FILE))
                {
                    using (IsolatedStorageFileStream fs = isf.OpenFile(ENEMY_FILE, System.IO.FileMode.Open))
                    {
                        //Read the file contents and try to deserialize it back to data object
                        XmlSerializer ser = new XmlSerializer(typeof(List<EnemyShip>), ShipTypes);
                        m_enemyShips = (List<EnemyShip>)ser.Deserialize(fs);

                        //If successfully deserialized, initialize data object variable with it
                        if (m_enemyShips != null)
                        {
                            newGame = false;
                            m_gameLevelManager.RestoreLevel(m_enemyShips);
                        }
                        else
                        {
                            m_enemyShips = new List<EnemyShip>();
                        }
                    }
                }
                else
                {
                    Trace(ENEMY_FILE + " not found.  New game.");
                }
            }
#endif
            Trace("Finished Enemy Data Load...");
            return newGame;
        }

        private void saveGameStateToIsolatedStorage()
        {
#if WINPHONE
            savePlayerData();
            saveGameToIsolatedStorage();
#endif
        }

        private void saveHighScore(int score)
        {
#if WINPHONE
            IsolatedStorageSettings.ApplicationSettings[highScoreTag] = score;
            IsolatedStorageSettings.ApplicationSettings.Save();
#endif
        }

        private void savePlayerData()
        {
#if WINPHONE
            IsolatedStorageSettings.ApplicationSettings[playerLifeTag] = m_playerShip._life;
            IsolatedStorageSettings.ApplicationSettings[playerScoreTag] = m_playerPoints;

            IsolatedStorageSettings.ApplicationSettings.Save();
#endif
        }

        private int loadHighScore()
        {
#if WINPHONE
            if (IsolatedStorageSettings.ApplicationSettings.Contains(highScoreTag))
            {
                return (int)IsolatedStorageSettings.ApplicationSettings[highScoreTag];
            }
            else
                return 0;
#endif
            return 0;
        }

        private void loadPlayerData()
        {
#if WINPHONE
            if (IsolatedStorageSettings.ApplicationSettings.Contains(playerLifeTag))
                m_playerShip._life = (int)IsolatedStorageSettings.ApplicationSettings[playerLifeTag];

            if (IsolatedStorageSettings.ApplicationSettings.Contains(playerScoreTag))
                m_playerPoints = (int)IsolatedStorageSettings.ApplicationSettings[playerScoreTag];
#endif
        }

        private bool loadGameFromIsolatedStorage()
        {
            bool ret = false;
#if WINPHONE
            if (IsolatedStorageSettings.ApplicationSettings.Contains(gameStateTag))
                m_currentState = (GameState)IsolatedStorageSettings.ApplicationSettings[gameStateTag];

            ret = loadEnemiesFromIsolatedStorage();
#endif
            return ret;
        }

        private bool saveGameToIsolatedStorage()
        {

            bool ret = true;
#if WINPHONE
            IsolatedStorageSettings.ApplicationSettings[gameStateTag] = m_currentState;
            IsolatedStorageSettings.ApplicationSettings.Save();

            saveEnemiesToIsolatedStorage();
#endif  
            return ret;
        }

        private void saveGameToStateObject()
        {
#if WINPHONE
            PhoneApplicationService.Current.State[playerScoreTag] = m_playerPoints;
            PhoneApplicationService.Current.State[enemyShipsTag] = m_gameLevelManager.GetCurrentLevel().GetEnemies();//m_enemyShips;
            PhoneApplicationService.Current.State[playerLifeTag] = m_playerShip._life;
            PhoneApplicationService.Current.State[highScoreTag] = m_highScore;
            PhoneApplicationService.Current.State[gameStateTag] = m_currentState;
#endif  
        }

        private bool loadGameFromStateObject()
        {
#if WINPHONE
            if (PhoneApplicationService.Current.State.ContainsKey(enemyShipsTag))
            {
                List<EnemyShip> es = (List<EnemyShip>)PhoneApplicationService.Current.State[enemyShipsTag];
                m_gameLevelManager.GetCurrentLevel().RestoreEnemies(es);
                PhoneApplicationService.Current.State.Remove(enemyShipsTag);
            }

            if (PhoneApplicationService.Current.State.ContainsKey(playerScoreTag))
            {
                m_playerPoints = (int)PhoneApplicationService.Current.State[playerScoreTag];
                PhoneApplicationService.Current.State.Remove(playerScoreTag);
            }

            if (PhoneApplicationService.Current.State.ContainsKey(playerLifeTag))
            {
                m_playerShip._life = (int)PhoneApplicationService.Current.State[playerLifeTag];
                PhoneApplicationService.Current.State.Remove(playerLifeTag);
            }

            if (PhoneApplicationService.Current.State.ContainsKey(highScoreTag))
            {
                m_highScore = (int)PhoneApplicationService.Current.State[highScoreTag];
                PhoneApplicationService.Current.State.Remove(highScoreTag);
            }

            if (PhoneApplicationService.Current.State.ContainsKey(gameStateTag))
            {
                m_currentState = (GameState)PhoneApplicationService.Current.State[gameStateTag];
                PhoneApplicationService.Current.State.Remove(gameStateTag);
            } 
#endif            
            return true;
        }

        #endregion

        #region Events

        private void bindEvents()
        {
            //Phone lifecycle events
#if WINPHONE
            PhoneApplicationService.Current.Activated += new EventHandler<ActivatedEventArgs>(Current_Activated);
            PhoneApplicationService.Current.Deactivated += new EventHandler<DeactivatedEventArgs>(Current_Deactivated);
            PhoneApplicationService.Current.Launching += new EventHandler<LaunchingEventArgs>(Current_Launching);
            PhoneApplicationService.Current.Closing += new EventHandler<ClosingEventArgs>(Current_Closing);
            this.Window.OrientationChanged += new EventHandler<EventArgs>(Window_OrientationChanged);
#endif
            //XNA lifecycle events
            this.Activated += new EventHandler<EventArgs>(SprigtFighter_Activated);
            this.Deactivated += new EventHandler<EventArgs>(SprightFighter_Deactivated);

            //Orientation event
            m_levelCompleteEvent.LevelCompleteEventHandler += new EventHandler<LevelCompleteEventArgs>(SpriteFighter_LevelComplete);
            m_nextLevelEvent.NextLevelEventHandler += new EventHandler<EventArgs>(SpriteFighter_NextLevel);
            m_openStoreEvent.OpenStoreEventHandler += new EventHandler<EventArgs>(SpriteFighter_OpenStore);
            m_buyHardwareEvent.BuyHardwareEventHandler += new EventHandler<BuyHardwareEventArgs>(SpriteFighter_BuyHardware);
        }

        private void SpriteFighter_LevelComplete(object src, LevelCompleteEventArgs lcea)
        {
            GameLevel finishedLevel = lcea.CompletedLevel;
            levelComplete(lcea.CompletionTime);
        }

        private void SpriteFighter_NextLevel(object src, EventArgs nlea)
        {
            nextLevel();
        }

        private void SpriteFighter_OpenStore(object src, EventArgs osea)
        {
            openStore();
        }

        private void SpriteFighter_BuyHardware(object src, BuyHardwareEventArgs bhea)
        {
            buyHardware(bhea._type, bhea._cost);
        }

        void SprigtFighter_Activated(object sender, EventArgs e)
        {
            Trace("XNA Activated");
        }

        void SprightFighter_Deactivated(object sender, EventArgs e)
        {
            //m_currentState = GameState.PAUSED;
            //m_currentScreen = new PauseScreen(spriteBatch, spriteFont);
            saveHighScore(m_highScore); //Just in case
            Trace("XNA Deactivated");
        }

#if WINPHONE
        void Current_Closing(object sender, ClosingEventArgs e)
        {
            saveHighScore(m_highScore);
            Trace("Closing");
        }

        void Current_Launching(object sender, LaunchingEventArgs e)
        {
            loadHighScore();
            Trace("Launching");
        }

        void Current_Deactivated(object sender, DeactivatedEventArgs e)
        {
            saveGameStateToIsolatedStorage();
            /*
            Thread t = new Thread(saveGameStateToIsolatedStorage);
            t.Start();
             */
            Trace("Deactivated");
        }

        void Current_Activated(object sender, ActivatedEventArgs e)
        {
            loadHighScore();
            loadPlayerData();
            if (loadGameFromIsolatedStorage())
            {
                Trace("Data loaded from isolated storage");
            }
            else
            {
                Trace("No isolated storage content found. ");
            }
            Trace("Activated");
        }

        void Window_OrientationChanged(object sender, EventArgs e)
        {
        }
#endif

        #endregion

        #region DebugUtils

        public static void Trace(string msg)
        {
#if DEBUG
            Debug.WriteLine("SPRITEFIGHTER EVENT: {0} at {1}", msg, DateTime.Now.ToLongTimeString());
#endif
        }

        #endregion
    }
}
