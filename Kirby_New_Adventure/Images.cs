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
        public readonly static ImageSource Tierra = LoadImages("Tierra.png");
        public readonly static ImageSource Kirby = LoadImages("KirbyRight.gif");
        public readonly static ImageSource Food = LoadImages("Food.png");
        public readonly static ImageSource Roca = LoadImages("Roca.png");
        public readonly static ImageSource Vacio = LoadImages("Vacio.png");
        public readonly static ImageSource Juan = LoadImages("juan.png");

        private static ImageSource LoadImages(string filename)
        {
            return new BitmapImage(new Uri($"Assets/{filename}", UriKind.Relative));
        }
    }
}
