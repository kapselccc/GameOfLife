using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace GameOfLife2
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window, INotifyPropertyChanged
    {
        private Game game;

        public Game Game
        {
            get { return game; }
            set { game = value; OnPropertyChanged();}
        }

        public SettingsWindow(Game game)
        {
            InitializeComponent();
            Game = game;
            DataContext = Game;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void EnableHighlightingCheckbox_Click(object sender, RoutedEventArgs e)
        {
            Game.ChangeHighlighting();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(HeightInput.Text, out int height) && 
                int.TryParse(WidthInput.Text, out int width) &&
                int.TryParse(MaxAliveNeighboursNumNotToDieInput. Text,out int maxAliveNeighboursNumNotToDieInput) &&
                int.TryParse(MinAliveNeighboursNumNotToDieInput.Text, out int minAliveNeighboursNumNotToDieInput) &&
                int.TryParse(AliveNeighboursNumToBeBornInput.Text, out int aliveNeighboursNumToBeBornInput))

            {
                if (height != Game.Height || width != Game.Width)
                {
                    Game.ResizeBoard(height, width);
                    Game.InitializeBoard();
                }

                Game.MaxAliveNeighboursNumNotToDie = maxAliveNeighboursNumNotToDieInput;
                Game.MinAliveNeighboursNumNotToDie = minAliveNeighboursNumNotToDieInput;
                Game.AliveNeighboursNumToBeBorn = aliveNeighboursNumToBeBornInput;
                var isHighlightingEnabled = HighlightingCheckbox.IsChecked ?? default(bool);
                if (Game.IsHighlightingEnabled == isHighlightingEnabled)
                {
                    Game.ChangeHighlighting();
                }

                Game.IsHighlightingEnabled = isHighlightingEnabled;
                Close();
            }
            else
            {
                MessageBox.Show("Inputs must be type of integer!");
            }
        }
    }
}
