using System;
using System.Drawing;


namespace SpaceInvaders
{
    class Missile : SimpleObject
    {
        private double vitesse = 500;

        public Side MissileSide { get; private set; }

        public Missile(Vecteur2D position, int lives, Bitmap image, Side missileSide) : base(image, lives, position, missileSide)
        {
            MissileSide = missileSide;
        }

        public double Vitesse
        {
            get { return vitesse; }
        }

        public override void Update(Game gameInstance, double deltaT)
        {
            // Mise à jour de la position verticale du missile en fonction de sa vitesse et du camp du vaisseau
            if (MissileSide == Side.Enemy)
            {
                base.Position.Y += Vitesse * deltaT;
            }
            else if (MissileSide == Side.Ally)
            {
                base.Position.Y -= Vitesse * deltaT;
            }

            // Contrôle de la sortie du missile hors de l'écran vertical
            if (base.Position.Y > gameInstance.gameSize.Height || base.Position.Y < 0)
            {
                Lives = 0; 
            }

            // Gestion des collisions avec d'autres objets
            foreach (GameObject gameObject in gameInstance.gameObjects)
            {
                gameObject.Collision(this);
            }
        }


        protected override void OnCollision(Missile missile, int numberOfPixelsInCollision)
        {
            if (missile is Missile && TestCollisionRectangles(missile))
            {
                Lives = 0;
            }
        }

    }



}
