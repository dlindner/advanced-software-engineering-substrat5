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
    using ADDRESS = UInt16;
    using OPCODE = UInt16;
    class BTFSC : IInstruction
    {
        private OPCODE opcode;
        private int cycles;

        public BTFSC(OPCODE opcode)
        {
            this.opcode = opcode;
        }

        public void execute(IPic16F84 pic)
        {
            ADDRESS fileRegister = InstructionUtilities.getFileAddress(opcode);
            byte targetBit = InstructionUtilities.getTargetBit(opcode);
            if (pic.Memory.getBit(fileRegister, targetBit))
            {
                cycles = 1;
            }
            else
            {
                pic.ProgramCounter.Increment();
                cycles = 2;
            }
        }

        public int getCycles()
        {
            return cycles;
        }
    }
}
