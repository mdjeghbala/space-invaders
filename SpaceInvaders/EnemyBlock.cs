using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SpaceInvaders
{
    class EnemyBlock : GameObject
    {
        private HashSet<SpaceShip> enemyShips = new HashSet<SpaceShip>();
        private int baseWidth;
        private Size size;
        private Vecteur2D position;

        public EnemyBlock(Vecteur2D position, int baseWidth)
        {
            this.position = position;
            this.baseWidth = baseWidth;
            this.enemyShips = new HashSet<SpaceShip>();
            UpdateSize();
        }

        public Vecteur2D Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        public Size Size
        {
            get { return this.size; }
            set { this.size = value; }
        }

        public void AddLine(int nbShips, int nbLives, Bitmap shipImage)
        {
            int spacing = 10;
            int ySpacing = 10; // Espacement vertical entre les lignes

            for (int i = 0; i < nbShips; i++)
            {
                var x = Position.X + (i * (shipImage.Width + spacing));
                var y = Position.Y + (enemyShips.Count / nbShips) * (shipImage.Height + ySpacing); // Utilisation de la ligne actuelle pour calculer la position Y
                SpaceShip enemyShip = new SpaceShip(new Vecteur2D(x, y), nbLives, shipImage);
                enemyShips.Add(enemyShip);
            }
            UpdateSize();
        }


        public void UpdateSize()
        {
            int width = baseWidth;
            int height = enemyShips.Count > 0 ? enemyShips.Max(ship => ship.Image.Height) : 0;
            size = new Size(width, height);
        }

        public override void Collision(Missile m)
        {
            // Gestion de la collision ici
        }

        public override void Update(Game gameInstance, double deltaT)
        {
            // Mise à jour des ennemis si nécessaire
        }

        public override void Draw(Game gameInstance, Graphics graphics)
        {
            foreach (SpaceShip s in enemyShips)
            {
                s.Draw(gameInstance, graphics);
            }
        }

        public override bool IsAlive()
        {
            // Vérifie si au moins un SpaceShip est en vie
            return enemyShips.Any(ship => ship.IsAlive());
        }
    }
}
