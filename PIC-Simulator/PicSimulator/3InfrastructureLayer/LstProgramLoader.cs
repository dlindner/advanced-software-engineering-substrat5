using PicSimulator._1DomainLayer;
using PicSimulator._2ApplicationLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSimulator._3InfrastructureLayer
{
    public class LstFileProgramLoader : ILstFileProgramLoader
    {
        IStringProgramLoader stringProgramLoader;
        string filePath;
        
        public LstFileProgramLoader(IStringProgramLoader stringProgramLoader)
        {
            this.stringProgramLoader = stringProgramLoader;
        }

        public string FilePath { 
            set { filePath = value; }
        }

        public IEnumerable<ProgramLine> loadProgram()
        {
            var file = new System.IO.StreamReader(filePath);
            string line;
            List<string> lines = new List<string>();
            while ((line = file.ReadLine()) != null)
            {
                var startsWithWhiteSpace = char.IsWhiteSpace(line, 0);
                //skip comments and label definitions
                if (startsWithWhiteSpace)
                    continue;
                lines.Add(line);

            }
            stringProgramLoader.ProgramLines = lines;
            return stringProgramLoader.loadProgram();
        }
    }
}
