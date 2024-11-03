using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSimulator._1DomainLayer
{
    public interface IPic16F84
    {
        public byte WDTCounter { get; set; }
        public byte WRegister { get; set; }

        public IProgramCounter ProgramCounter {get;}

        public IMemory Memory { get; }
        public IPicStack Stack { get; }

        public IProgramMemory ProgramMemory { get; }

        public void powerOnReset();

        public void executeNextInstruction();


    }
}
