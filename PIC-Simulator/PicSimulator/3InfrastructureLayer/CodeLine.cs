using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PicSimulator
{
    public class CodeLine : INotifyPropertyChanged
    {
        private string line;

        UInt16 address;

        private bool bp;
        private bool realCode;
        private int active;

        public CodeLine(string line)
        {
            this.line = line;
            bp = false;
        }
        public string Line { get => line; set => line = value; }
        public bool Bp { 
            get => bp;
            set
            {
                bp = value;
                OnPropertyChanged("Bp");
                OnPropertyChanged("ActiveStr");
            }
        }
        public bool RealCode { get => realCode; set => realCode = value; }
        public ushort Address { get => address; set => address = value; }
        public int Active { 
            get => active;
            set 
            { 
                active = value;
                OnPropertyChanged("Bp");
                OnPropertyChanged("ActiveStr");
            }
        }

        public string ActiveStr
        {
            get
            {
                if (active == 1) return "--->";
                else if (active == 2) return "--|";
                else return " ";
            }
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
