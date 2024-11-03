using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSimulator._2DomainLayer
{
    public enum StackAction
    {
        push,
        pop
    }

    public class StackChange
    {
        ushort _value;
        StackAction _action;

        public StackAction Action { get => _action; }
        public ushort Value { get => _value; }

        public StackChange(ushort value, StackAction action)
        {
            _value = value;
            _action = action;
        }

    }
}
