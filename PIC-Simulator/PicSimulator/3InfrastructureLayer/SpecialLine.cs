using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PicSimulator._3InfrastructureLayer
{
    abstract class SpecialLine : INotifyPropertyChanged
    {
        protected UInt32 data;
        protected string name;

        public event PropertyChangedEventHandler PropertyChanged;

        public SpecialLine(string _name)
        {
            name = _name;
        }

        public void setData(UInt32 data)
        {
            this.data = data;
        }


        public string Data
        {
            get
            {
                return data.ToString("X");
            }
            set
            {
                try
                {
                    data = UInt32.Parse(value, System.Globalization.NumberStyles.HexNumber);
                    OnPropertyChanged(null);
                    updateModel();
                }
                catch
                {

                }
            }
        }

        public abstract string Binary { get; set; }

        public string Name { get => name; set => name = value; }

        protected abstract void updateModel(); //the old updateMemory()

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
