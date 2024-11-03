using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSimulator._1DomainLayer
{
    public interface IProgramLoader
    {
        public IEnumerable<ProgramLine> loadProgram();
    }
}
