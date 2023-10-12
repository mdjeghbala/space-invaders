using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace SpaceInvaders
{
    class SpaceShip : GameObject
    {
        private double speedPixelPerSecond = 100;
        private Missile missile;


        public SpaceShip(Vecteur2D position, int lives, Bitmap image) : base()
        {
        }

        public void shoot(Game gameInstance)
        {
            if(this.missile == null || !(this.missile.IsAlive()))
            {
                this.missile = new Missile(new Vecteur2D(this.Position.X + 10, this.Position.Y - 20), 1, SpaceInvaders.Properties.Resources.shoot1);
                gameInstance.AddNewGameObject(this.missile);
            }
        }

        public override void Update(Game gameInstance, double deltaT)
        {
            // DEPLACEMENT LATERAL
            if (gameInstance.keyPressed.Contains(Keys.Left))
            {
                Position.x -= speedPixelPerSecond * deltaT;
                Console.WriteLine("Gauche : " + Position.X);
            }
            if (gameInstance.keyPressed.Contains(Keys.Right))
            {
                Position.x += speedPixelPerSecond * deltaT;
                Console.WriteLine("Droite : " + Position.X);
            }

            // CONTROLE LA SORTIE HORS DE LA ZONE DE JEU HORIZONTALE
            if (Position.X > gameInstance.gameSize.Width - 25)
            {
                Position.x = (gameInstance.gameSize.Width) - 25;
            }
            if(Position.X < 0)
            {
                Position.x = 0;
            }

            if (gameInstance.keyPressed.Contains(Keys.Up))
            {
                this.shoot(gameInstance);
            }
        }


    }

}
