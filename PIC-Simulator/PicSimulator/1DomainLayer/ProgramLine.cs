using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSimulator._1DomainLayer
{
    using ADDRESS = UInt16;
    using OPCODE = UInt16;
    public class ProgramLine
    {
        public ProgramLine(ADDRESS address, OPCODE opCode)
        {
            Address = address;
            OpCode = opCode;
        }

        public ADDRESS Address { get; }
        public OPCODE OpCode { get; }
    }
}
