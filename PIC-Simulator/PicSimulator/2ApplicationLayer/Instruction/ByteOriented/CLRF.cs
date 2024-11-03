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
    using ADDRESS = UInt16;
    class CLRF : IInstruction
    {
        private OPCODE opcode;
        private ADDRESS address;
        public CLRF(OPCODE opcode)
        {
            this.opcode = opcode;
        }
        public void execute(IPic16F84 pic)
        {
            address = InstructionUtilities.getFileAddress(opcode);
            pic.Memory.setByte(address, 0);
            pic.Memory.setZFlag(true);
        }

        public int getCycles()
        {
            return 1;
        }
    }
}
