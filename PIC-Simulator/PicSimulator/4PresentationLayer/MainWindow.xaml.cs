using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using PicSimulator.Simulator;
using PicSimulator._2ApplicationLayer;
using PicSimulator._1DomainLayer;
using PicSimulator._3InfrastructureLayer;

namespace PicSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public enum SpecialRegister : int
    {
        W = 0,
        PC13 = 1,
        Prescaler = 2
    }

    public partial class MainWindow : Window
    {
        ObservableCollection<CodeLine> CodeLines;
        ObservableCollection<CodeLine> CodeLinesReal;

        ObservableCollection<BankLine> Bank0Lines;
        ObservableCollection<BankLine> Bank1Lines;

        ObservableCollection<StackLine> StackLines;
        StackAdapter stackAdapter = new StackAdapter();

        ObservableCollection<SpecialLine> SpecialLines;

        BankLine BlStatus;
        BankLine BlOption;

        BankLine BlPortA;
        BankLine BlPortB;

        BankLine BlTrisA;
        BankLine BlTrisB;

        Pic16F84 pic;
        ProgramCounter picProgramCounter;
        Memory picMemory;
        PicStack picStack;

        TimeData timeData;



        byte[] mem = new byte[] { 1, 2, 3, 4 };

        CancellationTokenSource taskSource;
        CancellationToken token;
        Task task;



        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            taskSource = new CancellationTokenSource();
            token = taskSource.Token;
            task = Task.Run(() => PicAutoRun(token));
        }

        private void btnStep_Click(object sender, RoutedEventArgs e)
        {
            setActiveLineGetBp(pic.ProgramCounter.ProgramCounterValue, true);
            executeNextInstruciton();
        }


        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            taskSource?.Cancel();
        }

        private void PicAutoRun(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                bool breakPoint = setActiveLineGetBp(pic.ProgramCounter.ProgramCounterValue, false);
                if (breakPoint) break;

                Thread.Sleep(100);
                executeNextInstruciton();
            }
        }

        private void executeNextInstruciton()
        {
            pic.executeNextInstruction();
            TimeUpdate(pic.TotalCycles);
            runDispatchers();
        }

        private void runDispatchers()
        {

            Dispatcher.Invoke(() =>
            {
                CollectionViewSource.GetDefaultView(SpecialLines).Refresh();
                CollectionViewSource.GetDefaultView(Bank0Lines).Refresh();
                CollectionViewSource.GetDefaultView(Bank1Lines).Refresh();
                //call for otehr method;s
                //lol
                StackLines.Clear();
                foreach (var line in stackAdapter.StackLines)
                    StackLines.Add(line);
            });
        }

        private void SetupComponents()
        {

            SpecialLines = new ObservableCollection<SpecialLine>();
            SpecialLines.Add(new ProgramCounterSpecialLine(picProgramCounter));
            SpecialLines.Add(new WRegisterSpecialLine(pic.WRegisterProperty));
            SpecialLines.Add(new PrescalerSpecialLine(picMemory));

            CodeLines = new ObservableCollection<CodeLine>();
            CodeLinesReal = new ObservableCollection<CodeLine>();
            StackLines = new ObservableCollection<StackLine>();
            Bank0Lines = new ObservableCollection<BankLine>();
            Bank1Lines = new ObservableCollection<BankLine>();

            timeData = new TimeData(3);

            SetupBankLines();
            SetupDataContext();
        }

        private void SetupBankLines()
        {
            for (int i = 0; i < 128; i++)
            {
                Bank0Lines.Add(new BankLine((UInt16)i, 0, picMemory, 0));
            }

            for (int i = 0; i < 128; i++)
            {
                Bank1Lines.Add(new BankLine((UInt16)i, 0, picMemory, 1));
            }

            Bank0Lines[(UInt16)RegistersBank0.INDF].SfrName = "INDF";
            Bank0Lines[(UInt16)RegistersBank0.TMR0].SfrName = "TMR0";
            Bank0Lines[(UInt16)RegistersBank0.PCL].SfrName = "PCL";
            Bank0Lines[(UInt16)RegistersBank0.STATUS].SfrName = "STATUS";
            Bank0Lines[(UInt16)RegistersBank0.FSR].SfrName = "FSR";
            Bank0Lines[(UInt16)RegistersBank0.PORTA].SfrName = "PORTA";
            Bank0Lines[(UInt16)RegistersBank0.PORTB].SfrName = "PORTB";
            Bank0Lines[(UInt16)RegistersBank0.UNIMP].SfrName = "UNIMP";
            Bank0Lines[(UInt16)RegistersBank0.EEDATA].SfrName = "EEDATA";
            Bank0Lines[(UInt16)RegistersBank0.EEADR].SfrName = "EEADR";
            Bank0Lines[(UInt16)RegistersBank0.PCLATH].SfrName = "PCLATH";
            Bank0Lines[(UInt16)RegistersBank0.INTCON].SfrName = "INTCON";

            Bank1Lines[(UInt16)RegistersBank1.INDF - 0x80].SfrName = "INDF";
            Bank1Lines[(UInt16)RegistersBank1.OPTION_REG - 0x80].SfrName = "OPTION";
            Bank1Lines[(UInt16)RegistersBank1.PCL - 0x80].SfrName = "PCL";
            Bank1Lines[(UInt16)RegistersBank1.STATUS - 0x80].SfrName = "STATUS";
            Bank1Lines[(UInt16)RegistersBank1.FSR - 0x80].SfrName = "FSR";
            Bank1Lines[(UInt16)RegistersBank1.TRISA - 0x80].SfrName = "TRISA";
            Bank1Lines[(UInt16)RegistersBank1.TRISB - 0x80].SfrName = "TRISB";
            Bank1Lines[(UInt16)RegistersBank1.UNIMP - 0x80].SfrName = "UNIMP";
            Bank1Lines[(UInt16)RegistersBank1.EECON1 - 0x80].SfrName = "EECON1";
            Bank1Lines[(UInt16)RegistersBank1.EECON2 - 0x80].SfrName = "EECON2";
            Bank1Lines[(UInt16)RegistersBank1.PCLATH - 0x80].SfrName = "PCLATH";
            Bank1Lines[(UInt16)RegistersBank1.INTCON - 0x80].SfrName = "INTCON";

            BlStatus = Bank0Lines[(UInt16)RegistersBank0.STATUS];
            BlStatus.Bit0Text = "C";
            BlStatus.Bit1Text = "DC";
            BlStatus.Bit2Text = "Z";
            BlStatus.Bit3Text = "PD";
            BlStatus.Bit4Text = "TO";
            BlStatus.Bit5Text = "RP0";
            BlStatus.Bit6Text = "RP1";
            BlStatus.Bit7Text = "IRP";

            BlOption = Bank1Lines[(UInt16)RegistersBank1.OPTION_REG - 0x80];
            BlOption.Bit0Text = "PS0";
            BlOption.Bit1Text = "PS1";
            BlOption.Bit2Text = "PS2";
            BlOption.Bit3Text = "PSA";
            BlOption.Bit4Text = "T0SE";
            BlOption.Bit5Text = "T0CS";
            BlOption.Bit6Text = "INTEDG";
            BlOption.Bit7Text = "RBPU";

            BlPortA = Bank0Lines[(UInt16)RegistersBank0.PORTA];
            BlPortA.Bit0Text = "RA0";
            BlPortA.Bit1Text = "RA1";
            BlPortA.Bit2Text = "RA2";
            BlPortA.Bit3Text = "RA3";
            BlPortA.Bit4Text = "RA4";
            BlPortA.Bit5Text = "N/A";
            BlPortA.Bit6Text = "N/A";
            BlPortA.Bit7Text = "N/A";

            BlPortB = Bank0Lines[(UInt16)RegistersBank0.PORTB];
            BlPortB.Bit0Text = "RB0";
            BlPortB.Bit1Text = "RB1";
            BlPortB.Bit2Text = "RB2";
            BlPortB.Bit3Text = "RB3";
            BlPortB.Bit4Text = "RB4";
            BlPortB.Bit5Text = "RB5";
            BlPortB.Bit6Text = "RB6";
            BlPortB.Bit7Text = "RB7";

            BlTrisA = Bank1Lines[(UInt16)RegistersBank1.TRISA - 0x80];
            BlTrisA.Bit0Text = "0";
            BlTrisA.Bit1Text = "1";
            BlTrisA.Bit2Text = "2";
            BlTrisA.Bit3Text = "3";
            BlTrisA.Bit4Text = "4";
            BlTrisA.Bit5Text = "5";
            BlTrisA.Bit6Text = "6";
            BlTrisA.Bit7Text = "7";

            BlTrisB = Bank1Lines[(UInt16)RegistersBank1.TRISB - 0x80];
            BlTrisB.Bit0Text = "0";
            BlTrisB.Bit1Text = "1";
            BlTrisB.Bit2Text = "2";
            BlTrisB.Bit3Text = "3";
            BlTrisB.Bit4Text = "4";
            BlTrisB.Bit5Text = "5";
            BlTrisB.Bit6Text = "6";
            BlTrisB.Bit7Text = "7";
        }

        private void SetupDataContext()
        {
            foreach (CheckBox cb in spTrisA.Children)
            {
                cb.DataContext = BlTrisA;
            }

            foreach (CheckBox cb in spTrisB.Children)
            {
                cb.DataContext = BlTrisB;
            }

            foreach (CheckBox cb in spStatus.Children)
            {
                cb.DataContext = BlStatus;
            }

            foreach (CheckBox cb in spOption.Children)
            {
                cb.DataContext = BlOption;
            }

            foreach (CheckBox cb in spPortA.Children)
            {
                cb.DataContext = BlPortA;
            }

            foreach (CheckBox cb in spPortB.Children)
            {
                cb.DataContext = BlPortB;
            }

            sliderFreq.DataContext = timeData;
            tbFreq.DataContext = timeData;

            tbCycles.DataContext = timeData;
            tbTime.DataContext = timeData;

            StackListBox.ItemsSource = StackLines;

            dgSpecial.ItemsSource = SpecialLines;
            dgCode.ItemsSource = CodeLines;

            dgBank0.ItemsSource = Bank0Lines;
            dgBank1.ItemsSource = Bank1Lines;
        }


        public MainWindow(Pic16F84 pic, PicStack picStack, Memory picMemory, ProgramCounter picProgramCounter)
        {
            InitializeComponent();
            this.pic = pic;
            this.picStack = picStack;
            this.picMemory = picMemory;
            this.picProgramCounter = picProgramCounter;
            SetupComponents();

        }
        
       
        public void TimeUpdate(uint cycles)
        {
            timeData.Cycles = cycles;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Test methods here

        }

        public bool setActiveLineGetBp(UInt16 address, bool force)
        {
            bool breakPoint = false;
            int i;
            for (i = 0; i< CodeLinesReal.Count;i++) 
            {
                CodeLine cl = CodeLinesReal[i];
                if (cl.Address ==address)
                {
                    
                    breakPoint = cl.Bp;

                    if (!breakPoint)
                    { 
                        cl.Active = 1; // ---> Indicator
                    }
                    else
                    {
                        if (cl.Active == 2) // if already hit breakpoint once before
                        {
                            breakPoint = false;
                            cl.Active = 1;
                        }
                        else
                        {
                            //ignore breakpoint if stepping
                            if (force) cl.Active = 1; // ---> Indicator
                            else cl.Active = 2; // --| Indicator
                        }

                    }
                }
                else
                {
                    cl.Active = 0;
                }
            }
            Dispatcher.Invoke(() =>
            {
                dgCode.Items.Refresh();
            });

            return breakPoint; 
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "LST files (*.LST)|*.LST";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string[] fileLines = File.ReadAllLines(openFileDialog.FileName);
                    CodeLines.Clear();

                    for (int i = 0; i < fileLines.Length; i += 1)
                    {
                        string line = fileLines[i];

                        CodeLine cl = new CodeLine(line);

                        int matchesTotal = 0;
                        string pattern = @"[a-fA-F0-9]{4}";
                        Regex rg = new Regex(pattern);
                        MatchCollection matches = rg.Matches(line.Substring(0,4));
                        matchesTotal += matches.Count;

                        string part2 = line.Substring(5, 4);
                        matches = rg.Matches(line.Substring(5, 4));
                        matchesTotal += matches.Count;

                        if (matchesTotal == 2)
                        {
                            cl.Address = UInt16.Parse(line.Substring(0, 4), System.Globalization.NumberStyles.HexNumber);
                            cl.RealCode = true;
                            CodeLinesReal.Add(cl);
                        }
                        CodeLines.Add(cl);
                    }

                    List<string> pureCodeLines = new List<string>();
                    foreach (CodeLine cl in CodeLinesReal)
                    {
                        pureCodeLines.Add(cl.Line.Substring(0,9));
                    }

                    IStringProgramLoader programLoader = new StringProgramLoader();
                    programLoader.ProgramLines = pureCodeLines;
                    pic.ProgramMemory.loadProgram(programLoader);
                }
                catch
                {
                    
                }
            }           
        }

        private void btnReset(object sender, RoutedEventArgs e)
        {
            pic.powerOnReset();
        }
    }
}
