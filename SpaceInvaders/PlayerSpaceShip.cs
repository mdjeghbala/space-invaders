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

        private bool forMultiplayer;

        public PlayerSpaceship(Vecteur2D position, int lives, Bitmap image, Side side, bool multiplayer) : base(position, lives, image, side)
        {
            this.forMultiplayer = multiplayer;
        }


        public override void Update(Game gameInstance, double deltaT)
        {
            // DEPLACEMENT LATERAL
            if (!this.forMultiplayer)
            {
                if (gameInstance.keyPressed.Contains(Keys.Left))
                {
                    base.Position.x -= speedPixelPerSecond * deltaT;
                }
                if (gameInstance.keyPressed.Contains(Keys.Right))
                {
                    base.Position.x += speedPixelPerSecond * deltaT;
                }

                if (gameInstance.keyPressed.Contains(Keys.Up))
                {
                    this.shoot(gameInstance);
                    gameInstance.ReleaseKey(Keys.Space);
                }
            }

            if(this.forMultiplayer)
            {
                if (gameInstance.keyPressed.Contains(Keys.S))
                {
                    base.Position.x -= speedPixelPerSecond * deltaT;
                }
                if (gameInstance.keyPressed.Contains(Keys.D))
                {
                    base.Position.x += speedPixelPerSecond * deltaT;
                }

                if (gameInstance.keyPressed.Contains(Keys.Space))
                {
                    this.shoot(gameInstance);
                    gameInstance.ReleaseKey(Keys.Space);
                }
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

            
        }


        public override void Draw(Game gameInstance, Graphics graphics)
        {
            float x = 0, y = 0;

            if (!this.forMultiplayer)
            {
                x = (gameInstance.gameSize.Width / 2) - 297;
                y = (gameInstance.gameSize.Height / 2) + 257;
            }

            if(this.forMultiplayer) {
                x = (gameInstance.gameSize.Width) - 76;
                y = (gameInstance.gameSize.Height / 2) + 257;
            }
            
            graphics.DrawString("Vie : " + base.Lives, new Font(new FontFamily("Times New Roman"), 22, FontStyle.Bold, GraphicsUnit.Pixel), 
            new SolidBrush(Color.White), x, y);

            base.Draw(gameInstance, graphics);
        }


    }
}



