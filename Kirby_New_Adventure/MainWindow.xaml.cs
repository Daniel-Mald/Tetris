﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Kirby_New_Adventure;

namespace Kirby_New_Adventure
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Dictionary<GridValue, ImageSource> gridValToImag = new()
        {
            {GridValue.Empty, Images.Empty },
            {GridValue.Kirby, Images.Kirby },
            {GridValue.Food, Images.Food },
            {GridValue.Tierra, Images.Tierra },
            {GridValue.Roca, Images.Roca },
            {GridValue.Vacio, Images.Vacio },
            {GridValue.Outside, Images.Empty },
            {GridValue.Estrella, Images.Estrella }
        };
        private readonly Dictionary<Direction, int> dirToRotation = new()
        {
            {Direction.Up, 0 }
            , {Direction.Right , 90},
            {Direction.Down , 180 },
            {Direction.Left , 270 }
        };
        private readonly int rows = 5, cols = 8;
        private Image[,] gridImages;
        public GameState gameState ;
        public string Re { get; set; }
        public string Mens { get; set; }
        //private int state = 0;
        private bool gameRunning;
        public string dificultad;

        public delegate void Delegado();
        public event Delegado Al_Ganar;
        public event Delegado Al_Perder;
        public event Delegado Al_Caer;

        SoundPlayer Caida = new SoundPlayer("Assets/Fall_sound.wav");
        public MainWindow()
        {
            InitializeComponent();
            Al_Ganar += MostrarGanar;
           Al_Perder += MostrarPerder;

            //Al_Ganar += MainWindow_Al_Ganar;
            //Al_Perder += MainWindow_Al_Perder;
            //Al_Caer += MainWindow_Al_Caer;
        }

        //private void MainWindow_Al_Caer()
        //{
        //    gameState.Caer();
        //    Caida.Play();
           
        //}

        //private void MainWindow_Al_Perder()
        //{
        //    gameState.Morir();
            
        //}

        //private void MainWindow_Al_Ganar()
        //{
        //    throw new NotImplementedException();
        //}

        private void RunGame()
        {

            Overlay.Visibility = Visibility.Hidden;
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            var tecla = e.Key;
            
                
            
            if (gameRunning == true)
            {
                if (tecla == Key.Left)
                {
                    gameState.ChangeDirection(Direction.Left);
                }
                else if (tecla == Key.Right)
                {
                    gameState.ChangeDirection(Direction.Right);
                }
                else if (tecla == Key.Up)
                {
                    gameState.ChangeDirection(Direction.Up);
                }
                else if (tecla == Key.Down)
                {
                    gameState.ChangeDirection(Direction.Down);
                }
                else if (tecla != Key.Down&& tecla != Key.Left && tecla != Key.Right && tecla != Key.Up  )
                {
                    return;
                }
                
            }
            //Revisar si no perdio
            //gameState.Perdio();
            GameLoop();




        }

        private async void GameLoop()
        {
            //gameState.Move();
            if (!gameState.GameOver)
            {              
                gameState.Move();
                Draw();
                if (gameState.Ganar == true)
                {
                    await Task.Delay(1000);
                    
                    Al_Ganar?.Invoke();
                    

                }
                
            }
            else
            {
                
                gameRunning = false;
                await Task.Delay(1000);

                
            }         
        }
       
        public void MostrarGanar()
        {
            
            Screnganar.Visibility = Visibility.Visible;
        }
        public void MostrarPerder()
        {
            Screnperder.Visibility = Visibility.Visible;
        }

        private Image[,] SetUpGrid()
        {
            Image[,] images = new Image[rows, cols];
            GameGrid.Children.Clear();
            GameGrid.Rows = rows;
            GameGrid.Columns = cols;
            GameGrid.Width = GameGrid.Height * (cols / (double)rows);
           
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    Image image = new Image
                    {
                        //Source = Images.Empty,
                        RenderTransformOrigin = new Point(0.5, 0.5)
                       
                    };
                    images[r, c] = image;

                    
                    GameGrid.Children.Add(image);

                   
                }
            }
            


            //ShowCountDown();
            // RunGame();
            return images;
            
        }
       
        private void Draw()
        {
            DrawGrid();
            
            

            ScoreText.Text = $"Vidas {gameState.Vidas}- Movimientos {gameState.Moves}";
        }


       
        private void DrawGrid()
        {
            
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    GridValue gridVal = gameState.Grid[r, c];
                    if (gameState.Grid[r, c] != GridValue.Tierra)
                    {
                        
                        gridImages[r,c].Stretch = Stretch.Fill;
                        
                        gridImages[r, c].Source = gridValToImag[gridVal];
                    }
                    else
                    {
                        gridImages[r, c].Source = null;
                    }
                     
                    
                    
                    
                }
            }
            
            Position win = gameState.JuanPosition;
            gridImages[win.Row, win.Col].Source = Images.Estrella;

            Position kirb = gameState.KirbyPosiion;
            gridImages[kirb.Row, kirb.Col].Source = Images.Kirby;
            
        }
        
        

        private void Jugar_Click(object sender, RoutedEventArgs e)
        {
            if(btnfacil.IsChecked == true)
            {
                dificultad = "Facil";
            }
            else if (btndificil.IsChecked ==true)
            {
                dificultad = "Dificil";
            }
            else
            {
                dificultad = "Normal";
            }
            
            gameState = new GameState(rows, cols, dificultad);
            //gameState.Al_Perder += RegresarAInicio;
            //gameState.Al_Ganar += MostrarGanar;
            gameState.Al_Perder += MostrarPerder;

            gridImages = SetUpGrid();

            Draw();
            Screnganar.Visibility = Visibility.Hidden;
            SelectNivel.Visibility = Visibility.Hidden;
            gameRunning = true;
            
        }

        private void btnInicio_Click(object sender, RoutedEventArgs e)
        {
            Overlay.Visibility = Visibility.Hidden;
        }

        private void Regresar_Click(object sender, RoutedEventArgs e)
        {
            Screnganar.Visibility = Visibility.Hidden;
            Screnperder.Visibility = Visibility.Hidden;
            SelectNivel.Visibility = Visibility.Visible;
            Overlay.Visibility = Visibility.Visible;
        }

        private void Reintentar_Click(object sender, RoutedEventArgs e)
        {
            gameState = new GameState(rows, cols, dificultad);
            //gameState.Al_Perder += RegresarAInicio;
            //gameState.Al_Ganar += MostrarGanar;
            gameState.Al_Perder += MostrarPerder;

            gridImages = SetUpGrid();

            Draw();
            Screnganar.Visibility = Visibility.Hidden;
            SelectNivel.Visibility = Visibility.Hidden;
            Screnperder.Visibility = Visibility.Hidden;
            gameRunning = true;
        }

        private void RegresarAInicio()
        {          
            //await Task.Delay(3000);
            
            
            
        }
    }
}