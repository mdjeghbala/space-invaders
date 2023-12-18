using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;
using static System.Net.Mime.MediaTypeNames;
using NAudio.Wave;

namespace SpaceInvaders
{
    /// <summary>
    /// This class represents the entire game, it implements the singleton pattern
    /// </summary>
    /// Cette elle qui contient les mécanismes fondamentaux du jeu.
    class Game
    {

        #region GameObjects management
        /// <summary>
        /// Set of all game objects currently in the game
        /// </summary>
        public HashSet<GameObject> gameObjects = new HashSet<GameObject>();

        /// <summary>
        /// Set of new game objects scheduled for addition to the game
        /// </summary>
        private HashSet<GameObject> pendingNewGameObjects = new HashSet<GameObject>();

        /// <summary>
        /// Schedule a new object for addition in the game.
        /// The new object will be added at the beginning of the next update loop
        /// </summary>
        /// <param name="gameObject">object to add</param>
        public void AddNewGameObject(GameObject gameObject)
        {
            pendingNewGameObjects.Add(gameObject);
        }
        #endregion


        #region game technical elements
        /// <summary>
        /// Size of the game area
        /// </summary>
        public Size gameSize;

        /// <summary>
        /// State of the keyboard
        /// </summary>
        public HashSet<Keys> keyPressed = new HashSet<Keys>();
        #endregion


        #region static fields (helpers)
        /// <summary>
        /// Singleton for easy access
        /// </summary>
        public static Game game { get; private set; }

        /// <summary>
        /// A shared black brush
        /// </summary>
        private static Brush blackBrush = new SolidBrush(Color.Black);

        /// <summary>
        /// A shared simple font
        /// </summary>
        private static Font defaultFont = new Font("Times New Roman", 24, FontStyle.Bold, GraphicsUnit.Pixel);

        private static WaveOutEvent waveOutEvent;
        private static AudioFileReader audioFile;

        private static WaveOutEvent winWaveOutEvent;
        private static AudioFileReader winAudioFile;

        private static WaveOutEvent lostWaveOutEvent;
        private static AudioFileReader lostAudioFile;

        public static int score = 0;
        #endregion


        # region fields Private and Public 
        private SpaceShip playerShip;
        private SpaceShip playerShip2;

        private Bunker bunker;
        private Bunker bunker2;
        private Bunker bunker3;

        private EnemyBlock enemies;

        public Random random = new Random();

        private Bitmap backgroundImage;
        public enum GameState { Play, Pause, Win, Lost }

        private GameState state = GameState.Play;
        #endregion



        #region constructors
        /// <summary>
        /// Singleton constructor
        /// </summary>
        /// <param name="gameSize">Size of the game area</param>
        /// 
        /// <returns></returns>
        public static Game CreateGame(Size gameSize)
        {
            if (game == null)
                game = new Game(gameSize);
            return game;
        }

        /// <summary>
        /// Private constructor
        /// </summary>
        /// <param name="gameSize">Size of the game area</param>
        private Game(Size gameSize)
        {
            this.gameSize = gameSize;
            initGame();
        }
        #endregion


        #region methods
        public void initGame()
        {
            score = 0;
            audioFile = null;
            winAudioFile = null;
            lostAudioFile = null;
            backgroundImage = SpaceInvaders.Properties.Resources.background;

            // intialize two players and add to list
            this.playerShip = new PlayerSpaceship(new Vecteur2D((gameSize.Width / 2) - 157, gameSize.Height - 50), 3, SpaceInvaders.Properties.Resources.ship3, Side.Ally, false);
            this.playerShip2 = new PlayerSpaceship(new Vecteur2D((gameSize.Width / 2) + 130, gameSize.Height - 50), 3, SpaceInvaders.Properties.Resources.ship3, Side.Ally, true);
           
            AddNewGameObject(this.playerShip);
            AddNewGameObject(this.playerShip2);

            // initialize three bunkers and add to list
            this.bunker = new Bunker(new Vecteur2D(gameSize.Width / 2 - 45, gameSize.Height - 125), Side.Neutral);
            this.bunker2 = new Bunker(new Vecteur2D(gameSize.Width / 2 - 190, gameSize.Height - 125), Side.Neutral);
            this.bunker3 = new Bunker(new Vecteur2D(gameSize.Width / 2 + 100, gameSize.Height - 125), Side.Neutral);

            AddNewGameObject(this.bunker);
            AddNewGameObject(this.bunker2);
            AddNewGameObject(this.bunker3);

            // initialize enemy block with differents lines of enemy and add to list
            this.enemies = new EnemyBlock(new Vecteur2D(10, 95), 250, Side.Enemy);

            enemies.AddLine(7, 1, SpaceInvaders.Properties.Resources.ship4);
            enemies.AddLine(7, 1, SpaceInvaders.Properties.Resources.ship1);
            enemies.AddLine(7, 1, SpaceInvaders.Properties.Resources.ship7);
            enemies.AddLine(7, 1, SpaceInvaders.Properties.Resources.ship3);

            AddNewGameObject(this.enemies);
        }


