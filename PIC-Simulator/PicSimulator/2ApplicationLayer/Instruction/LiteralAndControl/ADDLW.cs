﻿using PicSimulator._1DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTests")]

namespace PicSimulator.Simulator.Instruction.LiteralAndControl
{
    using OPCODE = UInt16;
    using LITERAL = UInt16;
    class ADDLW : IInstruction
    {

        private OPCODE opcode;
        private LITERAL literal;
        private int result;
        public ADDLW(OPCODE opcode)
        {
            this.opcode = opcode;
        }
        public void execute(IPic16F84 pic)
        {
            literal = InstructionUtilities.getLiteral8(opcode);
            result = pic.WRegister + literal;
            InstructionUtilities.handleCarryFlag(result, pic);
            InstructionUtilities.handleDigitCarryFlag(pic.WRegister, (byte)literal, pic);
            InstructionUtilities.handleZeroFlag((byte)result, pic);

            pic.WRegister = (byte)(result);
        }

        public int getCycles()
        {
            return 1;
        }
    }
}
