using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    class Missile : SimpleObject
    {
        private double vitesse = 150;
        
        public Missile(Vecteur2D position, int lives, Bitmap image) : base(image, lives, position)
        {
        }

        public double Vitesse
        {
            get { return vitesse; }
        }


        public override void Update(Game gameInstance, double deltaT)
        {
            // DEPLACEMENT VERTICAL DU MISSILE
            Console.WriteLine(Lives);
            base.Position.y -= Vitesse * deltaT;
            Console.WriteLine(base.Position.Y);

            // CONTROLE DU MISSILE HORS LIMITE VERTICALE
            if(base.Position.y < -30)
            {
                Lives = 0;
            }
            Console.WriteLine(Lives);
        }

        
    }


}
