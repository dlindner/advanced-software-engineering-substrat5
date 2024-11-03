using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSimulator._1DomainLayer
{
    using ADDRESS = UInt16;
    public interface IPicStack
    {
        public void push(ADDRESS value);
        public ADDRESS pop();
    }
}
