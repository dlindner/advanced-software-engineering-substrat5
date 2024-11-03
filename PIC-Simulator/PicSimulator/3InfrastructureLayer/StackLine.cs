using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PicSimulator
{
    class StackLine 
    {
        UInt16 addr;

        public StackLine(UInt16 addr)
        {
            this.addr = addr;
        }

        public string AddrStr
        {
            get => ToString();
        }
        public override string ToString()
        {
                return "0x" + addr.ToString("X");
        }

    }
}
 