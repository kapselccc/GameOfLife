using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife2
{
    public class Stats : INotifyPropertyChanged
    {

        private int deadNum;
        private int bornNum;
        private int generation;

        public int Generation
        {
            get { return generation; }
            set { generation = value; OnPropertyChanged(); }
        }


        public int BornNum
        {
            get { return bornNum; }
            set { bornNum = value; OnPropertyChanged(); }
        }


        public int DeadNum
        {
            get { return deadNum; }
            set { deadNum = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
