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
        NewBorn,
        Alive,
        Unused,
        Dead
    }

    static class CellConstants
    {
        public static readonly Brush AliveButtonFill = Brushes.Black;
        public static readonly Brush UnusedButtonFill = Brushes.Transparent;
        public static readonly Brush DeadButtonFill = Brushes.Gray;
        public static readonly Brush NewbornButtonFill = Brushes.Navy;
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

        private void SetDead(bool isHighlightingOn)
        {
            nextCell.CellState = CellStateType.Dead;
            nextCell.ButtonFill = isHighlightingOn ? CellConstants.DeadButtonFill : CellConstants.UnusedButtonFill;
        }

        private void SetUnused()
        {
            nextCell.CellState = CellStateType.Unused;
            nextCell.ButtonFill = CellConstants.UnusedButtonFill;
        }

        private void SetNewBorn(bool isHighlightingOn)
        {
            nextCell.CellState = CellStateType.NewBorn;

            nextCell.ButtonFill = isHighlightingOn ? CellConstants.NewbornButtonFill : CellConstants.AliveButtonFill;

        }

        public void UpdateCell()
        {
            prevCellState = CellState;
            ButtonFill = nextCell.ButtonFill;
            CellState = nextCell.CellState;
        }
        public void SetState(CellStateType state, bool isHighlightingOn)
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
                    SetDead(isHighlightingOn);
                    break;
                case CellStateType.NewBorn:
                    SetNewBorn(isHighlightingOn);
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

        public void SetPrevAsCurrent(bool isHighlightingOn)
        {
            if (prevCellState != null)
            {
                SetState(prevCellState ?? default(CellStateType), isHighlightingOn);
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
