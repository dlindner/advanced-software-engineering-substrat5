using PicSimulator._1DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTests")]

namespace PicSimulator.Simulator.Instruction.BitOriented
{
    using OPCODE = UInt16;
    using ADDRESS = UInt16;

    class BCF : IInstruction
    {
        private OPCODE opcode;
        public BCF(OPCODE opcode)
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
            pic.Memory.setBit(fileRegister,targetBit, false);
        }
    }
}
