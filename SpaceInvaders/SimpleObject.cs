using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SpaceInvaders
{
    abstract class SimpleObject : GameObject
    {
        readonly private Bitmap image;
        private int lives;
        readonly private Vecteur2D position;


        public SimpleObject(Bitmap image, int lives, Vecteur2D position, Side side) : base(side)
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


        protected abstract void OnCollision(Missile m);

        public bool TestCollisionRectangles(Missile missile)
        {
            Rectangle objectRectangle = new Rectangle((int)Position.X, (int)Position.Y, Image.Width, Image.Height);
            Rectangle missileRectangle = new Rectangle((int)missile.Position.X, (int)missile.Position.Y, missile.Image.Width, missile.Image.Height);

            return objectRectangle.IntersectsWith(missileRectangle);
        }


        public void TestCollisionPixels(Missile missile)
        {
            for (int x = 0; x < missile.Image.Width; x++)
            {
                for (int y = 0; y < missile.Image.Height; y++)
                {
                    int objectX = (int)(missile.Position.X + x - Position.X);
                    int objectY = (int)(missile.Position.Y + y - Position.Y);

                    if (objectX >= 0 && objectX < Image.Width && objectY >= 0 && objectY < Image.Height)
                    {
                        Color objectPixel = Image.GetPixel(objectX, objectY);
                        Color missilePixel = missile.Image.GetPixel(x, y);

                        if (objectPixel.A > 0 && missilePixel.A > 0)
                        {
                            // collision detected so decrease lives of missile
                            Image.SetPixel(objectX, objectY, Color.FromArgb(0, 0, 0, 0));
                            missile.Lives = 0;
                        }
                    }
                }
            }
        }
        
        public override void Collision(Missile missile)
        {
            if (missile.ObjectSide == this.ObjectSide) return;

            OnCollision(missile);
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
