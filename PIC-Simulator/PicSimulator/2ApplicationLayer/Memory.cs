using PicSimulator._1DomainLayer;
using PicSimulator._2ApplicationLayer;
using System;
using System.Collections.Generic;

namespace PicSimulator.Simulator
{
    using ADDRESS = UInt16;

    public class Memory : IMemory, IPCLMediatorParticipant, IObservable<IMemory>
    {
        byte[] register = new byte[256];

        private IPCLMediator mediator;
        private List<IObserver<IMemory>> observers = new List<IObserver<IMemory>>();


        Dictionary<ADDRESS, Action<byte>> addressAction = new Dictionary<ADDRESS, Action<byte>>();

        private void InitAddressAction()
        {
            Action<byte> action;

            action = (byte value) =>
            {
                noop();
            };
            addressAction[(ADDRESS)RegistersBank0.INDF] = action;
            addressAction[(ADDRESS)RegistersBank1.INDF] = action;

            action = (byte value) =>
            {
                setByteDirectAddress((ADDRESS)RegistersBank0.PCL, value);
                setByteDirectAddress((ADDRESS)RegistersBank1.PCL, value);
                mediator.sendPC8Update(this, value);
            };
            addressAction[(ADDRESS)RegistersBank0.PCL] = action;
            addressAction[(ADDRESS)RegistersBank1.PCL] = action;

            action = (byte value) =>
            {
                setByteDirectAddress((ADDRESS)RegistersBank0.STATUS, value);
                setByteDirectAddress((ADDRESS)RegistersBank1.STATUS, value);
            };
            addressAction[(ADDRESS)RegistersBank0.STATUS] = action;
            addressAction[(ADDRESS)RegistersBank1.STATUS] = action;

            action = (byte value) =>
            {
                setByteDirectAddress((ADDRESS)RegistersBank0.FSR, value);
                setByteDirectAddress((ADDRESS)RegistersBank1.FSR, value);
            };
            addressAction[(ADDRESS)RegistersBank0.FSR] = action;
            addressAction[(ADDRESS)RegistersBank1.FSR] = action;

            action = (byte value) =>
            {
                setByteDirectAddress((ADDRESS)RegistersBank0.PCLATH, value);
                setByteDirectAddress((ADDRESS)RegistersBank1.PCLATH, value);
                mediator.sendPCLathUpdate(this, value);
            };
            addressAction[(ADDRESS)RegistersBank0.PCLATH] = action;
            addressAction[(ADDRESS)RegistersBank1.PCLATH] = action;

            action = (byte value) =>
            {
                setByteDirectAddress((ADDRESS)RegistersBank0.INTCON, value);
                setByteDirectAddress((ADDRESS)RegistersBank1.INTCON, value);
            };
            addressAction[(ADDRESS)RegistersBank0.INTCON] = action;
            addressAction[(ADDRESS)RegistersBank1.INTCON] = action;

            action = (byte value) =>
            {
                if (!getBitDirectAddress((ADDRESS)RegistersBank1.OPTION_REG, 3))
                    setByteDirectAddress((ADDRESS)RegistersBank0.TMR0, value);
            };
            addressAction[(ADDRESS)RegistersBank0.TMR0] = action;
        }

        public Memory(IPCLMediator mediator)
        {
            InitAddressAction();
            this.mediator = mediator;
            this.mediator.Memory = this;
        }
        public void powerOnReset()
        {
            setByteDirectAddress((ADDRESS)RegistersBank0.INDF, 0x0);
            setByteDirectAddress((ADDRESS)RegistersBank0.TMR0, 0x0);
            setByteDirectAddress((ADDRESS)RegistersBank0.PCL, 0x0);
            setByteDirectAddress((ADDRESS)RegistersBank0.STATUS, 0x18);
            setByteDirectAddress((ADDRESS)RegistersBank0.FSR, 0x0);
            setByteDirectAddress((ADDRESS)RegistersBank0.PORTA, 0x0);
            setByteDirectAddress((ADDRESS)RegistersBank0.PORTB, 0x0);
            setByteDirectAddress((ADDRESS)RegistersBank0.UNIMP, 0x0);
            setByteDirectAddress((ADDRESS)RegistersBank0.EEDATA, 0x0);
            setByteDirectAddress((ADDRESS)RegistersBank0.EEADR, 0x0);
            setByteDirectAddress((ADDRESS)RegistersBank0.PCLATH, 0x0);
            setByteDirectAddress((ADDRESS)RegistersBank0.INTCON, 0x0);

            setByteDirectAddress((ADDRESS)RegistersBank1.OPTION_REG, 0xFF);
            setByteDirectAddress((ADDRESS)RegistersBank1.TRISA, 0x1F);
            setByteDirectAddress((ADDRESS)RegistersBank1.TRISB, 0xFF);
            setByteDirectAddress((ADDRESS)RegistersBank1.UNIMP, 0x0);
            setByteDirectAddress((ADDRESS)RegistersBank1.EECON1, 0x0);
            setByteDirectAddress((ADDRESS)RegistersBank1.EECON2, 0x0);
        }

