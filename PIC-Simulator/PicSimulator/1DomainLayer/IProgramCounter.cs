using PicSimulator._1DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSimulator
{
    using ADDRESS = UInt16;
    public interface IProgramCounter 
    {
        public void setPc13(UInt16 pc);
        public void Reset();
        public void SetPc8(byte lowByte);
        public void SetPc11(ADDRESS address);

        public void SetPcLath(byte pcLath);

        public void Increment();

        public byte PCL { get; }
        
        public ADDRESS ProgramCounterValue { get; }
    }
}
