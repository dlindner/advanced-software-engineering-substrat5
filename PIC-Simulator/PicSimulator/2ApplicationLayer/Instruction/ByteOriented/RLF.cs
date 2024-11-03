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
    class RLF : IInstruction
    {
        private OPCODE opcode;
        private ADDRESS address;
        private int result;
        private bool cflag;
        public RLF(OPCODE opcode)
        {
            this.opcode = opcode;
        }
        public void execute(IPic16F84 pic)
        {
            address = InstructionUtilities.getFileAddress(opcode);
            result = pic.Memory.getByte(address);
            cflag = pic.Memory.getCFlag();

            if ((result & 0x80) != 0)
            {
                pic.Memory.setCFlag(true);
            }
            else
            {
                pic.Memory.setCFlag(false);
            }
            result = (byte)(result << 1);
            
            if (cflag)
            {
                result = result | 0x1;
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
            return 1;
        }
    }
}
