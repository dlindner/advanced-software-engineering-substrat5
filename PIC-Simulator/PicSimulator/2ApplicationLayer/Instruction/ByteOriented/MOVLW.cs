using PicSimulator._1DomainLayer;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTests")]

namespace PicSimulator.Simulator.Instruction.ByteOriented
{
    using OPCODE = UInt16;

    class MOVLW : IInstruction
    {
        private OPCODE opcode;

        public MOVLW(OPCODE opcode)
        {
            this.opcode = opcode;
        }

        public int getCycles()
        {
            return 1;
        }

        public void execute(IPic16F84 pic)
        {
            byte literal = (byte)(opcode & 0xFF);
            pic.WRegister = literal;
        }
    }
}
