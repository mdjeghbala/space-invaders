using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceInvaders
{
    class PlayerSpaceship : SpaceShip
    {
        private double speedPixelPerSecond = 100;
        public PlayerSpaceship(Vecteur2D position, int lives, Bitmap image, Side side) : base(position, lives, image, side)
        {

        }


        public override void Update(Game gameInstance, double deltaT)
        {
            // DEPLACEMENT LATERAL
            if (gameInstance.keyPressed.Contains(Keys.Left))
            {
                base.Position.x -= speedPixelPerSecond * deltaT;
            }
            if (gameInstance.keyPressed.Contains(Keys.Right))
            {
                base.Position.x += speedPixelPerSecond * deltaT;
            }

            // CONTROLE LA SORTIE HORS DE LA ZONE DE JEU HORIZONTALE
            if (base.Position.X > gameInstance.gameSize.Width - 25)
            {
                base.Position.x = (gameInstance.gameSize.Width) - 25;
            }
            if (base.Position.X < 0)
            {
                base.Position.x = 0;
            }

            if (gameInstance.keyPressed.Contains(Keys.Up))
            {
                this.shoot(gameInstance);
                gameInstance.ReleaseKey(Keys.Up);
            }
        }


        public override void Draw(Game gameInstance, Graphics graphics)
        {
            graphics.DrawString("Vie : " + base.Lives, new Font(new FontFamily("Times New Roman"), 22, FontStyle.Bold, GraphicsUnit.Pixel), 
            new SolidBrush(Color.Black), (gameInstance.gameSize.Width / 2) - 290, (gameInstance.gameSize.Height / 2) + 257);

            base.Draw(gameInstance, graphics);
        }


    }
}



