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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameOfLife2
{
    /// <summary>
    /// Interaction logic for CellControl.xaml
    /// </summary>
    public partial class CellControl : UserControl, INotifyPropertyChanged
    {
        private Cell cellModel;

        public Cell CellModel
        {
            get { return cellModel; }
            set { cellModel = value; OnPropertyChanged(); }
        }

        public CellControl()
        {
            InitializeComponent();
            CellModel = new Cell();
            DataContext = CellModel;
        }

        public CellStateType CellState
        {
            get { return CellModel.CellState;}
            set { CellModel.CellState = value; }
        }

        public void SetState(CellStateType state, bool isHighlightingOn)
        {
            CellModel.SetState(state, isHighlightingOn);
        }

        public void UpdateCell()
        {
            CellModel.UpdateCell();
        }

        public void SetPrevAsCurrent(bool isHighlightingOn)
        {
            cellModel.SetPrevAsCurrent(isHighlightingOn);
        }
        private void ControlButton_OnClick(object sender, RoutedEventArgs e)
        {
            CellModel.ChangeState();
            CellModel.UpdateCell();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

     
    }

    
}
