using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    abstract class SimpleObject : GameObject
    {
        private Bitmap image;
        private int lives;
        private Vecteur2D position;

        public SimpleObject(Bitmap image, int lives, Vecteur2D position) 
        {
            this.image = image;
            this.lives = lives;
            this.position = position;
        }

        public Vecteur2D Position
        {
            get { return this.position; }
        }

        public int Lives
        {
            get { return this.lives; }
            set { lives = value; }
        }

        public Bitmap Image
        {
            get { return this.image; }
        }

        public override void Draw(Game gameInstance, Graphics graphics)
        {
            if (Image != null)
                graphics.DrawImage(image, (float)Position.X, (float)Position.Y, (float)Image.Width, (float)Image.Height);
        }

        public override bool IsAlive()
        {
            return Lives > 0;
        }

    }
}
