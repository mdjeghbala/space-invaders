using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace SpaceInvaders
{
    class SpaceShip : SimpleObject
    {
    
        private Missile missile;

        public Side SpaceshipSide { get; private set; }


        public SpaceShip(Vecteur2D position, int lives, Bitmap image, Side spaceshipside) : base(image, lives, position, spaceshipside)
        {
            SpaceshipSide = spaceshipside;
        }

        public void shoot(Game gameInstance)
        {
            if (this.missile == null || !this.missile.IsAlive())
            {
                // Création d'un nouveau missile en fonction du camp du vaisseau
                Side missileSide = (SpaceshipSide == Side.Ally) ? Side.Ally : Side.Enemy;

                // Ajustement la position de départ du missile en fonction du camp
                int yOffset = (SpaceshipSide == Side.Ally) ? -20 : 20;

                // Création du missile
                this.missile = new Missile(new Vecteur2D(base.Position.X + 10, base.Position.Y + yOffset), 1, SpaceInvaders.Properties.Resources.shoot1, missileSide);

                // Ajout du missile au jeu
                gameInstance.AddNewGameObject(this.missile);
            }

        }

        public override void Update(Game gameInstance, double deltaT)
        {
            
        }

        protected override void OnCollision(Missile missile, int numberOfPixelsInCollision)
        {
            if (TestCollisionRectangles(missile))
            {
                //Reduction de la vie lorsqu'un vaisseau est touché 
                Lives--;
                missile.Lives--;
            }
        }

    }

}
