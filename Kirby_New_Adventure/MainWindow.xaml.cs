using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using System.Drawing;
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
using System.Windows.Threading;
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
            {GridValue.Estrella, Images.Estrella },
            {GridValue.Boton, Images.Boton },
            {GridValue.BotonPresionado, Images.BotonPresionado },
            {GridValue.RocaFalsa, Images.RocaFalsa },
            {GridValue.RocaFalsaDestruida, Images.RocaFalsaDestruida },
            {GridValue.RocaFalsaSemidestruida , Images.RocaFalsaSemidestruida }
        };
        private readonly Dictionary<Direction, int> dirToRotation = new()
        {
            {Direction.Up, 0 }
            , {Direction.Right , 90},
            {Direction.Down , 180 },
            {Direction.Left , 270 }
        };
        ControlNiveles Nivel = new();
        private  int rows = 5, cols = 8;
        private Image[,] gridImages;
        public GameState gameState;
        public string Re { get; set; }
        public string Mens { get; set; }
        //private int state = 0;
        private bool gameRunning;
        public string dificultad;

        public delegate void Delegado();
        public event Delegado Al_Ganar;
        public event Delegado Al_Perder;
        public event Delegado Al_Caer;

        //SoundPlayer Caida = new SoundPlayer("Assets/Fall_sound.wav");
        public MainWindow()
        {
            InitializeComponent();
            Al_Ganar += MostrarGanar;
            Al_Perder += MostrarPerder;
            
            //playerMovements = Directory.GetFiles("k", "*.png").ToList();
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
        private Dictionary<Key, bool> keyState = new Dictionary<Key, bool>();

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(gameState == null) return;
            var tecla = e.Key;
            if (keyState.ContainsKey(tecla) && keyState[tecla] == true)
            {
                // La tecla ya ha sido registrada, no hagas nada.
            }
            else
            {
                keyState[e.Key] = true;
                if (tecla != Key.Down && tecla != Key.Left && tecla != Key.Right && tecla != Key.Up)
                {
                    return;
                }

                if (gameRunning == true)
                {
                    if (tecla == Key.Left)
                    {
                        gameState.ChangeDirection(Direction.Left);
                        gameState.CadenaDePasos += "a";
                    }
                    else if (tecla == Key.Right)
                    {
                        gameState.ChangeDirection(Direction.Right);
                        gameState.CadenaDePasos += "d";
                    }
                    else if (tecla == Key.Up)
                    {
                        gameState.ChangeDirection(Direction.Up);
                        gameState.CadenaDePasos += "w";
                    }
                    else if (tecla == Key.Down)
                    {
                        gameState.ChangeDirection(Direction.Down);
                        gameState.CadenaDePasos += "s";
                    }


                }
                GameLoop();
            }
            
        }
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (keyState.ContainsKey(e.Key))
            {
                keyState[e.Key] = false;
            }
        }

        private async void GameLoop()
        {
            //gameState.Move();
            if (!gameState.GameOver)
            {
                gameState.ValidarPos();
                gameState.Move();
                //await Task.Delay(300);
                Draw();
                //gameState.ValidarPos(gameState.KirbyPosiion);

                if (gameState.Ganar == true)
                {
                    await Task.Delay(1000);

                    Al_Ganar?.Invoke();


                }

            }
            else
            {

                gameRunning = false;
                //gameState.Move();
                //Draw();
                await Task.Delay(1000);


            }
        }

        public void MostrarGanar()
        {
            Nivel.AumentarPuntaje();
            PuntajeFinal.Text = $"Puntaje final: {Nivel.PuntajeTotal}";
            if(Nivel.Nivel == 4)
            {
                ScrenSuperganar.Visibility = Visibility.Visible;
            }
            else
            {
                Screnganar.Visibility = Visibility.Visible;
            }
            
            mensaje.Visibility = Visibility.Hidden;
            if (gameState.MinimoMovs)
            {
                mensaje.Visibility = Visibility.Visible;
            }
            Nivel.SiguienteNivel();
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

        public void Draw()
        {
            DrawGrid();



            VidasText.Text = gameState.Vidas.ToString();
			MovimientosText.Text = gameState.Moves.ToString();

			ScoreText.Text = Nivel.PuntajeNivel.ToString();

		}



        private void DrawGrid()
        {

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    GridValue gridVal = gameState.Grid[r, c];
                    if (gameState.Grid[r, c] != GridValue.Tierra && gameState.Grid[r,c] != GridValue.Food && gameState.Grid[r, c] != GridValue.RocaFalsaDestruida && gameState.Grid[r, c] != GridValue.RocaFalsa)
                    {
                        gridImages[r, c].Source = gridValToImag[gridVal];
                        gridImages[r, c].Stretch = Stretch.Fill;
                        
                    }
                    else
                    {
                        gridImages[r, c].Source = null;
                    }
                 }
            }

            Position win = gameState.JuanPosition;
            gridImages[win.Row, win.Col].Source = Images.Estrella;

            DrawFood();
            DrawKirby();
            if(Nivel.Nivel >= 3)
            {
                DrawButton();
                DrawRocasFalsas(false);
            }
            
            
        }
        private async void  DrawRocasFalsas(bool? prmera_vez)
        {
            //if (prmera_vez!= null && prmera_vez == true)
            //{
            //    gridImages[3, 4].Source = Images.RocaFalsa;
            //    gridImages[3, 5].Source = Images.RocaFalsa;
            //    gridImages[3, 6].Source = Images.RocaFalsa;
            //    await Task.Delay(500);
            //    gridImages[3, 4].Source = Images.RocaFalsaSemidestruida;
            //    gridImages[3, 5].Source = Images.RocaFalsaSemidestruida;
            //    gridImages[3, 6].Source = Images.RocaFalsaSemidestruida;
            //    await Task.Delay(500);
                
            //}
           
            foreach (var item in gameState.Pos_PiedrasF)
            {
                
                if (item.Destruida != true)
                {
                    gridImages[item.Posision.Row, item.Posision.Col].Source = Images.RocaFalsa;
                }
                else
                {   
                    
                    gridImages[item.Posision.Row, item.Posision.Col].Source = Images.RocaFalsaDestruida;
                }

            }
        }
        private void DrawFood()
        {
            foreach (var item in gameState.Pos_Comida)
            {
                if(gameState.KirbyPosiion == item.Posision && item.Consumida == false)
                {
                    item.Consumida = true;
                    Nivel.PuntajeNivel += 100;
                }
                if(item.Consumida != true)
                {
                    gridImages[item.Posision.Row, item.Posision.Col].Source = Images.Food;
                }
                
            }
        }
        public void DrawKirby()
        {
            Position kirb = gameState.KirbyPosiion;
            gridImages[kirb.Row, kirb.Col].Source = player;
        }
        public void DrawButton()
        {
            if(Nivel.Nivel == 4)
            {
                foreach (var item in gameState.Pos_Botones)
                {
                    if(item.Presionado == true)
                    {
                        gridImages[item.Posision.Row, item.Posision.Col].Source = gridValToImag[GridValue.BotonPresionado];
                    }
                    else
                    {
                        gridImages[item.Posision.Row, item.Posision.Col].Source = gridValToImag[GridValue.Boton];
                    }
                }
            }
            else
            {
                if(gameState.BotonPresionado == true)
                {
                    gridImages[gameState.BotonPosition.Row, gameState.BotonPosition.Col].Source = gridValToImag[GridValue.BotonPresionado];
                }
                else
                {
                    gridImages[gameState.BotonPosition.Row, gameState.BotonPosition.Col].Source = gridValToImag[GridValue.Boton];
                }

            }
            
        }

        private void Jugar_Click(object sender, RoutedEventArgs e)
        {
            if (btnfacil.IsChecked == true)
            {
                dificultad = "Facil";
            }
            else if (btndificil.IsChecked == true)
            {
                dificultad = "Dificil";
            }
            else
            {
                dificultad = "Normal";
            }



            if(Nivel.Nivel == 4)
            {
                rows = 8;
                cols = 9;
                gameState = new GameState(rows, cols, dificultad, Nivel.Nivel);
            }
            else
            {

                gameState = new GameState(rows, cols, dificultad, Nivel.Nivel);
            }
            
            if(Nivel.Nivel >= 3)
            {
                gameState.Al_presionar_boton += GameState_Al_presionar_boton;
            }
            gameState.Al_Perder += MostrarPerder;
            el_reloj.Tick += El_reloj_Tick;
            el_reloj.Start();
            gridImages = SetUpGrid();

            Draw();
            Screnganar.Visibility = Visibility.Hidden;
            SelectNivel.Visibility = Visibility.Hidden;
            Tutorial1.Visibility = Visibility.Hidden;
            Tutorial2.Visibility = Visibility.Hidden;
            Tutorial3.Visibility = Visibility.Hidden;
            //aqui va el tutorial 3 
            gameRunning = true;

        }

        private void GameState_Al_presionar_boton()
        {
            
            DrawRocasFalsas(true);

        }
        //private void Animar_Piedra()
        //{
        //    gridImages[3, 4].Source = Images.RocaFalsa;
        //    gridImages[3, 5].Source = Images.RocaFalsa;
        //    gridImages[3, 6].Source = Images.RocaFalsa;
        //    Task.Delay(500);
        //    gridImages[3, 4].Source = Images.RocaFalsaSemidestruida;
        //    gridImages[3, 5].Source = Images.RocaFalsaSemidestruida;
        //    gridImages[3, 6].Source = Images.RocaFalsaSemidestruida;
        //    Task.Delay(500);
        //}

        private void btnInicio_Click(object sender, RoutedEventArgs e)
        {
            Overlay.Visibility = Visibility.Hidden;
        }

        private void Regresar_Click(object sender, RoutedEventArgs e)
        {
            Screnganar.Visibility = Visibility.Hidden;
            Screnperder.Visibility = Visibility.Hidden;
            ScrenSuperganar.Visibility = Visibility.Hidden;

            SelectNivel.Visibility = Visibility.Visible;
            Overlay.Visibility = Visibility.Visible;
            Nivel = new ControlNiveles();
            rows = 5;
            cols = 8;
        }

        private void Reintentar_Click(object sender, RoutedEventArgs e)
        {
            gameState = new GameState(rows, cols, dificultad, Nivel.Nivel);
            
            gameState.Al_Perder += MostrarPerder;

            gridImages = SetUpGrid();
            Nivel.PuntajeNivel = 0;
            Draw();
            Screnganar.Visibility = Visibility.Hidden;
            SelectNivel.Visibility = Visibility.Hidden;
            Screnperder.Visibility = Visibility.Hidden;
            gameRunning = true;
        }

       
        //Animacion
        public BitmapImage player { get; set; }      
        int steps = 0;
          
        DispatcherTimer el_reloj = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(50) };     
        private BitmapImage animatePlayer(int start, int end, Direction dir)
        {
            steps++;
            if (steps > end || steps < start)
            {
                steps = start;
            }
            if(dir == Direction.Up)
            {
                player = Images.kirbos_e[steps];
            }
            else if (dir == Direction.Down)
            {
                player = Images.kirbos_f[steps];
            }
            else if (dir == Direction.Left)
            {
                player = Images.kirbos_i[steps];
            }
            else
            {
                player = Images.kirbos[steps];
            }
            
            return player;
        }

		private void Ver_tuto_Click(object sender, RoutedEventArgs e)
		{
            if(Nivel.Nivel == 2)
            {
                Tutorial2.Visibility = Visibility.Visible;
            }
            else if (Nivel.Nivel == 3)
            {
                Tutorial3.Visibility = Visibility.Visible;
            }
            else
            {
                Tutorial1.Visibility = Visibility.Visible;
            }
            
		}

		private void El_reloj_Tick(object? sender, EventArgs e)
        {
            if(gameState.Dir == Direction.Up)
            {
                animatePlayer(0, 3, gameState.Dir);
                el_reloj.Interval = TimeSpan.FromMilliseconds(200);
            }
            else if (gameState.Dir == Direction.Down)
            {
                animatePlayer(0, 9, gameState.Dir);
                el_reloj.Interval = TimeSpan.FromMilliseconds(100);
            }
            else if (gameState.Dir == Direction.Left)
            {
                animatePlayer(0, 9, gameState.Dir);
                el_reloj.Interval = TimeSpan.FromMilliseconds(100);
            }
            else
            {
                animatePlayer(0, 9, gameState.Dir);
                el_reloj.Interval = TimeSpan.FromMilliseconds(100);
            }
            
            DrawKirby();
        }
    }
}
