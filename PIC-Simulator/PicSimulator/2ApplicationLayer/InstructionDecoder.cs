using System;
using PicSimulator._1DomainLayer;
using PicSimulator.Simulator.Instruction.BitOriented;
using PicSimulator.Simulator.Instruction.ByteOriented;
using PicSimulator.Simulator.Instruction.LiteralAndControl;

namespace PicSimulator.Simulator
{
    using OPCODE = UInt16;

   

    public class InstructionDecoder : IInstructionDecoder
    {

        private string NotImplementedMessage(OPCODE opcode)
        {
            return ("Instruction not Implemented OPCODE: " + opcode);
        }

        public Instruction.IInstruction decode(OPCODE opcode)
        {
            //we can decide the opcode type by the 2 most significant bits
            switch (opcode >> 12)
            {
                case 0b0000:
                    return decodeByteOriented(opcode);
                case 0b0001:
                    return decodeBitOriented(opcode);
                case 0b0010:
                    return decodeGoToOrCall(opcode);
                case 0b0011:
                    return decodeLiteralAndControlOriented(opcode);
                default:
                    throw new NotImplementedException("Opcode not implemented opcode:");
            }
        }

        
        private Instruction.IInstruction decodeByteOriented(OPCODE opcode)
        {
            /*Byte oriented operations look like the following:
             |00 0000|0|0000000|:
              OPCODE  D  FILE

            thus only the 6 most significant bits are relevant for decoding
             */
            //handle edge cases (actually literal and control operations)
            switch (opcode)
            {
                case 0b0000_0000_0110_0100:
                    //CLRWDT
                    return new CLRWDT(opcode);
                case 0b0000_0000_0000_1001:
                    //RETFIE
                    return new RETFIE(opcode);
                case 0b0000_0000_0000_1000:
                    //RETURN
                    return new RETURN(opcode);
                case 0b0000_0000_0000_0011:
                    //SLEEP
                    return new SLEEP(opcode);
                default:
                    break;
            }

            switch ((opcode >> 7) & 0b11111)
            {
                case 0b00011:
                    //CLRF
                    return new CLRF(opcode);
                case 0b00010:
                    //CLRW
                    return new CLRW(opcode);
                case 0b00001:
                    //MOVWF
                    return new MOVWF(opcode);
                case 0b00000:
                    //NOP
                    return new NOP(opcode);
            }

            switch ((opcode >> 8) & 0b1111)
            {
                case 0b0111:
                    //ADDWF
                    return new ADDWF(opcode);
                case 0b0101:
                    //ANDWF
                    return new ANDWF(opcode);
                case 0b1001:
                    //COMF
                    return new COMF(opcode);
                case 0b0011:
                    //DECF
                    return new DECF(opcode);
                case 0b1011:
                    //DECFSZ
                    return new DECFSZ(opcode);
                case 0b1010:
                    //INCF
                    return new INCF(opcode);
                case 0b1111:
                    //INCFSZ
                    return new INCFSZ(opcode);
                case 0b0100:
                    //IORWF
                    return new IORWF(opcode);
                case 0b1000:
                    //MOVF
                    return new MOVF(opcode);
                case 0b1101:
                    //RLF
                    return new RLF(opcode);
                case 0b1100:
                    //RRF
                    return new RRF(opcode);
                case 0b0010:
                    //SUBWF
                    return new SUBWF(opcode);
                case 0b1110:
                    //SWAPF
                    return new SWAPF(opcode);
                case 0b0110:
                    //XORWF
                    return new XORWF(opcode);
                default: 
                    throw new NotImplementedException(NotImplementedMessage(opcode) + " byteoriented");
            }

        }

        private Instruction.IInstruction decodeBitOriented(OPCODE opcode)
        {
            switch ((opcode >> 10) & 0b11)
            {
                case 0b00:
                    //BCF
                    return new BCF(opcode);
                case 0b01:
                    //BSF
                    return new BSF(opcode);
                case 0b10:
                    //BTFSC
                    return new BTFSC(opcode);
                case 0b11:
                    //BTFSS
                    return new BTFSS(opcode);
                default:
                    throw new NotImplementedException(NotImplementedMessage(opcode) + " bitoriented");
            }
        }

        private Instruction.IInstruction decodeLiteralAndControlOriented(OPCODE opcode)
        {
            switch ((opcode >> 10) & 0b11)
            {
                case 0b00:
                    //MOVLW
                    return new MOVLW(opcode);
                case 0b01:
                    //RETLW
                    return new RETLW(opcode);
                default:
                    break;
            }


            switch ((opcode >> 8) & 0b1111)
            {
                case 0b1110:
                case 0b1111:
                    //ADDLW
                    return new ADDLW(opcode);
                case 0b1001:
                    //ANDLW
                    return new ANDLW(opcode);
                case 0b1000:
                    //IORLW
                    return new IORLW(opcode);
                case 0b1100:
                case 0b1101:
                    //SUBLW
                    return new SUBLW(opcode);
                case 0b1010:
                    //XORLW
                    return new XORLW(opcode);
                default:
                    throw new NotImplementedException(NotImplementedMessage(opcode) + " literalAndControlOriented");
            }
        }

        private Instruction.IInstruction decodeGoToOrCall(OPCODE opcode)
        {
            //only intrested in the thirth most significant bits
            switch ((opcode >> 11) & 1)
            {
                case 0:
                //call
                   return new CALL(opcode);
                case 1:
                //goto
                   return new GOTO(opcode);
                default:
                   throw new NotImplementedException(NotImplementedMessage(opcode) + " goto or call");
            }
        }
    }
}
