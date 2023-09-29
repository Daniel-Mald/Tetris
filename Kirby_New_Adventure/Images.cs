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
        public readonly static ImageSource Food = LoadImages("Food.png");
        public readonly static ImageSource Roca = LoadImages("Roca_2.png");
        public readonly static ImageSource Vacio = LoadImages("Hoyo1.png");
        public readonly static ImageSource Estrella = LoadImages("Estrella.png");

        private static ImageSource LoadImages(string filename)
        {
            return new BitmapImage(new Uri($"Assets/{filename}", UriKind.Relative));
        }
    }
}
