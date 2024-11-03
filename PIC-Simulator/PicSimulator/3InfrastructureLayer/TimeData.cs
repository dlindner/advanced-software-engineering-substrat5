using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PicSimulator
{
    class TimeData : INotifyPropertyChanged
    {
        uint freq;
        uint cycles;
        public TimeData(uint freq)
        {
            cycles = 0;
            this.freq = 4;
        }
        public uint Freq { 
            get
            {
                return freq;
            }
            set
            {
                freq = value;
                OnPropertyChanged(null);
            }
        }

        public string FreqStr
        {
            get
            {
                return Freq.ToString() + " MHz";
            }
        }

        public string CyclesStr
        {
            get
            {
                return Cycles.ToString();
            }
        }

        public uint Cycles { get => cycles; set
            {
                cycles = value;
                OnPropertyChanged(null);
            }
        }

        public string TimeStr
        {
            get
            {
                
                return getTime().ToString() + " us";
            }
        }

        public uint getTime()
        {
            uint time = (Cycles * 4) / Freq;
            return time;
        }



        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
