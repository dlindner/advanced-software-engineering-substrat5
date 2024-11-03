using PicSimulator._1DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTests")]

namespace PicSimulator.Simulator.Instruction.BitOriented
{
    using OPCODE = UInt16;
    class NOP : IInstruction
    {

        private OPCODE opcode;
        public NOP(OPCODE opcode)
        {
            this.opcode = opcode;
        }
        public void execute(IPic16F84 pic)
        {
            
        }

        public int getCycles()
        {
            return 1;
        }
    }
}
