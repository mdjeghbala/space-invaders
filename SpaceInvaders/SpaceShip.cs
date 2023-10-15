using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace SpaceInvaders
{
    class SpaceShip : SimpleObject
    {
        private double speedPixelPerSecond = 100;
        private Missile missile;


        public SpaceShip(Vecteur2D position, int lives, Bitmap image) : base(image, lives, position)
        {
        }

        public void shoot(Game gameInstance)
        {
            if (this.missile == null || !this.missile.IsAlive())
            {
                this.missile = new Missile(new Vecteur2D(base.Position.X + 10, base.Position.Y - 20), 1, SpaceInvaders.Properties.Resources.shoot1);
                gameInstance.AddNewGameObject(this.missile);
            }

        }

        public override void Update(Game gameInstance, double deltaT)
        {
            // DEPLACEMENT LATERAL
            if (gameInstance.keyPressed.Contains(Keys.Left))
            {
                base.Position.x -= speedPixelPerSecond * deltaT;
                Console.WriteLine("Gauche : " + base.Position.X);
            }
            if (gameInstance.keyPressed.Contains(Keys.Right))
            {
                base.Position.x += speedPixelPerSecond * deltaT;
                Console.WriteLine("Droite : " + base.Position.X);
            }

            // CONTROLE LA SORTIE HORS DE LA ZONE DE JEU HORIZONTALE
            if (base.Position.X > gameInstance.gameSize.Width - 25)
            {
                base.Position.x = (gameInstance.gameSize.Width) - 25;
            }
            if(base.Position.X < 0)
            {
                base.Position.x = 0;
            }

            if (gameInstance.keyPressed.Contains(Keys.Up))
            {
                this.shoot(gameInstance);
            }
        }


    }

}
