using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

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

        private Bunker bunker;
        private Bunker bunker2;
        private Bunker bunker3;

        private EnemyBlock enemies;

        public Random random = new Random();


        // ENUM ET ATTRIBUT POUR GERER L'ETAT DU JEU
        public enum GameState { Play, Pause }

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

            // Initialisez le vaisseau du joueur avec 3 vies et Position initiale centrée en bas
            this.playerShip = new PlayerSpaceship(new Vecteur2D((gameSize.Width / 2) - 15, gameSize.Height - 50), 3, SpaceInvaders.Properties.Resources.ship3, Side.Ally);

            // Ajout vaisseau du joueur à la liste des objets du jeu
            AddNewGameObject(this.playerShip);

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
            enemies.AddLine(4, 1, SpaceInvaders.Properties.Resources.ship3);


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
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Draw(this, g);
            }

            if (state == GameState.Pause)
            {
                // Dessiner le texte "Pause" au centre de l'écran
                string pauseText = "Pause";
                SizeF textSize = g.MeasureString(pauseText, defaultFont);
                PointF textPosition = new PointF((gameSize.Width - textSize.Width) / 2, (gameSize.Height - textSize.Height) / 2);
                g.DrawString(pauseText, defaultFont, blackBrush, textPosition);
            }
        }


        /// <summary>
        /// Update game
        /// </summary>
        public void Update(double deltaT)
        {
            // Gestion de la touche "p" pour la pause
            if (keyPressed.Contains(Keys.P))
            {
                if (state == GameState.Play)
                {
                    state = GameState.Pause;
                }
                else if (state == GameState.Pause)
                {
                    state = GameState.Play;
                }

                // Libérer la touche "p" pour éviter les changements d'état répétés
                ReleaseKey(Keys.P);
            }

            if(state == GameState.Play)
            {
                // add new game objects
                gameObjects.UnionWith(pendingNewGameObjects);
                pendingNewGameObjects.Clear();


                // if space is pressed
                if (keyPressed.Contains(Keys.Space))
                {
                    // create new BalleQuiTombe
                    GameObject newObject = new BalleQuiTombe(gameSize.Width / 2, 0, Side.Enemy);
                    // add it to the game
                    AddNewGameObject(newObject);
                    // release key space (no autofire)
                    ReleaseKey(Keys.Space);
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
        }
        #endregion
    }
}
