using PicSimulator._1DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSimulator._2ApplicationLayer
{
    public class StringProgramLoader : IStringProgramLoader
    {
        private List<string> programLines;
        public List<string> ProgramLines
        {
            set { programLines = value; }
        }
        public IEnumerable<ProgramLine> loadProgram()
        {
            List<ProgramLine> lines = new List<ProgramLine>();
            foreach (string line in programLines)
            {
                //address
                UInt16 address = UInt16.Parse(line.Substring(0, 4), System.Globalization.NumberStyles.HexNumber);
                //opcode
                UInt16 opCode = UInt16.Parse(line.Substring(5, 4), System.Globalization.NumberStyles.HexNumber);
                lines.Add(new ProgramLine(address, opCode));
            }
            return lines;
        }
    }
}
