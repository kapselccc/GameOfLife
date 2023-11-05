using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace GameOfLife2
{
    public enum GameStateType
    {
        Running,
        Stop,
        NotStarted
    }
    public class Game : INotifyPropertyChanged
    {
        private int aliveNeighboursNumToBeBorn = 3;
        private int maxAliveNeighboursNumNotToDie = 3;
        private int minAliveNeighboursNumNotToDie = 2;
        private GridLength gridLength = new GridLength(1, GridUnitType.Star);
        private int height;
        private int width;
        private const int delay = 500;
        private GameStateType gameState;
        private Grid gameGrid;
        private CellControl[,] gameBoard;
        private Stats gameStats;
        private Stats? prevStats;
        private bool isHighlightingEnabled;

        public bool IsHighlightingEnabled
        {
            get { return isHighlightingEnabled; }
            set { isHighlightingEnabled = value; OnPropertyChanged();}
        }


        public Stats GameStats
        {
            get { return gameStats; }
            set { gameStats = value; OnPropertyChanged(); }
        }

        public CellControl[,] GameBoard
        {
            get { return gameBoard; }
            set { gameBoard = value; OnPropertyChanged(); }
        }

        public Grid GameGrid
        {
            get { return gameGrid; }
            set { gameGrid = value; OnPropertyChanged(); }
        }

        public GameStateType GameState
        {
            get { return gameState; }
            set
            {
                gameState = value;
                OnPropertyChanged();
            }
        }
        public int Width
        {
            get { return width; }
            set { width = value; OnPropertyChanged(); }
        }

        public int Height
        {
            get { return height; }
            set
            {
                height = value;
                OnPropertyChanged();
            }
        }



        public Game(int height, int width, Grid grid)
        {
            Height = height;
            Width = width;
            GameGrid = grid;
            GameBoard = new CellControl[height, width];
            GameState = GameStateType.NotStarted;
            GameStats = new Stats
            {
                BornNum = 0,
                DeadNum = 0,
                Generation = 0,
            };
            prevStats = null;
            IsHighlightingEnabled = true;
        }

        public void SetPrevState()
        {
          
            if (GameState != GameStateType.NotStarted)
            {
                GameStats.Generation = prevStats.Generation;
                GameStats.BornNum = prevStats.BornNum;
                GameStats.DeadNum = prevStats.DeadNum;
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        GameBoard[i, j].SetPrevAsCurrent(IsHighlightingEnabled);
                    }
                }
                UpdateCells();
            }
        }
        public void ResizeBoard(int height, int width)
        {
            Height = height;
            Width = width;
            GameGrid.RowDefinitions.Clear();
            GameGrid.ColumnDefinitions.Clear();
            GameBoard = new CellControl[height, width];
        }

        public void ResetGameStats()
        {
            GameStats = new Stats
            {
                BornNum = 0,
                DeadNum = 0,
                Generation = 0,
            };
            GameState = GameStateType.NotStarted;
        }

        public void FillBoard()
        {
            GameGrid.Children.Clear();
            for (int vert = 0; vert < Height; vert++)
            {
                for (int hor = 0; hor < Width; hor++)
                {
                    CellControl cell = new CellControl();
                    Grid.SetRow(cell, vert);
                    Grid.SetColumn(cell, hor);
                    GameBoard[vert, hor] = cell;
                    GameGrid.Children.Add(cell);
                }
            }
            OnPropertyChanged(nameof(GameGrid));
            OnPropertyChanged(nameof(GameBoard));
        }
        public void InitializeBoard()
        {

            for (int i = 0; i < Height; i++)
            {
                GameGrid.RowDefinitions.Add(new RowDefinition
                {
                    Height = gridLength
                });

            }

            for (int i = 0; i < Width; i++)
            {
                GameGrid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = gridLength
                });
            }
            FillBoard();
        }

        public void NextGeneration()
        {
            prevStats = prevStats ?? new Stats();
            prevStats.Generation = GameStats.Generation;
            prevStats.BornNum = GameStats.BornNum;
            prevStats.DeadNum = GameStats.DeadNum;

            GameStats.Generation++;
            GameStats.BornNum = 0;
            GameStats.DeadNum = 0;
            for (int vert = 0; vert < Height; vert++)
            {
                for (int hor = 0; hor < Width; hor++)
                {
                    var cellState = GameBoard[vert, hor].CellState;
                    int aliveNeighboursNum = GetAliveNeighboursNumber(hor, vert);

                    if ( cellState == CellStateType.Alive || cellState == CellStateType.NewBorn)
                    {
                        if (aliveNeighboursNum > maxAliveNeighboursNumNotToDie ||
                            aliveNeighboursNum < minAliveNeighboursNumNotToDie)
                        {
                            GameBoard[vert, hor].SetState(CellStateType.Dead, IsHighlightingEnabled);
                            GameStats.DeadNum++;
                        }
                        else if(cellState == CellStateType.NewBorn)
                        {
                            GameBoard[vert, hor].SetState(CellStateType.Alive, IsHighlightingEnabled);
                        }
                    }
                    else
                    {
                        if (aliveNeighboursNum == aliveNeighboursNumToBeBorn)
                        {
                            GameBoard[vert, hor].SetState(CellStateType.NewBorn, IsHighlightingEnabled);
                            GameStats.BornNum++;
                        }
                        else if(cellState == CellStateType.Dead)
                        {
                            GameBoard[vert,hor].SetState(CellStateType.Unused, IsHighlightingEnabled);
                        }
                    }
                }
            }
            UpdateCells();

        }

        public void StartGeneration()
        {
            GameState = GameStateType.Running;
            while (GameState == GameStateType.Running)
            {
                NextGeneration();
                GameGrid.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => { }));
                Thread.Sleep(delay);
            }
        }
        private void UpdateCells()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    GameBoard[i, j].UpdateCell();
                }
            }
        }
        public string[] SaveGameState()
        {
            string[] state = new string[Height];
            for (int i = 0; i < Height; i++)
            {
                string s = "";
                for (int j = 0; j < Width; j++)
                {
                    switch (GameBoard[i, j].CellState)
                    {
                        case CellStateType.Alive:
                            s += "A";
                            break;
                        case CellStateType.Dead:
                            s += "D";
                            break;
                        case CellStateType.Unused:
                            s += "U";
                            break;
                        case CellStateType.NewBorn:
                            s += "B";
                            break;
                    }
                }

                state[i] = s;
            }


            return state;
        }

        public void LoadState(string[] state)
        {

            int height = state.Length;
            int width = state[0].Length;

            if (Height != height || Width != width)
            {
                ResizeBoard(height, width);
            }
            ResetGameStats();
            InitializeBoard();

            for (int vert = 0; vert < Height; vert++)
            {
                for (int hor = 0; hor < Width; hor++)
                {
                    switch (state[vert][hor])
                    {
                        case 'A':
                            GameBoard[vert, hor].SetState(CellStateType.Alive, isHighlightingEnabled);
                            break;
                        case 'D':
                            GameBoard[vert, hor].SetState(CellStateType.Dead, isHighlightingEnabled);
                            break;
                        case 'U':
                            GameBoard[vert, hor].SetState(CellStateType.Unused, isHighlightingEnabled);
                            break;
                        case 'B':
                            GameBoard[vert, hor].SetState(CellStateType.NewBorn, isHighlightingEnabled);
                            break;
                        default:
                            break;
                    }
                }
            }
            UpdateCells();
        }

        private int GetAliveNeighboursNumber(int x, int y)
        {
            int aliveNeighbourNum = 0;
            for (int vert = y - 1; vert <= y + 1; vert++)
            {
                for (int hor = x - 1; hor <= x + 1; hor++)
                {
                    if (vert >= 0 && vert < Height && hor >= 0 && hor < Width)
                    {
                        if (hor == x && vert == y)
                        {
                            continue;
                        }

                        if (GameBoard[vert, hor].CellState == CellStateType.Alive)
                        {
                            aliveNeighbourNum++;
                        }
                    }
                }
            }

            return aliveNeighbourNum;
        }

        public void ChangeHighlighting()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    var cellState = GameBoard[i, j].CellState;
                    GameBoard[i, j].SetState(cellState,IsHighlightingEnabled);
                }
            }
            UpdateCells();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
