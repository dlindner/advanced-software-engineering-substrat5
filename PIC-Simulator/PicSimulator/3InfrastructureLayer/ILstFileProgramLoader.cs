using PicSimulator._1DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSimulator._3InfrastructureLayer
{
    public interface ILstFileProgramLoader : IProgramLoader
    {
        public string FilePath { set; }
    }
}
