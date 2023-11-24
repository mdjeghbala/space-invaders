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

        public Missile(Vecteur2D position, int lives, Bitmap image, Side side) : base(image, lives, position, side)
        {
        }

        public double Vitesse
        {
            get { return vitesse; }
        }

        public override void Update(Game gameInstance, double deltaT)
        {
            // Mise à jour de la position verticale du missile en fonction de sa vitesse
            base.Position.Y -= Vitesse * deltaT;

            // Contrôle de la sortie du missile hors de l'écran vertical
            if (base.Position.Y < -30)
            {
                Lives = 0; // Détruire le missile s'il sort de l'écran
            }

            // Gestion des collisions avec d'autres objets (à implémenter)
            foreach (GameObject gameObject in gameInstance.gameObjects)
            {
                gameObject.Collision(this);
            }
        }


        protected override void OnCollision(Missile missile, int numberOfPixelsInCollision)
        {

        }

    }



}
