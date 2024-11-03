using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSimulator._1DomainLayer
{
    using ADDRESS = UInt16;
    using OPCODE = UInt16;
    public interface IProgramMemory
    {
        public UInt16 Size { get; }
        public OPCODE getOpCode(ADDRESS address);
        public void loadProgram(IProgramLoader programLoader);
    }
}
