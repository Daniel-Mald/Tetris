using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Media;
using System.Text;
using System.Resources;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Kirby_New_Adventure
{
    public  class GameState
    {
        public int Rows { get; }
        public int Cols { get; }
        public int PosicionAnterior { get; set; }
        public GridValue[,] Grid { get; private set; }
        public Direction Dir { get; private set; }
        public int Score { get; private set; }
        public int Vidas { get; private set; }
        public int Moves { get; private set; }
        public Position KirbyPosiion { get; set; }
        public Position JuanPosition { get; set; }
        public Position KIrbyPosInitial { get; set; }
        public Position BotonPosition { get; set; }
      //  private Direction KirbyDirection { get; set; }  
        public bool GameOver { get; private set; }
        public int state { get; private set; }
        public string Dificultad { get; set; }
        bool ControlPiedra = false;
        public bool Ganar { get; set; }
        public bool ControlCaida { get; private set; } = false;

       // public List<char> CadenaDePasos = new List<char>();
        public string CadenaDePasos { get; set; } = "";
        //Delegado y eventos
        public delegate void Delegado();
        public event Delegado Al_Ganar;
        public event Delegado Al_Perder;
        public event Delegado Al_Caer;
        public event Delegado Al_presionar_boton;

        public bool MinimoMovs { get; set; } = false;
        private readonly Random random = new Random();
        public SoundPlayer CaerSonido = new SoundPlayer("../Assets/Fall_sound.wav");
        public SoundPlayer MorirSonido = new("file://application/Assets/Death_sound.wav");

        public List<Comidaa> Pos_Comida;
        public List<PiedrasFalsas> Pos_PiedrasF;
        public bool BotonPresionado = false;
      


        public GameState(int rows, int cols, string dificultad, int nivel)
        {
            Mapa Ma = new Mapa(rows, cols, nivel);
            Rows = rows; Cols = cols;
            Al_Caer += GameState_Al_Caer;
            Al_Ganar += GameState_Al_Ganar;
            Al_Perder += GameState_Al_Perder;
            Pos_Comida = new();
            Pos_PiedrasF = new();
            Grid = Ma.MapaActual;
            KirbyPosiion = Ma.KirbyPos;
            KIrbyPosInitial = Ma.KirbyPos;
            JuanPosition = Ma.EstrellaPosition;
            Ganar = false;
            Images.RellenarKirbos();
            if(nivel == 1)
            {
                if (dificultad == "Facil")
                {
                    Vidas = 6;
                    Moves = 20;
                }

                else if (dificultad == "Dificil")
                {
                    Vidas = 1;
                    Moves = 11;
                }
                else
                {
                    Vidas = 3;
                    Moves = 15;
                }
            }
            else if(nivel == 2)
            {
                if (dificultad == "Facil")
                {
                    Vidas = 6;
                    Moves = 35;
                }

                else if (dificultad == "Dificil")
                {
                    Vidas = 1;
                    Moves = 27;
                }
                else
                {
                    Vidas = 3;
                    Moves = 31;
                }
            }
            else if(nivel == 3)
            {
                if (dificultad == "Facil")
                {
                    Vidas = 6;
                    Moves = 35;
                }

                else if (dificultad == "Dificil")
                {
                    Vidas = 1;
                    Moves = 27;
                }
                else
                {
                    Vidas = 3;
                    Moves = 31;
                }

                foreach (var item in Ma.PiedrasF)
                {
                    Pos_PiedrasF.Add(new PiedrasFalsas
                    {
                        Posision = item
                    });
                }
                BotonPosition = Ma.Boton;
            }
            foreach (var item in Ma.Comida)
            {
                Pos_Comida.Add(new Comidaa
                {
                    Posision = item,
                    Consumida = false
                });
            }

            
                
            Dir = Direction.Right;

            
           
        }
        public bool  ValidarCadena()
        {
            if (CadenaDePasos == "sdddsdsddds")
            {
                
                return true;
            }
            return false;
        }

        private void GameState_Al_Perder()
        {
            GameOver = true;
            //MorirSonido.PlaySync();
        }

        private void GameState_Al_Ganar()
        {
            MinimoMovs = false;
            GameOver = true;
            if (ValidarCadena())
            {
                MinimoMovs = true;
            }
        }

        public async void GameState_Al_Caer()
        {
            //SetKirby(HeadPosition().Translate(Dir));
            //await Task.Delay(300);
            
            Vidas--;
            // Moves--;
            //KirbyPosiion = KIrbyPosInitial;
            
            
            //CaerSonido.PlaySync();
            SetKirby(KIrbyPosInitial);
            Perdio();
        }

        public Position HeadPosition()
        {
            return KirbyPosiion;
        }
        
        private void SetKirby(Position pos)
        {      
            KirbyPosiion = pos; 
        }

        public void ChangeDirection(Direction dir)
        {          
            Dir = dir;
        }
        private bool OutsideGrid(Position pos)
        {
            return pos.Row < 0 || pos.Row >= Rows || pos.Col < 0 || pos.Col >= Cols;
        }
        private GridValue WillHit(Position newHeadPos)
        {
            if (OutsideGrid(newHeadPos))
            {
                return GridValue.Outside;
            }           
            return Grid[newHeadPos.Row, newHeadPos.Col];
        }
        public bool Perdio()
        {
            if (Vidas == 0 || Moves == 0)
            {
                Al_Perder?.Invoke();
                return true;
            }
            return false;
        }
        public void ValidarPos()

        {
            Position _newPos = HeadPosition().Translate(Dir);
            GridValue hit = WillHit(_newPos);
            Perdio();
            if (_newPos == JuanPosition)
            {
                Ganar = true;

                Al_Ganar?.Invoke();
                return;
            }
            if (hit == GridValue.Vacio)
            {
                Vidas--;
                ControlCaida = true;
                return;
            }
            if (hit == GridValue.Roca || hit == GridValue.Outside || hit == GridValue.Empty || hit == GridValue.RocaFalsa)
            {
                ControlPiedra = true;

                return;
            }
            if(_newPos == BotonPosition)
            {
                BotonPresionado = true;
                foreach(var item in Pos_PiedrasF)
                {
                    item.Destruida = true;
                    Grid[item.Posision.Row, item.Posision.Col] = GridValue.RocaFalsaDestruida;
                }
                Al_presionar_boton?.Invoke();
                return;
            }

        }
        public void Move()
        {
            //Ver la siguiente posiscion
            Position newHeadPos = HeadPosition().Translate(Dir);
            if (ControlPiedra == true)
            {
                ControlPiedra = false;
                
                
            }
            else if (ControlCaida == true)
            {
                ControlCaida = false;
                SetKirby(newHeadPos);
               // Al_Caer?.Invoke();
                //await Task.Delay(150);
                Task.Delay(100);
                SetKirby(KIrbyPosInitial);
                
            }
            else
            {
                SetKirby(newHeadPos);
            }
            Perdio();

            Moves--;

            



        }
        public class Comidaa
        {
            public Position Posision { get; set; } = null!;
            public bool Consumida { get; set; }
            
        }
        public class PiedrasFalsas
        {
            public Position Posision { get; set; } = null!;
            public bool Destruida { get; set; } = false;
        }
    }
}
