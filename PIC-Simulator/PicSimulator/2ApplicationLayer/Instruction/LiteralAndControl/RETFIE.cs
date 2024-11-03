using PicSimulator._1DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTests")]

namespace PicSimulator.Simulator.Instruction.LiteralAndControl
{
    using OPCODE = UInt16;
    class RETFIE : IInstruction
    {

        private OPCODE opcode;
        public RETFIE(OPCODE opcode)
        {
            this.opcode = opcode;
        }
        public void execute(IPic16F84 pic)
        {
            pic.ProgramCounter.SetPc11(pic.Stack.pop());
            pic.Memory.setGIEBit(true);
        }

        public int getCycles()
        {
            return 2;
        }
    }
}
