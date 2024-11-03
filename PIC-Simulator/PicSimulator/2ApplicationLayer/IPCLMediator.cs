using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSimulator._2ApplicationLayer
{
    public interface IPCLMediator
    {
        IPCLMediatorParticipant Memory { get; set; }
        IPCLMediatorParticipant ProgramCounter { get; set; }


        public void sendPCLathUpdate(IPCLMediatorParticipant sender , byte pclath);
        public void sendPC8Update(IPCLMediatorParticipant sender, byte pc);
    }
}
