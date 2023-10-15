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

        public override void Collision(Missile m)
        {
            
        }

        public override void Update(Game gameInstance, double deltaT)
        {
        }

    }

}
