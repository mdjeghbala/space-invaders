using System;
using System.Collections.Generic;
using System.Drawing;

namespace SpaceInvaders
{
     class EnemyBlock : GameObject
    {
        private HashSet<SpaceShip> enemyShips = new HashSet<SpaceShip>();
        private int baseWidth;
        private Vecteur2D position;
        private Size size;

        public EnemyBlock(Vecteur2D position, Size size)
        {
            this.position = position;
            this.size = size;
        }

        public void AddLine(int nbShips, int nbLives, Bitmap shipImage)
        {
            foreach (var ship in enemyShips)
            {
               
            }
        }

        public override void Collision(Missile m)
        {
            throw new System.NotImplementedException();
        }

        public override void Draw(Game gameInstance, Graphics graphics)
        {
            throw new System.NotImplementedException();
        }

        public override bool IsAlive()
        {
            throw new System.NotImplementedException();
        }

        public override void Update(Game gameInstance, double deltaT)
        {
            throw new System.NotImplementedException();
        }
    }

}