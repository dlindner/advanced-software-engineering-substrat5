using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using PicSimulator._1DomainLayer;
using PicSimulator.Simulator;

namespace PicSimulator
{
    class BankLine : INotifyPropertyChanged, IObserver<IMemory>
    {
        int bank;

        UInt16 addr;
        byte data;
        string sfrName;

        string bit0Text;
        string bit1Text;
        string bit2Text;
        string bit3Text;
        string bit4Text;
        string bit5Text;
        string bit6Text;
        string bit7Text;

        Memory memRef;

        public BankLine (UInt16 addr, byte data, Memory memRef, int bank)
        {
            this.bank = bank;
            this.addr = addr;
            this.data = data;
            this.SfrName = "";
            this.memRef = memRef;
            this.memRef.Subscribe(this);
        }

        //wird aktuell von memory aufgerufen um die änderung weiterzugeben
        public void setData(byte data)
        {
            this.data = data;
            OnPropertyChanged(null);
        }
        public string Addr
        {
            get
            {
                return "0x"+addr.ToString("X");
            }
            
        }

        private void updateMemory()
        {
            memRef.setByteForBank((byte)addr, data, bank);
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
                    data = byte.Parse(value, System.Globalization.NumberStyles.HexNumber);
                    OnPropertyChanged(null);
                    updateMemory();
                }
                catch
                {

                }
            }
        }

        public string Binary
        {
            get
            {
                 return Convert.ToString(data, 2).PadLeft(8, '0'); // 00000011;
            }
            set
            {
                try
                {
                    data = Convert.ToByte(value, 2);
                    OnPropertyChanged(null);
                    updateMemory();
                }
                catch
                {

                }
            }
        }

        public bool Bit0
        {
            get => GetBit(0);
            set
            {
                SetBit(0, value);
                OnPropertyChanged(null);
            }
        }
        public bool Bit1
        {
            get => GetBit(1);
            set
            {
                SetBit(1, value);
                OnPropertyChanged(null);
            }
        }

        public bool Bit2
        {
            get => GetBit(2);
            set
            {
                SetBit(2, value);
                OnPropertyChanged(null);
            }
        }

        public bool Bit3
        {
            get => GetBit(3);
            set
            {
                SetBit(3, value);
                OnPropertyChanged(null);
            }
        }

        public bool Bit4
        {
            get => GetBit(4);
            set
            {
                SetBit(4, value);
                OnPropertyChanged(null);
            }
        }

        public bool Bit5
        {
            get => GetBit(5);
            set
            {
                SetBit(5, value);
                OnPropertyChanged(null);
            }
        }

        public bool Bit6
        {
            get => GetBit(6);
            set
            {
                SetBit(6, value);
                OnPropertyChanged(null);
            }
        }

        public bool Bit7
        {
            get => GetBit(7);
            set
            {
                SetBit(7, value);
                OnPropertyChanged(null);
            }
        }

        public bool GetBit(int position) => (data & (1 << position)) != 0;

        public void SetBit(int position, bool newValue)
        {
            if (newValue)
            { 
                data |= (byte)(1 << position);
            }
            else
            { 
                data &= (byte)(~(1 << position));
            }
            updateMemory();
        }

        public string SfrName { get => sfrName; set => sfrName = value; }
        public string Bit0Text { get => bit0Text; set => bit0Text = value; }
        public string Bit1Text { get => bit1Text; set => bit1Text = value; }
        public string Bit2Text { get => bit2Text; set => bit2Text = value; }
        public string Bit3Text { get => bit3Text; set => bit3Text = value; }
        public string Bit4Text { get => bit4Text; set => bit4Text = value; }
        public string Bit5Text { get => bit5Text; set => bit5Text = value; }
        public string Bit6Text { get => bit6Text; set => bit6Text = value; }
        public string Bit7Text { get => bit7Text; set => bit7Text = value; }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(IMemory value)
        {
            //bankupdate aus MainWindow
            byte newValue = value.getByteFromBank((byte)addr, bank);
            setData(newValue); //also calls onPropertyChanged, we don'T want to touch this without knowing the side effects
        }

        #endregion
    }
}
