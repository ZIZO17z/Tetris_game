using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DummiesV2
{
    public partial class MainWindow : Window
    {
        private const int TileSize = 30;

        private readonly SolidColorBrush[] blockBrushes = new SolidColorBrush[]
        {
            Brushes.Transparent,
            Brushes.Cyan,
            Brushes.Blue,
            Brushes.Orange,
            Brushes.Yellow,
            Brushes.Green,
            Brushes.Purple,
            Brushes.Red
        };

        private GameState gameState = null!;
        private DispatcherTimer gameTimer = null!;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StartGame();
        }

        private void StartGame()
        {
            gameState = new GameState();
            ScoreText.Text = gameState.Score.ToString();

            gameTimer = new DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromMilliseconds(500);
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();
        }

        private void GameTimer_Tick(object? sender, EventArgs e)
        {
            if (gameState.GameOver)
            {
                gameTimer.Stop();
                MessageBox.Show("Game Over!");
                return;
            }

            gameState.MoveBlockDown();
            Draw();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameState.GameOver)
            {
                return;
            }

            switch (e.Key)
            {
                case Key.Left:
                    gameState.MoveBlockLeft();
                    break;
                case Key.Right:
                    gameState.MoveBlockRight();
                    break;
                case Key.Down:
                    gameState.MoveBlockDown();
                    break;
                case Key.Up:
                    gameState.RotateBlockCW();
                    break;
                case Key.Z:
                    gameState.RotateBlockCCW();
                    break;
            }

            Draw();
        }

        private void Draw()
        {
            GameCanvas.Children.Clear();

            DrawGrid();
            DrawBlock(gameState.CurrentBlock);
            ScoreText.Text = gameState.Score.ToString();
        }

        private void DrawGrid()
        {
            for (int r = 2; r < gameState.GameGrid.Rows; r++)
            {
                for (int c = 0; c < gameState.GameGrid.Columns; c++)
                {
                    int id = gameState.GameGrid[r, c];
                    if (id > 0)
                    {
                        DrawTile(id, r - 2, c);
                    }
                }
            }
        }

        private void DrawBlock(Block block)
        {
            foreach (Position p in block.TilePositions())
            {
                if (p.Row >= 2)
                {
                    DrawTile(block.Id, p.Row - 2, p.Column);
                }
            }
        }

        private void DrawTile(int id, int r, int c)
        {
            Rectangle rect = new Rectangle
            {
                Width = TileSize,
                Height = TileSize,
                Fill = blockBrushes[id],
                Stroke = Brushes.White,
                StrokeThickness = 1
            };

            Canvas.SetTop(rect, r * TileSize);
            Canvas.SetLeft(rect, c * TileSize);
            GameCanvas.Children.Add(rect);
        }
    }
}
