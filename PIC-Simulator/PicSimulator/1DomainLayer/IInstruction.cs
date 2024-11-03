using PicSimulator._1DomainLayer;
using System;
using System.Reflection.Emit;

namespace PicSimulator.Simulator.Instruction
{
    using OPCODE = UInt16;

    public interface IInstruction
    {
        int getCycles();
        void execute(IPic16F84 pic);
    }
}