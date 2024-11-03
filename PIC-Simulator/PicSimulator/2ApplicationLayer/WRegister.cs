using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSimulator._2DomainLayer
{
    public class WRegister : IObservable<WRegister>
    {
        byte _wRegister = 0;

        private List<IObserver<WRegister>> observers = new List<IObserver<WRegister>>();
        public byte WRegisterValue { get =>  _wRegister; 
            set {
                _wRegister = value;
                notifyObservers();
            }
        }

        private void notifyObservers()
        {
            foreach (var observer in observers)
                observer.OnNext(this);
        }

        public IDisposable Subscribe(IObserver<WRegister> observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
                // Provide observer with existing data.
                observer.OnNext(this);
            }
            return new WRegisterUnsubscriber<WRegister>(observers, observer);
        }
    }

    internal class WRegisterUnsubscriber<WRegister> : IDisposable
    {
        private List<IObserver<WRegister>> _observers;
        private IObserver<WRegister> _observer;
        internal WRegisterUnsubscriber(List<IObserver<WRegister>> observers, IObserver<WRegister> observer)
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
