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
    class INCFSZ : IInstruction
    {
        private OPCODE opcode;
        private ADDRESS address;
        private int result;
        private int cycles;

        public INCFSZ(OPCODE opcode)
        {
            this.opcode = opcode;
        }
        public void execute(IPic16F84 pic)
        {
            address = InstructionUtilities.getFileAddress(opcode);
            result = pic.Memory.getByte(address) + 1;
            if (result > 0xFF)
            {
                pic.ProgramCounter.Increment();
                cycles = 2;
            }
            else
            {
                cycles = 1;
            }

            if (InstructionUtilities.destinationSetToFileRegister(opcode))
            {
                pic.Memory.setByte(address, (byte)result);
            }
            else
            {
                pic.WRegister = (byte)result;
            }

        }

        public int getCycles()
        {
            return cycles;
        }
    }
}
