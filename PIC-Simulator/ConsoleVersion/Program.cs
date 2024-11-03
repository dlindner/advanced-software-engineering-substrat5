using PicSimulator.Simulator;
using System;

using PicSimulator._3InfrastructureLayer;
using PicSimulator._2ApplicationLayer;
using PicSimulator._1DomainLayer;
using PicSimulator;

namespace ConsoleVersion
{
    class Program
    {
        static void Main(string[] args)
        {
            Pic16F84 pic;
            IPicStack picStack = new PicStack();
            IPCLMediator pclMediator = new PCLMediator();
            IMemory picMemory = new Memory(pclMediator);
            IProgramCounter picProgramCounter = new ProgramCounter(pclMediator);
            IProgramMemory picProgramMemory = new ProgramMemory();
            IInstructionDecoder picInstructionDecoder = new InstructionDecoder();

            pic = new Pic16F84(picStack, picMemory, picProgramCounter,
                picProgramMemory, picInstructionDecoder);

            var stringProgramLoader = new StringProgramLoader();
            var programLoader = new LstFileProgramLoader(stringProgramLoader);
            programLoader.FilePath = @"TestPrograms\TPicSim1.LST";
            
            pic.ProgramMemory.loadProgram(programLoader);

            var memory = pic.ProgramMemory;
            for (ushort i = 0; i < 1024 * 8; i++)
            {
                if (memory.getOpCode(i) == 0)
                    continue;
                Console.WriteLine("Address: " + i + "\tOPCODE: " + memory.getOpCode(i));
            }
            while(true)
            {
                pic.executeNextInstruction();
            }
        }
    }
}


/*
  static void Main()
        {
            Pic16F84 pic = new Pic16F84();
            pic.powerOnReset();
            pic.loadProgramToMemory("test" /*PATH*///);
            /*while (true)
            {
                if (true /*startButtonClicked*///)
              /*  {
                    pic.reset();
                    while (true /*NotStopButtonClicked*///)
                   /* {
                        pic.executeNextInstruction(pic);
                    }
                }
            }
            
            
        }
*/
