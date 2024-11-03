using PicSimulator.Simulator.Instruction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSimulator._1DomainLayer
{
    using OPCODE = UInt16;
    public interface IInstructionDecoder
    {
        public IInstruction decode(OPCODE opcode);
    }
}
