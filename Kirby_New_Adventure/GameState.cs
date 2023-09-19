using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kirby_New_Adventure
{
    public class GameState
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
        public bool Ganar { get; set; }
        

        private readonly Random random = new Random();

        public GameState(int rows, int cols, string dif)
        {
            Mapa Ma = new Mapa(rows, cols, 1);
            Rows = rows; Cols = cols;
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
                Moves = 15;
            }
            else
            {
                Vidas = 3;
                Moves = 11;
            }
                
            Dir = Direction.Right;

            
           
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
        public void Move()
        {
            
            Position newHeadPos = HeadPosition().Translate(Dir);
            GridValue hit = WillHit(newHeadPos);
            
            if(Vidas == 0 || Moves == 0)
            {
                GameOver = true;
                return;
            }
            if(newHeadPos == JuanPosition)
            {
                Ganar = true;
                GameOver = true;
                
                return;
            }
            if (hit == GridValue.Empty || hit == GridValue.Vacio )
            {
                
                Vidas--;
                
                KirbyPosiion = KIrbyPosInitial;
            }
            else if (hit == GridValue.Roca || hit == GridValue.Outside || hit == GridValue.Empty)
            {
                Moves--;
                return;
            }
            else
            {
                SetKirby( newHeadPos);
               
            }
            Moves--;

        }
    }
}
