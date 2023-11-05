using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace GameOfLife2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private Game game;
        private int height;
        private int width;
        public Game Game
        {
            get { return game; }
            set { game = value; OnPropertyChanged(); }
        }
        public int Width
        {
            get { return width; }
            set { width = value; OnPropertyChanged(); }
        }
        public int Height
        {
            get { return height; }
            set { height = value; OnPropertyChanged(); }
        }

        public MainWindow()
        {
            InitializeComponent();
            Height = 20;
            Width = 30;
            Game = new Game(Height, Width, BoardGrid);
            DataContext = Game;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void InitGame(object sender, RoutedEventArgs e)
        {
            Game.InitializeBoard();
        }
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            Game.NextGeneration();
            Game.GameState = GameStateType.Stop;
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            Game.ResetGameStats();
            Game.FillBoard();
        }


        private void SaveToFile_Click(object sender, RoutedEventArgs e)
        {
            string[] state = Game.SaveGameState();
            FileManager.SaveStateToFile(state);
        }

        private void LoadFromFile_Click(object sender, RoutedEventArgs e)
        {
            string[] state = FileManager.LoadStateFromFile();

            if (state != Array.Empty<string>())
            {
                Game.LoadState(state);
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Game.StartGeneration();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            Game.GameState = GameStateType.Stop;
        }

        private void SetSizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(HeightInput.Text, out int height) && int.TryParse(WidthInput.Text, out int width))
            {
                Game.ResizeBoard(height, width);
                Game.InitializeBoard();
            }
            else
            {
                MessageBox.Show("Height and Width must be integer!");
            }
        }

        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            Game.SetPrevState();
        }

        private void EnableHighlightingCheckbox_Click(object sender, RoutedEventArgs e)
        {
            Game.ChangeHighlighting();
        }
    }
}
