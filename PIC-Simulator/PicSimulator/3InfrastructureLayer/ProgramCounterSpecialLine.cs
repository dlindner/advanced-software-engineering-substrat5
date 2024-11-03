using PicSimulator.Simulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSimulator._3InfrastructureLayer
{
    class ProgramCounterSpecialLine : SpecialLine, IObserver<ushort>
    {
        ProgramCounter programCounter;
        public override string Binary { 
            get => Convert.ToString(data, 2).PadLeft(13, '0');
            set {
                data = Convert.ToUInt32(value, 2);
                updateModel();
            }
        }
        
        public ProgramCounterSpecialLine(ProgramCounter pc) : base("PC")
        {
            programCounter = pc;
            programCounter.Subscribe(this);
            
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(ushort value)
        {
            data = value;
            OnPropertyChanged(null);
        }

        protected override void updateModel()
        {
            programCounter.setPc13((UInt16) data);
        }
    }
}
