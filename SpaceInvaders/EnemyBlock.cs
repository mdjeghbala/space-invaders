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
            this.size = new Size(baseWidth, 0); // Initialise la hauteur à 0
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
            int shipWidth = shipImage.Width;
            int yOffset = this.Size.Height;

            for (int i = 0; i < nbShips; i++)
            {
                Vecteur2D shipPosition;
                if (nbShips == 1)
                {
                    // Placement du vaisseau au début de la ligne si il y'en a que 1
                    shipPosition = new Vecteur2D(this.Position.X, this.Position.Y + yOffset);
                }
                else
                {
                    // Répartition des vaisseaux avec un espacement
                    int spacing = (baseWidth - (nbShips * shipWidth)) / (nbShips - 1);
                    shipPosition = new Vecteur2D(this.Position.X + i * (shipWidth + spacing), this.Position.Y + yOffset);
                }

                SpaceShip enemyShip = new SpaceShip(shipPosition, nbLives, shipImage);
                enemyShips.Add(enemyShip);
            }

            // Ajustement du bloc après chaque ligne ajoutée
            UpdateSize();
        }

        public void UpdateSize()
        {
            // Initialisation des valeurs minimales et maximales pour le calcul de la taille
            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;

            foreach (SpaceShip enemyShip in enemyShips)
            {
                // Trouve les coordonnées minimales et maximales de tous les vaisseaux
                if (enemyShip.Position.X < minX)
                    minX = (float)enemyShip.Position.X;
                if (enemyShip.Position.Y < minY)
                    minY = (float)enemyShip.Position.Y;
                if (enemyShip.Position.X + enemyShip.Image.Width > maxX)
                    maxX = (float)(enemyShip.Position.X + enemyShip.Image.Width);
                if (enemyShip.Position.Y + enemyShip.Image.Height > maxY)
                    maxY = (float)(enemyShip.Position.Y + enemyShip.Image.Height);
            }

            // Met à jour la position et la taille du bloc
            this.Position = new Vecteur2D(minX, minY);
            this.Size = new Size((int)(maxX - minX), (int)(maxY - minY));
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
