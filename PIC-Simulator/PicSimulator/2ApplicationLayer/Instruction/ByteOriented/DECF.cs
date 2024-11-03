using PicSimulator._1DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTests")]

namespace PicSimulator.Simulator.Instruction.ByteOriented
{
    using OPCODE = UInt16;
    using ADDRESS = UInt16;
    class DECF : IInstruction
    {
        private OPCODE opcode;

        public DECF(OPCODE opcode)
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
            byte initialValue = pic.Memory.getByte(fileRegister);
            byte result = (byte)(initialValue - 1);
            bool storeInFileRegister = InstructionUtilities.destinationSetToFileRegister(opcode);

            InstructionUtilities.handleZeroFlag((byte)result, pic);
            if (storeInFileRegister)
                pic.Memory.setByte(fileRegister, result);
            else
                pic.WRegister = result;
        }
    }
}
