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
    class GOTO : IInstruction
    {

        private OPCODE opcode;
        private LITERAL literal;
        public GOTO(OPCODE opcode)
        {
            this.opcode = opcode;
        }
        public void execute(IPic16F84 pic)
        {
            literal = InstructionUtilities.getLiteral11(opcode);
            pic.ProgramCounter.SetPc11(literal);
        }

        public int getCycles()
        {
            return 2;
        }
    }
}
