using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Kirby_New_Adventure
{
    public class Mapa
    {
        public GridValue[,] MapaActual;
        public Position KirbyPos;
        public Position EstrellaPosition;
        public Mapa(int rows, int cols, int nivel)
        {
            MapaActual = new GridValue[rows,cols];
            CargarMapa(nivel, rows, cols); 
        }
        private void CargarMapa(int nivel, int row, int col)
        {

           MapaActual = new GridValue[row, col];
            for (int c = 0; c < col; c++)
            {
                for (int r = 0; r < row; r++)
                {
                    if (r == 1 && c == 4 || r == 1 && c == 5 || r == 3 && c == 1 || r== 0 && c == 1
                        || r == 0 && c == 2 || r == 2 && c == 1 || r == 2 && c == 2 || r == 4 && c == 5 || r == 4 && c == 6
                        || r == 0 && c == 1)
                        MapaActual[r, c] = GridValue.Vacio;
                    else if (r == 1 && c == 6 || r == 2 && c == 5 || r == 3 && c == 2 || r == 3 && c == 3
                        || r == 2 && c == 0 || r == 3 && c == 0 || r == 4 && c == 0
                        || r == 2 && c == 6 || r == 2 && c == 7 || r == 4 && c == 1)
                        MapaActual[r, c] = GridValue.Roca;
                    else
                        MapaActual[r, c] = GridValue.Tierra;
                }
            }

            KirbyPos = new Position(0, 0);
            EstrellaPosition = new Position(4, 7);


        }
    }
}
