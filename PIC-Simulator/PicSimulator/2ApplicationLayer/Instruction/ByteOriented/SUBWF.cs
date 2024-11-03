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
    using ADDRESS =  UInt16;
    using OPCODE = UInt16;
    class SUBWF : IInstruction
    {
        private OPCODE opcode;
        private ADDRESS address;

        public SUBWF(OPCODE opcode)
        {
            this.opcode = opcode;
        }

        public void execute(IPic16F84 pic)
        {
            address = InstructionUtilities.getFileAddress(opcode);
            byte fileRegisterValue = pic.Memory.getByte(address);
            byte firstComplement = (byte)(~pic.WRegister);
            byte secondComplement = (byte)(firstComplement + 1);
            int result = secondComplement + fileRegisterValue;

            //flags
            InstructionUtilities.handleCarryFlagSub(pic.WRegister, fileRegisterValue, pic);
            InstructionUtilities.handleDigitCarryFlagSub(pic.WRegister, fileRegisterValue, pic);
            InstructionUtilities.handleZeroFlag((byte)result, pic);

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
