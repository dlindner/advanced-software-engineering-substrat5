using PicSimulator._1DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace PicSimulator.Simulator.Instruction
{
    using OPCODE = UInt16;
    using ADDRESS = UInt16;
    public static class InstructionUtilities
    {
        public static bool destinationSetToFileRegister(OPCODE opcode)
        {
            byte flag = (byte)((opcode >> 7) & 1); //only intrested in the 8th bit
            if (flag == 1)
                return true;
            else
                return false;
        }

        public static void handleCarryFlag(int result, IPic16F84 pic)
        {
            if (result > 0xFF)
                pic.Memory.setCFlag(true);
            else
                pic.Memory.setCFlag(false);
        }

        public static void handleCarryFlagSub(int wRegister, int value, IPic16F84 pic)
        {
            if (wRegister > value)
                pic.Memory.setCFlag(true);
            else
                pic.Memory.setCFlag(false);
        }

        public static void handleZeroFlag(byte result, IPic16F84 pic)
        {
            if (result == 0x00)
                pic.Memory.setZFlag(true);
            else
                pic.Memory.setZFlag(false);
        }

        public static void handleDigitCarryFlag(byte wRegister, byte literal, IPic16F84 pic)
        {
            if ((wRegister & 0x0F) + (literal & 0x0F) > 0x0F)
                pic.Memory.setDCFlag(true);
            else
                pic.Memory.setDCFlag(false);
        }

        public static void handleDigitCarryFlagSub(byte wRegister, byte literal, IPic16F84 pic)
        {
            if ((wRegister & 0x0F) > (literal & 0x0F))
                pic.Memory.setDCFlag(true);
            else
                pic.Memory.setDCFlag(false);
        }

        public static ADDRESS getFileAddress(OPCODE opcode)
        {
            return (ADDRESS) (opcode & 0x7F);
        }

        public static ADDRESS getLiteral8(OPCODE opcode)
        {
            return (ADDRESS)(opcode & 0xFF);
        }

        public static ADDRESS getLiteral11(OPCODE opcode)
        {
            return (ADDRESS)(opcode & 0x7FF);
        }

        //returns the selected bit of bit-oriented operations
        public static byte getTargetBit(OPCODE opcode)
        {
            return (byte)((opcode >> 7) & 0b0111);

        }

    }
}
