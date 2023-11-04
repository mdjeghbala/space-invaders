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

        public bool TestCollisionRectangles(Missile missile)
        {
            Rectangle bunkerRectangle = new Rectangle((int)Position.X, (int)Position.Y, Image.Width, Image.Height);
            Rectangle missileRectangle = new Rectangle((int)missile.Position.X, (int)missile.Position.Y, missile.Image.Width, missile.Image.Height);

            return bunkerRectangle.IntersectsWith(missileRectangle);
        }

        public void TestCollisionPixels(Missile missile)
        {
            for (int x = 0; x < missile.Image.Width; x++)
            {
                for (int y = 0; y < missile.Image.Height; y++)
                {
                    int bunkerX = (int)(missile.Position.X + x - Position.X);
                    int bunkerY = (int)(missile.Position.Y + y - Position.Y);

                    if (bunkerX >= 0 && bunkerX < Image.Width && bunkerY >= 0 && bunkerY < Image.Height)
                    {
                        // Vérifie la couleur alpha du pixel du bunker et du missile
                        Color bunkerPixel = Image.GetPixel(bunkerX, bunkerY);
                        Color missilePixel = missile.Image.GetPixel(x, y);

                        if (bunkerPixel.A > 0 && missilePixel.A > 0)
                        {
                            // Collision détectée : marque le pixel du bunker comme transparent (couleur alpha à 0)
                            Image.SetPixel(bunkerX, bunkerY, Color.FromArgb(0, 0, 0, 0));
                            // Réduit le nombre de vies du missile
                            missile.Lives--;
                        }
                    }
                }
            }
        }


        public override void Collision(Missile missile)
        {
            if (TestCollisionRectangles(missile))
            {
                TestCollisionPixels(missile);
            }
        }


        public override void Update(Game gameInstance, double deltaT)
        {
        }

    }

}
