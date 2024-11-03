using PicSimulator._1DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTests")]

namespace PicSimulator.Simulator.Instruction.BitOriented
{
    using ADDRESS = UInt16;
    using OPCODE = UInt16;
    class BSF : IInstruction
    {
        private OPCODE opcode;

        public BSF(OPCODE opcode)
        {
            this.opcode = opcode;
        }

        public int getCycles()
        {
            return 1;
        }

        public void execute(IPic16F84 pic)
        {
            ADDRESS fileRegister = InstructionUtilities.getFileAddress(opcode);
            byte targetBit = InstructionUtilities.getTargetBit(opcode);
            pic.Memory.setBit(fileRegister, targetBit, true);
        }
    }
}
