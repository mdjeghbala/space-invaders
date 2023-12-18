﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    class Bunker : SimpleObject
    {
        public Bunker(Vecteur2D position, Side side) : base(SpaceInvaders.Properties.Resources.bunker, 3, position, side)
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

        public override void Collision(Missile missile)
        {
            if (missile.ObjectSide == this.ObjectSide)
            {
                // Ignore la collision si le missile est du meme camp que le bunker
                return;
            }
            else
            {
                TestCollision(missile);
            }
        }

        public override void Update(Game gameInstance, double deltaT)
        {
            
        }
    }
}
