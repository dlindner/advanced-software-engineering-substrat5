using PicSimulator._2DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSimulator._3InfrastructureLayer
{
    class WRegisterSpecialLine : SpecialLine, IObserver<WRegister>
    {
        private WRegister _wRegister;

        public WRegisterSpecialLine(WRegister wRegister) : base("W Register")
        {
            _wRegister = wRegister;
            _wRegister.Subscribe(this);
        }

        public override string Binary
        {
            get => Convert.ToString(data, 2).PadLeft(8, '0'); // 00000011;
            set
            {
                data = Convert.ToUInt32(value, 2);
                updateModel();
            }
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(WRegister value)
        {
            data = value.WRegisterValue;
            OnPropertyChanged(null);
        }

        protected override void updateModel()
        {
            _wRegister.WRegisterValue = (byte)data;
        }
    }
}
