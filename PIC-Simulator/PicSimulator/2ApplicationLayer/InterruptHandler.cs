using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSimulator.Simulator
{
    using ADDRESS = UInt16;

    public static class InterruptHandler
    {
        //shouldn't have to be static

        private static bool ra4_old = false;
        private static bool rb0_old = false;
        private static byte rb4to7_old = 0;
        private static int countedCycles = 0;

        private static void executeInterrupt(Pic16F84 pic)
        {
            pic.Memory.setBitDirectAddress((ADDRESS)RegistersBank0.INTCON, 7, false);
            pic.Stack.push((ADDRESS)(pic.ProgramCounter.PCL + 1));
            pic.ProgramCounter.SetPc11(0x04);
        }

        private static void handleRB0INT(Pic16F84 pic, bool globalInterruptEnabled)
        {
            bool isActivated = pic.Memory.getBit((ADDRESS)RegistersBank0.INTCON, 4);
            bool risingEdge = pic.Memory.getBitDirectAddress((ADDRESS)RegistersBank1.OPTION_REG, 6);
            bool rb0_new = pic.Memory.getBitDirectAddress((ADDRESS)RegistersBank0.PORTB, 0);

            if (pic.Memory.getBitDirectAddress((ADDRESS)RegistersBank0.INTCON, 1))
            {
                rb0_old = rb0_new;
                if (isActivated && globalInterruptEnabled)
                {
                    executeInterrupt(pic);
                }
                return;
            }

            if (rb0_old == rb0_new)
            {
                return;
            } 
            else
            {
                rb0_old = rb0_new;
                //rising edge
                if (risingEdge != rb0_new )
                    return;
            }

            pic.Memory.setBit((ADDRESS)RegistersBank0.INTCON, 1, true);
            if (isActivated && globalInterruptEnabled)
            {
                executeInterrupt(pic);
            }
        }

        private static void handleTMR0(Pic16F84 pic, bool globalInterruptEnabled, int cycles)
        {
            bool isActivated = pic.Memory.getBit((ADDRESS)RegistersBank0.INTCON, 5);
            bool hardwareClock = pic.Memory.getBitDirectAddress((ADDRESS)RegistersBank1.OPTION_REG, 5);
            bool risingEdge = !pic.Memory.getBitDirectAddress((ADDRESS)RegistersBank1.OPTION_REG, 4);
            bool ra4_new = pic.Memory.getBitDirectAddress((ADDRESS)RegistersBank0.PORTA, 4);
            bool applyPrescaler = pic.Memory.getBitDirectAddress((ADDRESS)RegistersBank1.OPTION_REG, 3);

            pic.Memory.getPrescalerTMR0();

            if (pic.Memory.getBitDirectAddress((ADDRESS)RegistersBank0.INTCON, 2))
            {
                ra4_old = ra4_new;
                if (isActivated && globalInterruptEnabled)
                {
                    executeInterrupt(pic);
                }
                return;
            }

            if (hardwareClock)
            {

                if (ra4_old == ra4_new)
                    {
                        return;
                    }    
                else
                {
                    ra4_old = ra4_new;
                    //rising edge
                    if (risingEdge != ra4_new)
                        return;
                }
            }

            int newVal = 0;

            if (applyPrescaler)
            {
                countedCycles = countedCycles + cycles;
                if (countedCycles >= pic.Memory.getPrescalerTMR0())
                {
                    newVal = pic.Memory.getByte((ADDRESS)RegistersBank0.TMR0) + 1;
                    countedCycles = countedCycles % pic.Memory.getPrescalerTMR0();
                }
            }
            else
            {
                newVal = pic.Memory.getByte((ADDRESS)RegistersBank0.TMR0) + cycles;
            }

            //check if interrupt required
            if (newVal > 0xFF)
            {
                pic.Memory.setBit((ADDRESS)RegistersBank0.INTCON, 2, true);
                pic.Memory.setByte(((ADDRESS)RegistersBank0.TMR0), 0);
                //execute interrupt
                if (isActivated && globalInterruptEnabled)
                {
                    executeInterrupt(pic);
                }
            }
            else
            {
                pic.Memory.setByte(((ADDRESS)RegistersBank0.TMR0), (byte)newVal);
            }
        }

        private static void handlePORTB(Pic16F84 pic, bool globalInterruptEnabled)
        {
            bool isActivated = pic.Memory.getBit((ADDRESS)RegistersBank0.INTCON, 3);
            byte trisb = pic.Memory.getByteDirectAddress((byte)RegistersBank1.TRISB);
            byte rb4to7_new = (byte)(pic.Memory.getByteDirectAddress((byte)RegistersBank0.PORTB) & 0b1111_0000);
            byte trisb_check = 0;
            byte rb4to7_new_check = 0;
            byte rb4to7_old_check = 0;

            if (pic.Memory.getBitDirectAddress((ADDRESS)RegistersBank0.INTCON, 0))
            {
                rb4to7_old = rb4to7_new;
                if (isActivated && globalInterruptEnabled)
                {
                    executeInterrupt(pic);
                }
                return;
            }

            if (rb4to7_old == rb4to7_new)
            {
                return;
            }
            else
            {
                for (int i = 4; i < 8; i++)
                {
                    trisb_check = (byte)(trisb >> i);
                    trisb_check = (byte)(trisb_check & 0x1);
                    rb4to7_new_check = (byte)(rb4to7_new >> i);
                    rb4to7_new_check = (byte)(rb4to7_new_check & 0x1);
                    rb4to7_old_check = (byte)(rb4to7_old >> i);
                    rb4to7_old_check = (byte)(rb4to7_old_check & 0x1);
                    if (rb4to7_new_check != rb4to7_old_check)
                    {
                        pic.Memory.setBit((ADDRESS)RegistersBank0.INTCON, 0, true);
                        if (isActivated && globalInterruptEnabled && trisb_check != 0)
                        {
                            executeInterrupt(pic);
                        }
                        rb4to7_old = rb4to7_new;
                        return;
                    }
                }
            }
        }

        public static void handleInterrupts(Pic16F84 pic, int cycles)
        {
            bool globalInterruptEnabled = pic.Memory.getBitDirectAddress((ADDRESS)RegistersBank0.INTCON, 7);
            handleRB0INT(pic, globalInterruptEnabled);
            handleTMR0(pic, globalInterruptEnabled, cycles);
            handlePORTB(pic, globalInterruptEnabled);
        }
    }
}
