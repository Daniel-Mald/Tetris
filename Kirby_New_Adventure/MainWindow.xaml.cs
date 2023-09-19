using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            {GridValue.Juan, Images.Juan }
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
        private GameState gameState;
        //private int state = 0;
        private bool gameRunning;
        public string dificultad;
        public MainWindow()
        {
            InitializeComponent();


        }
        private void RunGame()
        {

            Overlay.Visibility = Visibility.Hidden;
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            var tecla = e.Key;
            if (gameState.Vidas == 0 || gameState.Moves == 0)
            {
                gameState.Move();
            }
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
                GameLoop();
            }
            


            //if (gameRunning == true)
            //{
            //    switch (e.Key)
            //    {
            //        case Key.Left:
            //            gameState.ChangeDirection(Direction.Left); break;
            //        case Key.Right:
            //            gameState.ChangeDirection(Direction.Right); break;
            //        case Key.Up:
            //            gameState.ChangeDirection(Direction.Up); break;
            //        case Key.Down:
            //            gameState.ChangeDirection(Direction.Down); break;
            //    }
            //    GameLoop();
            //}
         //hacer metodo de chequeo a ver si perdio
            
        }

        private async void GameLoop()
        {
            if (!gameState.GameOver)
            {              
                gameState.Move();
                Draw();
                if (gameState.Ganar == true)
                {
                    Screnganar.Visibility = Visibility.Visible;
                    await Task.Delay(2000);
                    
                }
            }
            else
            {
                //gameState.Move();
                
                //Draw();
                gameRunning = false;
                await ShowGameOver();
            }         
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
                        Source = Images.Empty,
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
                    gridImages[r, c].Source = gridValToImag[gridVal];
                   
                    //gridImages[r, c].RenderTransform = Transform.Identity;
                }
            }
            
            Position win = gameState.JuanPosition;
            gridImages[win.Row, win.Col].Source = Images.Juan;
            Position kirb = gameState.KirbyPosiion;
            gridImages[kirb.Row, kirb.Col].Source = Images.Kirby;
        }
        
        

        private void Jugar_Click(object sender, RoutedEventArgs e)
        {
            if(btnfacil.IsChecked == true)
            {
                dificultad = "Facil";
            }
            else if (btndificil .IsChecked ==true)
            {
                dificultad = "Dificil";
            }
            else
            {
                dificultad = "Normal";
            }
            
            gameState = new GameState(rows, cols,dificultad );
           
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

        private async Task ShowGameOver()
        {          
            await Task.Delay(400);
            SelectNivel.Visibility = Visibility.Visible;
            Overlay.Visibility = Visibility.Visible;
            
        }
    }
}
