
using PicSimulator._1DomainLayer;
using PicSimulator._2ApplicationLayer;
using PicSimulator._3InfrastructureLayer;
using PicSimulator.Simulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace PicSimulator
{
    public class SimulatorMain
    {

        [System.STAThreadAttribute()]
        public static void Main()
        {

            StackAdapter stackAdapter = new StackAdapter();
            var picStack = new PicStack();
            IPCLMediator pclMediator = new PCLMediator();
            var picMemory = new Memory(pclMediator);
            var picProgramCounter = new ProgramCounter(pclMediator);
            IProgramMemory picProgramMemory = new ProgramMemory();
            IInstructionDecoder picInstructionDecoder = new InstructionDecoder();

            var pic = new Pic16F84(picStack, picMemory, picProgramCounter,
                picProgramMemory, picInstructionDecoder);

            picStack.Subscribe(stackAdapter);
            MainWindow wnd = new MainWindow(pic, picStack, picMemory, picProgramCounter);
            wnd.Show();

            Dispatcher.Run();
        }
    }
}