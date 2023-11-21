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
        private int moveSpeed = 15;
        public EnemyBlock(Vecteur2D position, int baseWidth)
        {
            this.position = position;
            this.baseWidth = baseWidth;
            this.size = new Size(baseWidth, 0);
        }

        public void AddLine(int nbShips, int nbLives, Bitmap shipImage)
        {
            int shipWidth = shipImage.Width;
            int yOffset = this.size.Height;

            for (int i = 0; i < nbShips; i++)
            {
                Vecteur2D shipPosition;
                if (nbShips == 1)
                {
                    shipPosition = new Vecteur2D(this.position.X, this.position.Y + yOffset);
                }
                else
                {
                    int spacing = (baseWidth - (nbShips * shipWidth)) / (nbShips - 1);
                    shipPosition = new Vecteur2D(this.position.X + i * (shipWidth + spacing), this.position.Y + yOffset);
                }

                SpaceShip enemyShip = new SpaceShip(shipPosition, nbLives, shipImage);
                enemyShips.Add(enemyShip);
            }

            UpdateSize();
        }

        public void UpdateSize()
        {
            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;

            foreach (SpaceShip enemyShip in enemyShips)
            {
                if (enemyShip.Position.X < minX)
                    minX = (float)enemyShip.Position.X;
                if (enemyShip.Position.Y < minY)
                    minY = (float)enemyShip.Position.Y;
                if (enemyShip.Position.X + enemyShip.Image.Width > maxX)
                    maxX = (float)(enemyShip.Position.X + enemyShip.Image.Width);
                if (enemyShip.Position.Y + enemyShip.Image.Height > maxY)
                    maxY = (float)(enemyShip.Position.Y + enemyShip.Image.Height);
            }

            this.position = new Vecteur2D(minX, minY);
            this.size = new Size((int)(maxX - minX), (int)(maxY - minY));
        }

        // Fonction qui permet de faire descendre le bloc
        public void MoveDown(int distance)
        {
            position.Y += distance;
            foreach (SpaceShip enemyShip in enemyShips)
            {
                enemyShip.Position.Y += distance;
            }
        }

        private int direction = 1; // 1 pour droite, -1 pour gauche

        public override void Update(Game gameInstance, double deltaT)
        {
            // Largeur de l'écran du jeu
            int screenWidth = gameInstance.gameSize.Width;

            // Mise à jour de la position X du bloc en fonction de la direction
            position.X += direction * moveSpeed * deltaT;

            // Si le bloc a atteint le bord droit de l'écran
            if (position.X + size.Width >= screenWidth && direction == 1)
            {
                // Appel de la fonction pour faire descendre le bloc
                MoveDown(enemyShips.First().Image.Height);
                // La direction du bloc passe à gauche
                direction = -1;
                // Augmentation de la vitesse horizontale du bloc
                moveSpeed += 3;
            }
            // Si le bloc a atteint le bord gauche de l'écran
            if (position.X <= 0 && direction == -1)
            {
                // Appel de la fonction pour faire descendre le bloc
                MoveDown(enemyShips.First().Image.Height);
                // La direction du bloc passe à droite
                direction = 1;
                // Augmentation de la vitesse horizontale du bloc
                moveSpeed += 3;
            }

            // Mise à jour de la position X de chaque vaisseau dans le bloc
            foreach (SpaceShip enemyShip in enemyShips)
            {
                enemyShip.Position.X += direction * moveSpeed * deltaT;
            }
            // Suppression des vaisseaux enemis lorsqu'ils ne sont plus en vie
            enemyShips.RemoveWhere(ship => !ship.IsAlive());

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
            return enemyShips.Any(ship => ship.IsAlive());
        }

        public override void Collision(Missile missile)
        {
            foreach (SpaceShip enemy in enemyShips)
            {
                if (enemy.TestCollisionRectangles(missile))
                {
                    enemy.Collision(missile); 
                }
            }
        }
    }

}
