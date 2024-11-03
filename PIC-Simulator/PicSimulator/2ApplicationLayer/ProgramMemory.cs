using PicSimulator._1DomainLayer;
using System;
using System.Collections.Generic;

namespace PicSimulator.Simulator
{
    using ADDRESS = UInt16;
    using OPCODE = UInt16;

    public class ProgramMemory : IProgramMemory
    {
        const UInt16 size = 8 * 1024;
        public UInt16 Size { get => size; }
        protected UInt16[] memory = new UInt16[size];


        public ushort getOpCode(ushort address)
        {
            return memory[address];
        }

        private void loadProgram(IEnumerable<ProgramLine> programLines)
        {
            foreach (var programLine in programLines)
            {
                memory[programLine.Address] = programLine.OpCode;
            }
        }

        public void loadProgram(IProgramLoader programLoader)
        {
            loadProgram(programLoader.loadProgram());
        }
    }
}
