using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSimulator._1DomainLayer
{
    using ADDRESS = UInt16;
    public interface IMemory 
    {
        public void setByte8(ushort address8bit, byte value); //temporarily until we resolved the gui dependencies bankline depends on this
        public void powerOnReset();
        public void reset();

        public void setByte(ADDRESS address7bit, byte value);
        public void setByteForBank(byte address, byte value, int bank);
        public byte getByte(ADDRESS address7bit);
        public byte getByteDirectAddress(byte address);

        public void setBit(ADDRESS address, byte bit, bool value);
        public void setBitDirectAddress(ADDRESS address, byte bit, bool value);
        public bool getBit(ADDRESS address, byte bit);
        public bool getBitDirectAddress(ADDRESS address, byte bit);
        public byte getByteFromBank(byte address, int bank);

        public void setCFlag(bool value);
        public bool getCFlag();
        public void setDCFlag(bool value);
        public bool getDCFlag();

        public void setZFlag(bool value);
        public bool getZFlag();
        public void setTOBit(bool value);
        public void setPDBit(bool value);
        public void setGIEBit(bool value);
        public int getPrescalerTMR0();
        public void setPrescaler(byte prescaler);

    }
}
