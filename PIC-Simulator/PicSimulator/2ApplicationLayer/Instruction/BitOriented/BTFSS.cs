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

    class BTFSS : IInstruction
    {
        private OPCODE opcode;
        private int cycles;

        public BTFSS(OPCODE opcode)
        {
            this.opcode = opcode;
        }

        public int getCycles()
        {
            return cycles;
        }

        public void execute(IPic16F84 pic)
        {
            ADDRESS fileRegister = InstructionUtilities.getFileAddress(opcode);
            byte targetBit = InstructionUtilities.getTargetBit(opcode);
            bool bitIsSet = pic.Memory.getBit(fileRegister, targetBit);

            if (bitIsSet)
            {
                //skip the instruction by incrementing the programmcounter
                pic.ProgramCounter.Increment();
                cycles = 2;
            }
            else
            {
                cycles = 1;
            }
        }
    }
}
