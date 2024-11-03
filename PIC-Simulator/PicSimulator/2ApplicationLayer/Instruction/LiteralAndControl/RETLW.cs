using PicSimulator._1DomainLayer;
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
    class RETLW : IInstruction
    {

        private OPCODE opcode;
        private LITERAL literal;
        public RETLW(OPCODE opcode)
        {
            this.opcode = opcode;
        }
        public void execute(IPic16F84 pic)
        {
            literal = InstructionUtilities.getLiteral8(opcode);
            pic.WRegister = (byte)literal;
            pic.ProgramCounter.SetPc11(pic.Stack.pop());
        }

        public int getCycles()
        {
            return 2;
        }
    }
}
