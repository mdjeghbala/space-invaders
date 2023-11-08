using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    class Bunker : SimpleObject
    {
        public Bunker(Vecteur2D position) : base(SpaceInvaders.Properties.Resources.bunker, 3, position)
        {
        }

        protected override void OnCollision(Missile missile, int numberOfPixelsInCollision)
        {
            // Réduction du nombre de vies du missile en fonction du nombre de pixels en collision
            missile.Lives -= numberOfPixelsInCollision;
        }

        public void TestCollision(Missile missile)
        {
            if (TestCollisionRectangles(missile))
            {
                TestCollisionPixels(missile);
            }
        }

        public override void Collision(Missile missile)
        {
            TestCollision(missile);
        }

        public override void Update(Game gameInstance, double deltaT)
        {
            
        }
    }
}
