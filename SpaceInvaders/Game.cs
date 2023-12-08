using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;
using static System.Net.Mime.MediaTypeNames;

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

        private SpaceShip playerShip;
        private SpaceShip playerShip2;

        private Bunker bunker;
        private Bunker bunker2;
        private Bunker bunker3;

        private EnemyBlock enemies;

        public Random random = new Random();

        private Bitmap backgroundImage;


        // ENUM ET ATTRIBUT POUR GERER L'ETAT DU JEU
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
            backgroundImage = SpaceInvaders.Properties.Resources.background;

            // Initialise 2 vaisseaux joueur avec 3 vies et Position initiale centrée en bas
            this.playerShip = new PlayerSpaceship(new Vecteur2D((gameSize.Width / 2) - 157, gameSize.Height - 50), 3, SpaceInvaders.Properties.Resources.ship3, Side.Ally, false);
            this.playerShip2 = new PlayerSpaceship(new Vecteur2D((gameSize.Width / 2) + 130, gameSize.Height - 50), 3, SpaceInvaders.Properties.Resources.ship3, Side.Ally, true);
           
            // Ajout vaisseau du joueur à la liste des objets du jeu
            AddNewGameObject(this.playerShip);
            AddNewGameObject(this.playerShip2);

            // Initialisation des bunkers et Position initiale centrée en bas
            this.bunker = new Bunker(new Vecteur2D(gameSize.Width / 2 - 45, gameSize.Height - 125), Side.Neutral);
            this.bunker2 = new Bunker(new Vecteur2D(gameSize.Width / 2 - 190, gameSize.Height - 125), Side.Neutral);
            this.bunker3 = new Bunker(new Vecteur2D(gameSize.Width / 2 + 100, gameSize.Height - 125), Side.Neutral);

            // Ajouts des bunkers à la liste des objets du jeu
            AddNewGameObject(this.bunker);
            AddNewGameObject(this.bunker2);
            AddNewGameObject(this.bunker3);

            // Initialisation block ennemie et Position initiale coin supérieur gauche
            this.enemies = new EnemyBlock(new Vecteur2D(10, 20), 250, Side.Enemy);

            //Ajout différentes lignes d'ennemies
            enemies.AddLine(9, 1, SpaceInvaders.Properties.Resources.ship6);
            enemies.AddLine(5, 1, SpaceInvaders.Properties.Resources.ship5);
            enemies.AddLine(3, 1, SpaceInvaders.Properties.Resources.ship3);
            enemies.AddLine(9, 1, SpaceInvaders.Properties.Resources.ship8);
            enemies.AddLine(3, 1, SpaceInvaders.Properties.Resources.ship8);
            enemies.AddLine(3, 1, SpaceInvaders.Properties.Resources.ship3);

            // Ajout block ennemie à la liste des objets du jeu
            AddNewGameObject(this.enemies);

        }

        #endregion

        #region methods

        /// <summary>
        /// Force a given key to be ignored in following updates until the user
        /// explicitily retype it or the system autofires it again.
        /// </summary>
        /// <param name="key">key to ignore</param>
        public void ReleaseKey(Keys key)
        {
            keyPressed.Remove(key);
        }


        /// <summary>
        /// Draw the whole game
        /// </summary>
        /// <param name="g">Graphics to draw in</param>
        public void Draw(Graphics g)
        {
            if (backgroundImage != null)
            {
                g.DrawImage(backgroundImage, new Rectangle(0, 0, gameSize.Width, gameSize.Height));
            }

            if (this.state == GameState.Play)
            {
                foreach (GameObject gameObject in gameObjects)
                {
                    gameObject.Draw(this, g);
                }
            }

            string text = "";

            if (this.state == GameState.Pause) text = "Pause";
            if (this.state == GameState.Lost) text = " Vous avez perdu ! \n\nAppuyez sur espace pour relancer une partie";
            if (this.state == GameState.Win) text = " Vous avez gagné ! \n\nAppuyez sur espace pour relancer une partie";

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
                // Gestion de la touche "p" pour mettre pause
                if (keyPressed.Contains(Keys.P))
                {
                    state = GameState.Pause;
                    ReleaseKey(Keys.P);
                }

                // add new game objects
                gameObjects.UnionWith(pendingNewGameObjects);
                pendingNewGameObjects.Clear();


                // if space is pressed
                if (keyPressed.Contains(Keys.B))
                {
                    // create new BalleQuiTombe
                    GameObject newObject = new BalleQuiTombe(gameSize.Width / 2, 0, Side.Enemy);
                    // add it to the game
                    AddNewGameObject(newObject);
                    // release key space (no autofire)
                    ReleaseKey(Keys.B);
                }

                // JOUEUR MEURT DONC PERDU
                if(!(this.playerShip.IsAlive()) && !(this.playerShip2.IsAlive()))
                {
                    this.state = GameState.Lost;
                }

                //BLOC ENNEMIES MEURT DONC GAGNER
                if (this.enemies.EnemyShips.All(ship => !ship.IsAlive()))
                {
                    this.state = GameState.Win;
                }

                // BLOC ENNEMIE ARRIVE AU NIVEAU DU JOUEUR
                if (this.enemies.Position.Y + this.enemies.Size.Height >= this.playerShip.Position.y)
                {
                    this.playerShip.Lives = 0;
                    this.playerShip2.Lives = 0;
                }

                // update each game object
                foreach (GameObject gameObject in gameObjects)
                {
                    gameObject.Update(this, deltaT);
                }

                // remove dead objects
                gameObjects.RemoveWhere(gameObject => !gameObject.IsAlive());

                gameObjects.RemoveWhere(gameObject => gameObject is SpaceShip && !((SpaceShip)gameObject).IsAlive());
            }

            // GAGNER OU PERDU DONC RELANCER LE JEU
            if (this.state == GameState.Win || this.state == GameState.Lost)
            {
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

            // Gestion de la touche "p" pour enlever pause
            if (state == GameState.Pause && keyPressed.Contains(Keys.P))
            {
                state = GameState.Play;
                ReleaseKey(Keys.P);
            }

        }
        #endregion
    }
}
