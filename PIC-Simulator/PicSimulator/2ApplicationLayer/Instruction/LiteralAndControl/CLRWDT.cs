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
    using ADDRESS = UInt16;
    class CLRWDT : IInstruction
    {

        private OPCODE opcode;
        public CLRWDT(OPCODE opcode)
        {
            this.opcode = opcode;
        }
        public void execute(IPic16F84 pic)
        {

            pic.WDTCounter = 0;
            if (pic.Memory.getBitDirectAddress((ADDRESS)RegistersBank1.OPTION_REG, 3))
            {
                pic.Memory.setPrescaler(0);
            }
            pic.Memory.setPDBit(true);
            pic.Memory.setTOBit(true);
        }

        public int getCycles()
        {
            return 1;
        }
    }
}
