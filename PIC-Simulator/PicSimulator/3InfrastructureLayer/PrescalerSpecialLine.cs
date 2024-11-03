using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PicSimulator._1DomainLayer;
using PicSimulator.Simulator;

namespace PicSimulator._3InfrastructureLayer
{
    class PrescalerSpecialLine : SpecialLine, IObserver<IMemory>
    {
        private Memory _memory;
        public PrescalerSpecialLine(Memory memory) : base("Prescaler")
        {
            _memory = memory;
            _memory.Subscribe(this);

        }

        public override string Binary
        {
            get => Convert.ToString(data, 2).PadLeft(3, '0');
            set { } //lol the original implementation didn't do anything either
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
            data = (UInt32)value.getPrescalerTMR0(); //muss ich nachgucken
            OnPropertyChanged(null);
        }

        protected override void updateModel()
        {
            //the original implementation didn't do anything either
        }
    }
}