        public void reset()
        {
            setByteDirectAddress((ADDRESS)RegistersBank0.INDF, 0x0);
            setByteDirectAddress((ADDRESS)RegistersBank0.PCL, 0x0);
            setByteDirectAddress((ADDRESS)RegistersBank0.STATUS, getByteDirectAddress((ADDRESS)(RegistersBank0.STATUS) & 0x7));
            setByteDirectAddress((ADDRESS)RegistersBank0.PORTA, getByteDirectAddress((ADDRESS)(RegistersBank0.PORTA) & 0x1F));
            setByteDirectAddress((ADDRESS)RegistersBank0.UNIMP, 0x0);
            setByteDirectAddress((ADDRESS)RegistersBank0.PCLATH, 0x0);
            setByteDirectAddress((ADDRESS)RegistersBank0.INTCON, getByteDirectAddress((ADDRESS)(RegistersBank0.INTCON) & 0x1));

            setByteDirectAddress((ADDRESS)RegistersBank1.OPTION_REG, 0xFF);
            setByteDirectAddress((ADDRESS)RegistersBank1.TRISA, 0x1F);
            setByteDirectAddress((ADDRESS)RegistersBank1.TRISB, 0xFF);
            setByteDirectAddress((ADDRESS)RegistersBank1.UNIMP, 0x00);
            setByteDirectAddress((ADDRESS)RegistersBank1.EECON1, 0x8);
            setByteDirectAddress((ADDRESS)RegistersBank1.EECON2, 0x0);
        }

        public void syncPCL(byte pcl)
        {
            setByteDirectAddress((ADDRESS)RegistersBank0.PCL, pcl);
            setByteDirectAddress((ADDRESS)RegistersBank1.PCL, pcl);
        }

        public void noop()
        {
            // do nothing
            // writing to the indf register indirectly results in a no operation (although STATUS bits may be affected)
        }

        public void setByte8(ADDRESS address8bit, byte value)
        {
            if (addressAction.ContainsKey(address8bit))
            {
                addressAction[address8bit](value);
            }
            else
            {
                setByteDirectAddress(address8bit, value);
            }
        }
        public void setByte(ADDRESS address7bit, byte value)
        {
            ADDRESS address8bit = get8bitAddress(address7bit);

            setByte8(address8bit, value);
        }

        private bool isIndirectAddressing(ADDRESS address)
        {
            if (address == (ADDRESS)RegistersBank0.INDF || address == (ADDRESS)RegistersBank1.INDF)
                return true;
            else
                return false;
        }

        public byte getByte(ADDRESS address7bit)
        {
            ADDRESS address8bit = get8bitAddress(address7bit);
            return register[address8bit];
        }

        public byte getByteDirectAddress(byte address)
        {
            return register[address];
        }

        public byte getByteFromBank(byte address, int bank)
        {
            if (bank == 0)
                return getByteDirectAddress(address);
            else 
                return getByteDirectAddress((byte)(address + 128));
        }

        private void setByteDirectAddress(ADDRESS address, byte value)
        {
            register[address] = value;
            notifyObservers();
        }

        private byte byteCalc(byte startValue, byte bit, bool bitValue)
        {
            byte result = 0;

            byte bitPos = (byte)(bit & 0x7);
            if (bitValue)
                result = (byte)(startValue | (0x1 << bitPos));
            else
                result = (byte)(startValue & ~(0x1 << bitPos));
            return result;
        }

        public void setBit(ADDRESS address, byte bit, bool value)
        {
            address = get8bitAddress(address);
            setBitDirectAddress(address, bit, value);
        }

        public void setBitDirectAddress(ADDRESS address, byte bit, bool value)
        {
            byte result = byteCalc(register[address], bit, value);
            setByte8(address, result);
        }

