using PicSimulator._1DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSimulator._2ApplicationLayer
{
    public interface IStringProgramLoader : IProgramLoader
    {
        public List<string> ProgramLines { set; }
    }
}
