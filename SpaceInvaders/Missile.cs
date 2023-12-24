using NAudio.SoundFont;
using SpaceInvaders.Properties;
using System;
using System.Drawing;
using System.Reflection;
using System.Xml.Linq;

namespace SpaceInvaders
{
    class Missile : SimpleObject
    {
        readonly private double  vitesse = 500;

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
            // update missile vertical position depending of speed et side of ship
            if (MissileSide == Side.Enemy) base.Position.Y += Vitesse * deltaT;
            else if (MissileSide == Side.Ally) base.Position.Y -= Vitesse * deltaT;
            
            // handle missile exit ouf of vertical position
            if (base.Position.Y > gameInstance.gameSize.Height || base.Position.Y < 0) Lives = 0; 
            
            // handle collisiohn with others collision
            foreach (GameObject gameObject in gameInstance.gameObjects) gameObject.Collision(this);
        }


         protected override void OnCollision(Missile otherMissile)
         {
            if (Lives > 0 && otherMissile.Lives > 0 && TestCollisionRectangles(otherMissile))
            {
                Lives = 0;
                otherMissile.Lives = 0;
            }       
        }
    }

}