        /// <summary>
        /// Force a given key to be ignored in following updates until the user
        /// explicitily retype it or the system autofires it again.
        /// </summary>
        /// <param name="key">key to ignore</param>
        public void ReleaseKey(Keys key)
        {
            keyPressed.Remove(key);
        }


        private void PlayAudio(string filePath, ref WaveOutEvent waveOutEvent, ref AudioFileReader audioFile)
        {
            if (audioFile == null)
            {
                audioFile = new AudioFileReader(filePath);
                waveOutEvent = new WaveOutEvent();
                waveOutEvent.Init(audioFile);
                waveOutEvent.Play();
            }
        }


        public void winGameSound()
        {
            PlayAudio("Resources\\victorySound.wav", ref winWaveOutEvent, ref winAudioFile);
        }


        public void gameCreatedMusic()
        {
            if (this.state == GameState.Play)
            {
                PlayAudio("Resources\\gameMusic.wav", ref waveOutEvent, ref audioFile);
            }
        }


        public void loseGameSound()
        {
            PlayAudio("Resources\\lostSound.wav", ref lostWaveOutEvent, ref lostAudioFile);
        }


        public void verifMusic()
        {
            if (audioFile == null)
            {
                gameCreatedMusic();
            }
            else
            {
                if (this.state == GameState.Play) waveOutEvent.Play();
                if (this.state == GameState.Pause) waveOutEvent.Pause();
                if (this.state == GameState.Win || this.state == GameState.Lost) 
                {
                    waveOutEvent.Stop();
                    if (this.state == GameState.Win) winGameSound();
                    if (this.state == GameState.Lost) loseGameSound();
                }
            }
        }


        /// <summary>
        /// Draw the whole game
        /// </summary>
        /// <param name="g">Graphics to draw in</param>
        public void Draw(Graphics g)
        {
            g.DrawImage(backgroundImage, new Rectangle(0, 0, gameSize.Width, gameSize.Height));

            if (this.state == GameState.Play)
            {
                foreach (GameObject gameObject in gameObjects) gameObject.Draw(this, g);
            }

            string text = "";

            if (this.state == GameState.Pause) text = "Pause";
            if (this.state == GameState.Lost) text = " Vous avez perdu avec le score de " + score + " \n\nAppuyez sur espace pour relancer une partie";
            if (this.state == GameState.Win) text = " Vous avez gagné avec le score de " + score + " !" + " \n\nAppuyez sur espace pour relancer une partie";

            SizeF textSize = g.MeasureString(text, defaultFont);
            PointF textPosition = new PointF((gameSize.Width - textSize.Width) / 2, (gameSize.Height - textSize.Height) / 2);
            g.DrawString(text, defaultFont, new SolidBrush(Color.White), textPosition);
        }


        /// <summary>
        /// Update game
        /// </summary>
        public void Update(double deltaT)
        {
            if (state == GameState.Play)
            {
                verifMusic();

                // make pause
                if (keyPressed.Contains(Keys.P))
                {
                    state = GameState.Pause;
                    verifMusic();
                    ReleaseKey(Keys.P);
                }

                // add new game objects
                gameObjects.UnionWith(pendingNewGameObjects);
                pendingNewGameObjects.Clear();

                // players die
                if(!(this.playerShip.IsAlive()) && !(this.playerShip2.IsAlive()))
                {
                    this.state = GameState.Lost;
                }

                // enemy block dies
                if (this.enemies.EnemyShips.All(ship => !ship.IsAlive()))
                {
                    this.state = GameState.Win;
                }

                // enemy block at same height of players so he lost
                if (this.enemies.Position.Y + this.enemies.Size.Height >= this.playerShip.Position.y)
                {
                    this.playerShip.Lives = 0;
                    this.playerShip2.Lives = 0;
                }

                // update each game object
                foreach (GameObject gameObject in gameObjects) gameObject.Update(this, deltaT);

                // remove dead objects
                gameObjects.RemoveWhere(gameObject => !gameObject.IsAlive());
                gameObjects.RemoveWhere(gameObject => gameObject is SpaceShip && !((SpaceShip)gameObject).IsAlive());
            }

            // restart game
            if (this.state == GameState.Win || this.state == GameState.Lost)
            {
                verifMusic();
                gameObjects.RemoveWhere(gameObject => gameObject.IsAlive());
                gameObjects.Clear();
                pendingNewGameObjects.Clear();

                if (keyPressed.Contains(Keys.Space))
                {
                    initGame();
                    this.state = GameState.Play;
                    ReleaseKey(Keys.Space);
                }
            }

            // remove pause
            if (state == GameState.Pause && keyPressed.Contains(Keys.P))
            {
                state = GameState.Play;
                ReleaseKey(Keys.P);
            }
        }
        #endregion


    }

}
