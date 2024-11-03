using PicSimulator._1DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSimulator._2ApplicationLayer
{
    public class PCLMediator : IPCLMediator
    {
        private IPCLMediatorParticipant memory;
        private IPCLMediatorParticipant programCounter;
        public IPCLMediatorParticipant Memory { 
            get => memory; 
            set => memory = value; 
        }
        public IPCLMediatorParticipant ProgramCounter {
            get => programCounter;
            set => programCounter = value;
        }

        //TODO: the structure should be abstracted in some sort of higher order function
        public void sendPC8Update(IPCLMediatorParticipant sender, byte pc)
        {
            if (sender == memory)
            {
                if (programCounter == null)
                    return;
                programCounter.notifyPC8Update(pc);
                return;
            }
            memory.notifyPC8Update(pc);
        }

        public void sendPCLathUpdate(IPCLMediatorParticipant sender, byte pclath)
        {
            if (sender == memory)
            {
                if (programCounter == null)
                    return;
                programCounter.notifyPCLathUpdate(pclath);
                return;
            }
            memory.notifyPCLathUpdate(pclath);
        }
    }
}
