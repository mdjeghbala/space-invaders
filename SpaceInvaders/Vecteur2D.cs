using System;

namespace SpaceInvaders
{
    class Vecteur2D
    {
        public double x;
        public double y;


        public Vecteur2D(double x = 0, double y = 0)
        {
            this.x = x;
            this.y = y;
        }


        public double X
        {
            get { return x; }
            set { x = value; }
        }


        public double Y
        {
            get { return y; }
            set { y = value; }
        }


        public double Norme
        {
            get
            {
                return Math.Sqrt(x * x + y * y);
            }
        }


        public static Vecteur2D operator +(Vecteur2D v1, Vecteur2D v2)
        {
            return new Vecteur2D(v1.x + v2.x, v1.y + v2.y);
        }


        public static Vecteur2D operator -(Vecteur2D v1, Vecteur2D v2)
        {
            return new Vecteur2D(v1.x - v2.x, v1.y - v2.y);
        }


        public static Vecteur2D MoinsUnaire(Vecteur2D v1)
        {
            return new Vecteur2D(-v1.x, -v1.y);
        }


        public static Vecteur2D operator *(Vecteur2D v1, double scalaireDroite)
        {
            return new Vecteur2D(v1.x * scalaireDroite, v1.y * scalaireDroite);
        }


        public static Vecteur2D operator *(double scalaireGauche, Vecteur2D v1)
        {
            return new Vecteur2D(scalaireGauche * v1.x , scalaireGauche * v1.y);
        }


        public static Vecteur2D operator /(Vecteur2D v1, double scalaire)
        {
            return new Vecteur2D( v1.x / scalaire, v1.y / scalaire);
        }
    }

}