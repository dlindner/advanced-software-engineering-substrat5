using System;
using System.Collections.Generic;
using PicSimulator._1DomainLayer;
using PicSimulator._2ApplicationLayer;
using PicSimulator._2DomainLayer;
using PicSimulator.Simulator.Instruction.LiteralAndControl;

namespace PicSimulator.Simulator
{

    using ADDRESS = UInt16;
    public enum RegistersBank0 : UInt16
    {
        INDF = 0x00,
        TMR0 = 0x01,
        PCL = 0x02,
        STATUS = 0x03,
        FSR = 0x04,
        PORTA = 0x05,
        PORTB = 0x06,
        UNIMP = 0x07,
        EEDATA = 0x08,
        EEADR = 0x09,
        PCLATH = 0x0A,
        INTCON = 0x0B
    }

    public enum RegistersBank1 : UInt16
    {
        INDF = 0x80,
        OPTION_REG = 0x81,
        PCL = 0x82,
        STATUS = 0x83,
        FSR = 0x84,
        TRISA = 0x85,
        TRISB = 0x86,
        UNIMP = 0x87,
        EECON1 = 0x88,
        EECON2 = 0x89,
        PCLATH = 0x8A,
        INTCON = 0x8B

    }


    public class Pic16F84 : IPic16F84
    {
        IInstructionDecoder instructionDecoder;
        IProgramMemory programMemory; 
        IMemory ram;
        IPicStack stack;
        IProgramCounter programCounter;

        private WRegister _wRegister;
        private byte wdtCounter = 0;

        UInt32 totalCycles = 0;

        public Pic16F84(IPicStack picStack, IMemory picMemory,
            IProgramCounter picProgramCounter,
            IProgramMemory picProgramMemory,
            IInstructionDecoder picInstructionDecoder)
        {
            _wRegister = new WRegister();
            stack = picStack;
            programMemory = picProgramMemory;
            ram = picMemory;
            programCounter = picProgramCounter;
            instructionDecoder = picInstructionDecoder;
            powerOnReset();
        }

        public byte WRegister
        {
            get { return _wRegister.WRegisterValue; }
            set { 
                _wRegister.WRegisterValue = value;
            }
        }

        public WRegister WRegisterProperty
        {
            get { return _wRegister; }
        }

        public byte WDTCounter
        {
            get { return wdtCounter; }
            set { wdtCounter = value; }
        }

        public IProgramCounter ProgramCounter { get => programCounter; }

        public uint TotalCycles { get => totalCycles;
            set { 
                totalCycles = value;
            } 
        }

        public IMemory Memory
        {
            get => ram;
        }

        public IPicStack Stack
        {
            get => stack;
        }

        public IProgramMemory ProgramMemory
        {
            get { return programMemory; }
        }


        public void powerOnReset()
        {
            TotalCycles = 0;
            WRegister = 0;
            Memory.powerOnReset();
            ProgramCounter.Reset();
        }

        private bool shouldIncrementProgramCounter(Instruction.IInstruction nextInstruction)
        {
            if ((nextInstruction is GOTO) || (nextInstruction is CALL) ||
                (nextInstruction is RETFIE) || (nextInstruction is RETLW) ||
                (nextInstruction is RETURN))
                return false;
            else
                return true;
        }

        public void executeNextInstruction()
        {
            UInt16 nextInstAddr = programCounter.ProgramCounterValue;
            var nextInstructionOpCode = programMemory.getOpCode(nextInstAddr);

            var nextInstruction = instructionDecoder.decode(nextInstructionOpCode);

            nextInstruction.execute(this);
            InterruptHandler.handleInterrupts(this, nextInstruction.getCycles());

            TotalCycles += (uint)nextInstruction.getCycles();

            if (shouldIncrementProgramCounter(nextInstruction))
                programCounter.Increment();

        }

    }


}
