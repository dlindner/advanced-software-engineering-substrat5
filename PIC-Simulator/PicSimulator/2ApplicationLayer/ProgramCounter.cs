using PicSimulator._2ApplicationLayer;
using System;
using System.Collections.Generic;

namespace PicSimulator.Simulator
{
    using ADDRESS = UInt16;
    public class ProgramCounter : IProgramCounter, IPCLMediatorParticipant, IObservable<ADDRESS>
    {
        //sync pcl should be done via events
        //programmcoutner should implement an interface on which Memory depends
        private List<IObserver<ADDRESS>> observers;

        private ADDRESS pc;
        private byte pcLath;

        private IPCLMediator mediator;

        public ProgramCounter(IPCLMediator mediator)
        {
            observers = new List<IObserver<ADDRESS>>(); 
            pc = (ADDRESS)0;
            pcLath = (byte)0;
            this.mediator = mediator;
            this.mediator.ProgramCounter = this;
        }
       
        public void setPc13(UInt16 pc)
        {
            pc_synched = pc;
        }


        public void Reset(){
            pc_synched = 0x00;
        }
        public void SetPc8(byte lowByte)
        {
            pc = 0;
            pc = (ADDRESS)(pc | (ADDRESS)lowByte);
            pc_synched = (ADDRESS)(pc | (((ADDRESS)pcLath) << 8));
        }

        public void SetPc11(ADDRESS address)
        {
            pc = 0;
            // cut off after 11 bits
            pc = (ADDRESS)(pc | (address & 0x7FF));
            pc_synched = (ADDRESS)(pc | ((((ADDRESS)pcLath) & 0x18) << 8));
        }

        public void SetPcLath(byte pcLath)
        {
            this.pcLath = pcLath;
            // cut off upper bits
            this.pcLath = (byte)(pcLath & 0x1F);
            this.mediator.sendPCLathUpdate(this, this.pcLath);
        }

        public void Increment()
        {
            pc++;
  
            if ( (pc & (ADDRESS)0xE000 ) != 0 )
            {
                pc = 0;
            }

            pc_synched = pc;
        }

        private ADDRESS pc_synched
        {
            get
            {
                return pc;
            }
            set
            {
                pc = value;
                mediator.sendPCLathUpdate(this, PCL);
                notifyObservers();
            }
        }
        
        private void notifyObservers()
        {
            foreach (var observer in observers)
                observer.OnNext(ProgramCounterValue);
        }

        public ADDRESS ProgramCounterValue => pc;

        public byte PCL
        {
            get
            {
                byte pcl = (byte)(pc & 0xFF);
                return pcl;
            }
        }

        public void notifyPCLathUpdate(byte pclath)
        {
            SetPcLath(pclath);
        }


        public void notifyPC8Update(byte pc)
        {
            SetPc8(pc);
        }



        public IDisposable Subscribe(IObserver<ADDRESS> observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
                // Provide observer with existing data.
                observer.OnNext(ProgramCounterValue);
            }
            return new ProgramCounterUnsubscriber<ADDRESS>(observers, observer);
        }
    }

    internal class ProgramCounterUnsubscriber<ADDRESS> : IDisposable
    {
        private List<IObserver<ADDRESS>> _observers;
        private IObserver<ADDRESS> _observer;
        internal ProgramCounterUnsubscriber(List<IObserver<ADDRESS>> observers, IObserver<ADDRESS> observer)
        {
            this._observers = observers;
            this._observer = observer;
        }
        public void Dispose()
        {
            if (_observers.Contains(_observer))
                _observers.Remove(_observer);
        }
    }
}


