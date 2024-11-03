using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSimulator._2ApplicationLayer
{
    public interface IPCLMediatorParticipant
    {
        public void notifyPCLathUpdate(byte pclath);
        public void notifyPC8Update(byte pc);
    }
}
