using PicSimulator._1DomainLayer;
using PicSimulator._2DomainLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace PicSimulator.Simulator
{
    using ADDRESS = UInt16;
    //Pic16F84 Stack implemented as Ringbuffer
    public class PicStack : IPicStack, IObservable<StackChange>
    {
        public const ADDRESS BufferLength = 8;
        ADDRESS[] stack = new ADDRESS[BufferLength];
        int stackpointer = 0;

        private List<IObserver<StackChange>> observers = new List<IObserver<StackChange>>();
        //Stack push method
        public void push(ADDRESS value)
        {
            const ADDRESS mask = 0x1FFF;
            stack[stackpointer] = (ADDRESS) (value & mask);
            stackpointer = (stackpointer + 1) % BufferLength;
            notifyObservers(new StackChange(value, StackAction.push));
        }

        //Stack pop method
        public ADDRESS pop()
        {
            stackpointer = (stackpointer + BufferLength - 1) % BufferLength;
            ADDRESS reti = stack[stackpointer];
            notifyObservers(new StackChange(reti, StackAction.pop));
            return reti;

        }

        private void notifyObservers(StackChange value)
        {
            foreach (var observer in observers)
                observer.OnNext(value);
        }
        public override string ToString()
        {
            var reti = new StringBuilder();
            reti.Append("Stackpointer position: " + stackpointer + "\n");
            for (int i = 0; i < BufferLength; i++)
                reti.Append("index: " + i + "value: " + stack[i] + "\n");

            return reti.ToString();
        }

        public IDisposable Subscribe(IObserver<StackChange> observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
            }
            return new StackChangeUnsubscriber<StackChange>(observers, observer);
        }
    }

    internal class StackChangeUnsubscriber<StackChange> : IDisposable
    {
        private List<IObserver<StackChange>> _observers;
        private IObserver<StackChange> _observer;
        internal StackChangeUnsubscriber(List<IObserver<StackChange>> observers, IObserver<StackChange> observer)
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