        public bool getBit(ADDRESS address, byte bit)
        {
            address = get8bitAddress(address);
            return getBitDirectAddress(address, bit);
        }

        public bool getBitDirectAddress(ADDRESS address, byte bit)
        {
            byte bitPos = (byte)(bit & 0x7);

            byte bitVal = (byte)(register[address] & (0x1 << bitPos)); 

            if (bitVal != 0)
                return true;
            else
                return false;
        }

        private ADDRESS get8bitAddress(ADDRESS address7bit)
        {
            //indirect addressing
            if (isIndirectAddressing(address7bit))
                address7bit = register[(ADDRESS)RegistersBank0.FSR];
            // add bank bit to the 7 bit address
            ADDRESS address8bit = (ADDRESS)((address7bit & 0x7F) | ((register[(ADDRESS)RegistersBank0.STATUS] & 0x20) << 2));
            return address8bit;
        }

        public void setCFlag(bool value)
        {
            setBitDirectAddress((ADDRESS)RegistersBank0.STATUS, 0, value);
        }

        public bool getCFlag()
        {
            return getBitDirectAddress((ADDRESS)RegistersBank0.STATUS, 0);
        }

        public void setDCFlag(bool value)
        {
            setBitDirectAddress((ADDRESS)RegistersBank0.STATUS, 1, value);
        }

        public bool getDCFlag()
        {
            return getBitDirectAddress((ADDRESS)RegistersBank0.STATUS, 1);
        }

        public void setZFlag(bool value)
        {
            setBitDirectAddress((ADDRESS)RegistersBank0.STATUS, 2, value);
        }

        public bool getZFlag()
        {
            return getBitDirectAddress((ADDRESS) RegistersBank0.STATUS, 2);
        }

        public void setTOBit(bool value)
        {
            setBitDirectAddress((ADDRESS)RegistersBank0.STATUS, 3, value);
        }
        
        public void setPDBit(bool value)
        {
            setBitDirectAddress((ADDRESS)RegistersBank0.STATUS, 4, value);
        }

        public void setGIEBit(bool value)
        {
            setBitDirectAddress((ADDRESS)RegistersBank0.STATUS, 7, value);
        }

        public int getPrescalerTMR0()
        {
            byte prescalerBits = (byte)(register[(ADDRESS)RegistersBank1.OPTION_REG] & 0x07);
            int result = (int)Math.Pow(2, prescalerBits + 1);
            return result;
        }

        public int getPrescalerWDT()
        {
            byte prescalerBits = (byte)(register[(ADDRESS)RegistersBank1.OPTION_REG] & 0x07);
            return (int)Math.Pow(2, prescalerBits);
        }

        public void setPrescaler(byte prescaler)
        {
            byte optionReg = (byte)(register[(ADDRESS)RegistersBank1.OPTION_REG] & 0xF8);
            prescaler = (byte)(prescaler & 0x07);

            byte result = (byte)(optionReg | prescaler);

            this.setByteDirectAddress((ADDRESS)RegistersBank1.OPTION_REG, result);
        }

        public void notifyPCLathUpdate(byte pclath)
        {
            //the notifcation happens at points were previously ProgrammCounter called 
            //syncPCL thus we don't change any behaviour
            syncPCL(pclath);
        }


        public void notifyPC8Update(byte pc)
        {
            //shouldn't happen and in case it does we'll see this, lol
            throw new NotImplementedException();
        }

        private void notifyObservers()
        {
            foreach (var observer in observers)
                observer.OnNext(this);
        }

        public IDisposable Subscribe(IObserver<IMemory> observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
                // Provide observer with existing data.
                observer.OnNext(this);
            }
            return new MemoryUnsubscriber<IMemory>(observers, observer);
        }

        public void setByteForBank(byte address, byte value, int bank)
        {
            if (bank == 0)
                setByte8(address, value);
            else
                setByte8((byte)(address + 128), value);
        }
    }

    internal class MemoryUnsubscriber<IMemory> : IDisposable
    {
        private List<IObserver<IMemory>> _observers;
        private IObserver<IMemory> _observer;
        internal MemoryUnsubscriber(List<IObserver<IMemory>> observers, IObserver<IMemory> observer)
        {
            this._observers = observers;
            this._observer = observer;
        }
        public void Dispose()
        {
            if (_observers.Contains(_observer))
                _observers.Remove(_observer);
        }
    }

}
