using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private int moveSpeed = 60;
        private double randomShootProbability = 0.035;

        public EnemyBlock(Vecteur2D position, int baseWidth, Side side) : base(side)
        {
            this.position = position;
            this.baseWidth = baseWidth;
            this.size = new Size(baseWidth, 0);
        }

        public Vecteur2D Position
        {
            get { return this.position; }
        }

        public Size Size
        {
            get { return this.size; }
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

                SpaceShip enemyShip = new SpaceShip(shipPosition, nbLives, shipImage, Side.Enemy);
                enemyShips.Add(enemyShip);
            }

            UpdateSize();
        }


        public HashSet<SpaceShip> EnemyShips
        {
            get { return enemyShips; }
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

        public override void Update(Game gameInstance, double deltaT)
        {
            int screenWidth = gameInstance.gameSize.Width;
            position.X += direction * moveSpeed * deltaT;

            if (position.X + size.Width >= screenWidth && direction == 1)
            {
                MoveDown(enemyShips.First().Image.Height);
                direction = -1;
                moveSpeed += 8;
                randomShootProbability += 0.025;
            }
            // Si le bloc a atteint le bord gauche de l'écran
            if (position.X <= 0 && direction == -1)
            {
                MoveDown(enemyShips.First().Image.Height);
                direction = 1;
                moveSpeed += 8;
                randomShootProbability += 0.015;
            }

            foreach (SpaceShip enemyShip in EnemyShips)
            {
                enemyShip.Position.X += direction * moveSpeed * deltaT;

                // Tir aléatoire du bloc ennemi
                if (gameInstance.random.NextDouble() <= randomShootProbability * deltaT)
                {
                    enemyShip.shoot(gameInstance);
                }
            }

            enemyShips.RemoveWhere(ship =>  !(ship.IsAlive()));
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
