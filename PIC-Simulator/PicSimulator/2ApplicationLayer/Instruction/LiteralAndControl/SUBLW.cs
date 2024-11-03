using PicSimulator._1DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTests")]

namespace PicSimulator.Simulator.Instruction.LiteralAndControl
{
    using OPCODE =  UInt16;
    class SUBLW : IInstruction
    {
        private OPCODE opcode;
        public SUBLW(OPCODE opcode)
        {
            this.opcode = opcode;
        }

        public int getCycles()
        {
            return 1;
        }

        public void execute(IPic16F84 pic)
        {
            byte literal = (byte)InstructionUtilities.getLiteral8(opcode);
            byte firstComplement = (byte)(~pic.WRegister);
            byte secondComplement = (byte) (firstComplement + 1);
            int result = secondComplement + literal;

            //flags
            InstructionUtilities.handleCarryFlagSub(pic.WRegister, literal, pic);
            InstructionUtilities.handleDigitCarryFlagSub(pic.WRegister, literal, pic);
            InstructionUtilities.handleZeroFlag((byte)result, pic);

            pic.WRegister = (byte)result;
        }
    }
}
