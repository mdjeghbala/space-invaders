using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using NAudio.Wave;

namespace SpaceInvaders
{
    class SpaceShip : SimpleObject
    {
        private Missile missile;

        public Side SpaceshipSide { get; private set; }

        public SpaceShip(Vecteur2D position, int lives, Bitmap image, Side spaceshipside) : base(image, lives, position, spaceshipside)
        {
            SpaceshipSide = spaceshipside;
        }

        public void shoot(Game gameInstance)
        {
            if (this.missile == null || !this.missile.IsAlive())
            {
                // Création d'un nouveau missile en fonction du camp du vaisseau
                Side missileSide = (SpaceshipSide == Side.Ally) ? Side.Ally : Side.Enemy;

                // Ajustement la position de départ du missile en fonction du camp
                int yOffset = (SpaceshipSide == Side.Ally) ? -20 : 20;

                // Création du missile
                this.missile = new Missile(new Vecteur2D(base.Position.X + 10, base.Position.Y + yOffset), 1, SpaceInvaders.Properties.Resources.shoot1, missileSide);

                // Ajout du missile au jeu
                gameInstance.AddNewGameObject(this.missile);

                AudioFileReader audioFile = new AudioFileReader("Resources\\laserShoot.wav");
                WaveOutEvent waveOutEvent = new WaveOutEvent();

                waveOutEvent.Init(audioFile);
                waveOutEvent.Volume = 0.9f;
                waveOutEvent.Play();
            }

        }

        public override void Update(Game gameInstance, double deltaT)
        {
            
        }

        protected override void OnCollision(Missile missile, int numberOfPixelsInCollision)
        {
            if (TestCollisionRectangles(missile))
            {
                //Reduction de la vie lorsqu'un vaisseau est touché 
                Lives--;
                missile.Lives--;
                if(SpaceshipSide == Side.Enemy)
                {
                    Game.score += 20;
                }
                else
                {
                    if (Game.score <= 0) Game.score = 0;
                    else Game.score -= 20;
                }
                AudioFileReader audioFile = new AudioFileReader("Resources\\explosionSound.wav");
                WaveOutEvent waveOutEvent = new WaveOutEvent();
                waveOutEvent.Init(audioFile);
                waveOutEvent.Play();
            }
        }
    }

}
