using PicSimulator._2DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSimulator._3InfrastructureLayer
{
    class StackAdapter : IObserver<StackChange>
    {
        List<StackLine> stackLines = new List<StackLine>();

        public List<StackLine> StackLines { get => stackLines; }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(StackChange value)
        {
            if (value.Action == StackAction.push)
            {
                if (stackLines.Count >= 8)
                {
                    stackLines.RemoveAt(stackLines.Count - 1);
                }
                stackLines.Insert(0, new StackLine(value.Value));
            }
            else
            {
                if (stackLines.Count > 0)
                {
                    stackLines.RemoveAt(0);
                }
            }
        }
    }
}
