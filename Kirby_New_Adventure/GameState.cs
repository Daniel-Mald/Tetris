using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Media;
using System.Text;
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
      //  private Direction KirbyDirection { get; set; }  
        public bool GameOver { get; private set; }
        public int state { get; private set; }
        public string Dificultad { get; set; }
        bool ControlPiedra = false;
        public bool Ganar { get; set; }
        List<string> cadena = new List<string>();

        //Delegado y eventos
        public delegate void Delegado();
        public event Delegado Al_Ganar;
        public event Delegado Al_Perder;
        public event Delegado Al_Caer;


        private readonly Random random = new Random();
        public SoundPlayer p = new();
        string ruta = "";
      


        public GameState(int rows, int cols, string dif)
        {
            Mapa Ma = new Mapa(rows, cols, 1);
            Rows = rows; Cols = cols;
            Al_Caer += GameState_Al_Caer;
            Al_Ganar += GameState_Al_Ganar;
            Al_Perder += GameState_Al_Perder;
            Grid = Ma.MapaActual;
            KirbyPosiion = Ma.KirbyPos;
            KIrbyPosInitial = Ma.KirbyPos;
            JuanPosition = Ma.EstrellaPosition;
            Ganar = false;
            if (dif == "Facil")
            {
                Vidas = 10;
                Moves = 30;
            }
                
            else if (dif == "Dificil")
            {
                Vidas = 1;
                Moves = 11;
            }
            else
            {
                Vidas = 3;
                Moves = 15;
            }
                
            Dir = Direction.Right;

            
           
        }

        private void GameState_Al_Perder()
        {
            GameOver = true;
        }

        private void GameState_Al_Ganar()
        {
            GameOver = true;
        }

        public async void GameState_Al_Caer()
        {
            //SetKirby(HeadPosition().Translate(Dir));
            //await Task.Delay(300);
            
            Vidas--;
           // Moves--;
            //KirbyPosiion = KIrbyPosInitial;
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
        public void ValidarPos(Position _newPos)
        {
            GridValue hit = WillHit(_newPos);
            if (_newPos == JuanPosition)
            {
                Ganar = true;
                Al_Ganar?.Invoke();
                return;
            }
            if (hit == GridValue.Empty || hit == GridValue.Vacio)
            {
                Al_Caer?.Invoke();
                return;
            }
            if (hit == GridValue.Roca || hit == GridValue.Outside || hit == GridValue.Empty)
            {
                ControlPiedra = true;
                Perdio();
                return;
            }
        }
        public void Move()
        {
            //Ver la siguiente posiscion
            Position newHeadPos = HeadPosition().Translate(Dir);
            if (!ControlPiedra)
            {
                SetKirby(newHeadPos);
                
            }
               
            
            Moves--;

            //GridValue hit = WillHit(newHeadPos);


            //if(newHeadPos == JuanPosition)
            //{
            //    Ganar = true;
            //    Al_Ganar?.Invoke();              
            //    return;
            //}
            //if (hit == GridValue.Empty || hit == GridValue.Vacio )
            //{  
            //    Al_Caer?.Invoke();
            //    return;
            //}
            //if (hit == GridValue.Roca || hit == GridValue.Outside || hit == GridValue.Empty)
            //{
            //    Moves--;
            //    Perdio();
            //    return;
            //}




        }
    }
}
