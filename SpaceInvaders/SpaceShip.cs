﻿using System;
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


        public void Shoot(Game gameInstance)
        {
            if (this.missile == null || !this.missile.IsAlive())
            {
                // 
                Side missileSide = (SpaceshipSide == Side.Ally) ? Side.Ally : Side.Enemy;
                int yOffset = (SpaceshipSide == Side.Ally) ? -20 : 20;

                // create and add missile to game list
                this.missile = new Missile(new Vecteur2D(base.Position.X + 10, base.Position.Y + yOffset), 1, SpaceInvaders.Properties.Resources.shoot1, missileSide);
                gameInstance.AddNewGameObject(this.missile);

                // laser sound effect
                AudioFileReader audioFile = new AudioFileReader("Resources\\laserShoot.wav");
                WaveOutEvent waveOutEvent = new WaveOutEvent();

                waveOutEvent.Init(audioFile);
                waveOutEvent.Play();
            }
        }


        public override void Update(Game gameInstance, double deltaT)
        {
            
        }


        protected override void OnCollision(Missile missile)
        {
            if (TestCollisionRectangles(missile))
            {
                // decrease missile and spaceship lives when have collision
                Lives--;
                missile.Lives--;

                if(SpaceshipSide == Side.Enemy) Game.score += 20;      
                else
                    if (Game.score <= 0) Game.score = 0;
                    else Game.score -= 20;
                
                // explosion sound effect
                AudioFileReader audioFile = new AudioFileReader("Resources\\explosionSound.wav");
                WaveOutEvent waveOutEvent = new WaveOutEvent();

                waveOutEvent.Init(audioFile);
                waveOutEvent.Play();
            }
        }


    }

}
