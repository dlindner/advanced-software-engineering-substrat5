using PicSimulator._1DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTests")]

namespace PicSimulator.Simulator.Instruction.ByteOriented
{
    using OPCODE = UInt16;
    class CLRW : IInstruction
    {
        private OPCODE opcode;
        public CLRW(OPCODE opcode)
        {
            this.opcode = opcode;
        }
        public void execute(IPic16F84 pic)
        {
            pic.WRegister = 0;
            pic.Memory.setZFlag(true);
        }

        public int getCycles()
        {
            return 1;
        }
    }
}
