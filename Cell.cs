using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GameOfLife2
{
    public enum CellStateType
    {
        Alive,
        Unused,
        Dead
    }

    static class CellConstants
    {
        public static readonly Brush AliveButtonFill = Brushes.Black;
        public static readonly Brush UnusedButtonFill = Brushes.Transparent;
        public static readonly Brush DeadButtonFill = Brushes.Gray;
    }
    public class Cell : INotifyPropertyChanged
    {

        private Cell nextCell;
        private Brush buttonFill;
        private CellStateType cellState;
        private CellStateType? prevCellState;

        public CellStateType CellState
        {
            get { return cellState; }
            set
            {
                cellState = value;
                OnPropertyChanged();
            }
        }

        public Brush ButtonFill
        {
            get { return buttonFill; }
            set
            {
                buttonFill = value;
                OnPropertyChanged();
            }
        }

        public Cell()
        {
            ButtonFill = CellConstants.UnusedButtonFill;
            CellState = CellStateType.Unused;
            nextCell = new Cell(true);
        }
        // Constructor for cell representing next cell
        public Cell(bool nextCellConstructor)
        {
            ButtonFill = CellConstants.UnusedButtonFill;
            CellState = CellStateType.Unused;
            prevCellState = CellStateType.Unused;
            nextCell = null;
        }
        private void SetAlive()
        {
            nextCell.CellState = CellStateType.Alive;
            nextCell.ButtonFill = CellConstants.AliveButtonFill;
        }

        private void SetDead()
        {
            nextCell.CellState = CellStateType.Dead;
            nextCell.ButtonFill = CellConstants.DeadButtonFill;
        }

        private void SetUnused()
        {
            nextCell.CellState = CellStateType.Unused;
            nextCell.ButtonFill = CellConstants.UnusedButtonFill;
        }

        public void UpdateCell()
        {
            prevCellState = CellState;
            ButtonFill = nextCell.ButtonFill;
            CellState = nextCell.CellState;
        }
        public void SetState(CellStateType state)
        {
            switch (state)
            {
                case CellStateType.Unused:
                    SetUnused();
                    break;
                case CellStateType.Alive:
                    SetAlive();
                    break;
                case CellStateType.Dead:
                    SetDead();
                    break;
            }
        }

        public void ChangeState()
        {
            if (CellState == CellStateType.Alive)
            {
                SetUnused();
                return;
            }
            SetAlive();

        }

        public void SetPrevAsCurrent()
        {
            if(prevCellState != null)
            {
                SetState(prevCellState ?? default(CellStateType));
                prevCellState = null;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
