using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Kirby_New_Adventure
{
    public static class Images
    {
        public readonly static ImageSource Empty = LoadImages("Empty.png");
        public readonly static ImageSource Tierra = LoadImages("Pasto1.png");
        public readonly static ImageSource Kirby = LoadImages("a.png");
        public readonly static ImageSource Food = LoadImages("Comida/Cereza.png");
        public readonly static ImageSource Roca = LoadImages("Roca22.png");
        public readonly static ImageSource Vacio = LoadImages("Hoyo22.png");
        public readonly static ImageSource Estrella = LoadImages("Estrella.png");
        public readonly static ImageSource RocaFalsa = LoadImages("PiedraRota.png");
        public readonly static ImageSource RocaFalsaDestruida = LoadImages("rocaf2.png");
        public readonly static ImageSource Boton = LoadImages("Boton.png");
        public readonly static ImageSource BotonPresionado = LoadImages("BotonPresionado.png");
        public readonly static ImageSource RocaFalsaSemidestruida = LoadImages("PiedraRota.png");

        private static ImageSource LoadImages(string filename)
        {
            return new BitmapImage(new Uri($"Assets/{filename}", UriKind.Relative));
        }
        public static List<BitmapImage> kirbos = new  List<BitmapImage>();
        public static List<BitmapImage> kirbos_e = new List<BitmapImage>();
        public static List<BitmapImage> kirbos_f = new List<BitmapImage>();
        public static List<BitmapImage> kirbos_i = new List<BitmapImage>();
        //public static List<BitmapImage> Dididis = new();
        public  static void RellenarKirbos()
        {
            for (int i = 1; i < 11; i++)
            {
                kirbos.Add( (BitmapImage)LoadImages($"Kirbis/k{i}.png"));
                kirbos_i.Add((BitmapImage)LoadImages($"Kirbis_izquierda/ki{i}.png"));
                kirbos_f.Add((BitmapImage)LoadImages($"Kirbis_frente/kf{i}.png"));
            }
            for (int i = 1; i < 5; i++)
            {
                kirbos_e.Add((BitmapImage)LoadImages($"Kirbis_espalda/ke{i}.png"));
            }
            //for (int i = 1; i < 5; i++)
            //{
            //    Dididis.Add((BitmapImage)LoadImages($"Sprites lvl4/JefeMov{i}.png"));
            //}
           
        }
    }
}
