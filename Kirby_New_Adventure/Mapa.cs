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
        public Position Boton;
        public List<Position> Comida;
        public List<Position> PiedrasF;
        public List<Position> Botones;
        public Mapa(int rows, int cols, int nivel)
        {
            Comida = new();
            PiedrasF = new List<Position>();
            MapaActual = new GridValue[rows,cols];
            if(nivel == 4)
            {
                Botones = new List<Position>();
            }
            CargarMapa(nivel, rows, cols);
            
        }
        private void CargarMapa(int nivel, int row, int col)
        {

           MapaActual = new GridValue[row, col];
            if(nivel == 1)
            {
                for (int c = 0; c < col; c++)
                {
                    for (int r = 0; r < row; r++)
                    {
                        if (r == 1 && c == 4 || r == 1 && c == 5 || r == 3 && c == 1 || r == 0 && c == 1
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
            else if (nivel == 2)
            {
                for (int c = 0; c < col; c++)
                {
                    for (int r = 0; r < row; r++)
                    {
                        if (r == 0 && c == 1 || r == 0 && c == 2 || r == 0 && c == 3 || r == 1 && c == 1
                            || r == 1 && c == 6 || r == 2 && c == 5 || r == 3 && c == 3 ||
                            r == 4 && c == 5)//para hoyos
                            MapaActual[r, c] = GridValue.Vacio;
                        else if (r == 0 && c == 7 || r == 1 && c == 5 || r == 1 && c == 7 || r == 2 && c == 7 ||
                            r == 3 && c == 1 || r == 3 && c == 2 || r == 4 && c == 6) //para piedras
                            MapaActual[r, c] = GridValue.Roca;
                        else if (r == 4 && c == 1 || r == 1 && c == 2 || r == 0 && c == 6)
                        {
                            MapaActual[r, c] = GridValue.Food;
                            Comida.Add(new Position(r, c));
                        }
                        else
                            MapaActual[r, c] = GridValue.Tierra;
                    }
                }

                KirbyPos = new Position(0, 0);
                EstrellaPosition = new Position(4, 7);
            }
            else if (nivel == 3)
            {
                for (int c = 0; c < col; c++)
                {
                    for (int r = 0; r < row; r++)
                    {
                        if (r == 4 && c == 0 || r == 4 && c == 1 || r == 1 && c == 2
                    || r == 4 && c == 5 || r == 1 && c == 6 || r == 2 && c == 6)//para hoyos
                            MapaActual[r, c] = GridValue.Vacio;

                        else if (r == 3 && c == 1 || r == 0 && c == 3 || r == 1 && c == 3 || r == 4 && c == 3 ||
                            r == 0 && c == 4 || r == 4 && c == 4 || r == 2 && c == 7) //para piedras
                            MapaActual[r, c] = GridValue.Roca;

                        else if (r == 3 && c == 4 || r == 3 && c == 5 || r == 3 && c == 6)//para piedras que explotan
                        {
                            MapaActual[r, c] = GridValue.RocaFalsa;
                            PiedrasF.Add(new Position(r, c));
                        }

                        else if (r == 3 && c == 0 || r == 2 && c == 4 || r == 0 && c == 5 || r == 4 && c == 6)
                        {
                            MapaActual[r, c] = GridValue.Food;
                            Comida.Add(new Position(r, c));
                        }
                        else
                            MapaActual[r, c] = GridValue.Tierra;
                    }
                }
                KirbyPos = new Position(0, 0);
                EstrellaPosition = new Position(4, 7);
                Boton = new Position(1, 7);
            }
            else if(nivel == 4)
            {
                for (int c = 0; c < col; c++)
                {
                    for (int r = 0; r < row; r++)
                    {
                        if (r == 1 && c == 2 || r == 1 && c == 3 || r == 0 && c == 8
                    || r == 3 && c == 1 || r == 3 && c == 4 || r == 4 && c == 8 || r == 5 && c == 8 || r == 7 && c == 1 || r == 7 && c == 7)//para hoyos
                            MapaActual[r, c] = GridValue.Vacio;

                        else if (r == 0 && c == 1 || r == 0 && c == 5 || r == 1 && c == 1 || r == 1 && c == 5 ||
                            r == 2 && c == 1 || r == 2 && c == 2 || r == 2 && c == 3 || r == 2 && c == 6 || r == 3 && c == 8 || r == 5 && c == 0 || r == 5 && c == 2
                            || r == 5 && c == 3 || r == 5 && c == 4 || r == 5 && c == 5 || r == 6 && c == 4 || r == 7 && c == 0) //para piedras
                            MapaActual[r, c] = GridValue.Roca;

                        else if (r == 6 && c == 7 || r == 6 && c == 8 )//para piedras que explotan
                        {
                            MapaActual[r, c] = GridValue.RocaFalsa;
                            PiedrasF.Add(new Position(r, c));
                        }

                        else if (r == 1 && c == 8 || r == 2 && c == 5 || r == 4 && c == 4 || r == 7 && c == 2)
                        {
                            MapaActual[r, c] = GridValue.Food;
                            Comida.Add(new Position(r, c));
                        }
                        else if ( r == 0 && c == 2 || r == 1 && c == 6 || r == 6 && c == 3 || r == 7 && c == 6)
                        {
                            Botones.Add(new Position(r, c));
                            MapaActual[r, c] = GridValue.Tierra;
                        }
                        else
                            MapaActual[r, c] = GridValue.Tierra;
                    }
                }
                KirbyPos = new Position(0, 0);
                EstrellaPosition = new Position(7, 8);
               // Boton = new Position(1, 7);//ahora son varios botones
              
            }
            


        }
      
    }
}
