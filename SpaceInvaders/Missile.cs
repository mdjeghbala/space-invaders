using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    class Missile : GameObject
    {
        private double vitesse = 150;
        private Vecteur2D position;
        private int lives;
        private Bitmap image;

        public Missile(Vecteur2D position, int lives, Bitmap image) : base()
        {
            this.position = position;
            this.lives = lives;
            this.image = image;
        }

        public double Vitesse
        {
            get { return vitesse; }
        }

        public Vecteur2D Position
        {
            get { return position; }
        }

        public int Lives
        {
            get { return lives; }
            set { lives = value; }
        }

        public Bitmap Image
        {
            get { return image; }
        }

        public override void Update(Game gameInstance, double deltaT)
        {
            // DEPLACEMENT VERTICAL DU MISSILE
            Console.WriteLine(Lives);
            Position.y -= Vitesse * deltaT;
            Console.WriteLine(Position.Y);

            // CONTROLE DU MISSILE HORS LIMITE VERTICALE
            if(Position.y < -30)
            {
                Lives = 0;
            }
            Console.WriteLine(Lives);
        }


        public override void Draw(Game gameInstance, Graphics graphics)
        {
            if (Image != null)
            {
                graphics.DrawImage(Image, (float)Position.X, (float)Position.Y, (float)Image.Width, (float)Image.Height);
            }
        }

        public override bool IsAlive()
        {
            return Lives > 0;
        }

        
    }


}
