using PicSimulator._1DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTests")]

namespace PicSimulator.Simulator.Instruction.ByteOriented
{
    using OPCODE = UInt16; 
    using ADDRESS = UInt16;
    class MOVWF : IInstruction
    {
        private OPCODE opcode;
         public MOVWF(OPCODE opcode)
        {
            this.opcode = opcode;
        }



        public int getCycles()
        {
            return 1;
        }

        public void execute(IPic16F84 pic)
        {
            //Move data from w register to register
            ADDRESS register = (ADDRESS) (opcode & 0b0111_1111);
            pic.Memory.setByte(register, pic.WRegister);
        }
    }
}
