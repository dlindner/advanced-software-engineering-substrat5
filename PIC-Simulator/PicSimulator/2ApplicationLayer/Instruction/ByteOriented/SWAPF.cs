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
    class SWAPF : IInstruction
    {
        private OPCODE opcode;
        private ADDRESS address;
        private int result;
        public SWAPF(OPCODE opcode)
        {
            this.opcode = opcode;
        }
        public void execute(IPic16F84 pic)
        {
            address = InstructionUtilities.getFileAddress(opcode);
            result = (byte)(pic.Memory.getByte(address) << 4) + (byte)(pic.Memory.getByte(address) >> 4);

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
            return 1;
        }
    }
}
