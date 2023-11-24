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

        public SpaceShip(Vecteur2D position, int lives, Bitmap image, Side side) : base(image, lives, position, side)
        {
        }

        public void shoot(Game gameInstance)
        {
            if (this.missile == null || !this.missile.IsAlive())
            {
                this.missile = new Missile(new Vecteur2D(base.Position.X + 10, base.Position.Y - 20), 1, SpaceInvaders.Properties.Resources.shoot1, Side.Ally);
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
