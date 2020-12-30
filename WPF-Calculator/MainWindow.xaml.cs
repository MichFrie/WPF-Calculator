using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
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

namespace WPF_Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {   
        private Operation lastOperation;
        private string lastValue;
        private static DisplayBoard displayBoard;
        
        private string Display { get; set; }
        public bool EraseDisplay { get; set; }
        public string LastValue
        {
            get 
            { 
                if(lastValue == string.Empty)
                    return "0";
                return lastValue;
            }
            set { lastValue = value; }
        }

        public MainWindow()
        {
            InitializeComponent();
            displayBoard = new DisplayBoard(this);
            EraseDisplay = true;
        }

        private void OnMenuExit (object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnMenuAbout(object sender, RoutedEventArgs e)
        {
            var parent = (Window)TopPanelMenu.Parent;
            MessageBox.Show(parent, parent.Title + " by M. H. in S.", parent.Title, MessageBoxButton.OK);
        }

        private void OnWindowKeyDown()
        {

        }

        
        private void NumericButton_Click(object sender, RoutedEventArgs e)
        {
            var num = ((Button)sender).Content.ToString();
            var numCharArray = num.ToCharArray();
            ProcessKey(numCharArray[0]);
        }

        private void ProcessKey(char c)
        {
            if (EraseDisplay)
            {
                Display = string.Empty;
                EraseDisplay = false;
            }
            AddToDisplay(c);
        }

        private void AddToDisplay(char c)
        {
           if(c == '.')
            {
                if(Display.IndexOf('.',0) >= 0)
                {
                    return;
                }
                Display = Display + c;
            }
            else
            {
                if(c >= '0' && c <= '9')
                {
                    Display = Display + c;
                }
                else if(c == '\b')                 
                {
                    if (Display.Length >= 1)
                        Display = string.Empty;
                    else
                    {
                        var i = Display.Length;
                        Display = Display.Remove(i - 1, 1); 
                    }
                }
            }
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            DisplayBox.Text = Display == string.Empty ? "0" : Display;
        }

        private void OperatingButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessOperation(((Button)sender).Name);
        }

        private void ProcessOperation(string s)
        {
            var d = 0.0;
            switch (s)
            {
                case "BPlusMinus":
                    lastOperation = Operation.Negate;
                    LastValue = Display;
                    CalcResults();
                    LastValue = Display;
                    EraseDisplay = true;
                    lastOperation = Operation.None;
                    break;
                case "BDevide":
                    if (EraseDisplay)
                    {
                        lastOperation = Operation.Devide;
                        break;
                    }
                    CalcResults();
                    lastOperation = Operation.Devide;
                    LastValue = Display;
                    EraseDisplay = true;
                    break;
                case "BMultiply":
                    if (EraseDisplay)
                    {
                        lastOperation = Operation.Multiply;
                        break;
                    }
                    CalcResults();
                    lastOperation = Operation.Multiply;
                    LastValue = Display;
                    EraseDisplay = true;
                    break;
                case "BMinus":
                    if (EraseDisplay)
                    {
                        lastOperation = Operation.Subtract;
                        break;
                    }
                    CalcResults();
                    lastOperation = Operation.Subtract;
                    LastValue = Display;
                    EraseDisplay = true;
                    break;
                case "BPlus":
                    if (EraseDisplay)
                    {
                        lastOperation = Operation.Add;
                        break;
                    }
                    CalcResults();
                    lastOperation = Operation.Add;
                    LastValue = Display;
                    EraseDisplay = true;
                    break;
                case "BEqual":
                    if (EraseDisplay) 
                        break;
                    CalcResults();
                    EraseDisplay = true;
                    lastOperation = Operation.None;
                    LastValue = Display;
                    break;
                case "BSqrt":
                    lastOperation = Operation.Sqrt;
                    LastValue = Display;
                    CalcResults();
                    LastValue = Display;
                    EraseDisplay = true;
                    lastOperation = Operation.None;
                    break;
                case "BPercent":
                    if (EraseDisplay)
                    {
                        lastOperation = Operation.Percent;
                        break;
                    }
                    CalcResults();
                    lastOperation = Operation.Percent;
                    LastValue = Display;
                    EraseDisplay = true;
                    //lastOperation = Operation.None;
                    break;
                case "BOneOver":
                    lastOperation = Operation.OneX;
                    LastValue = Display;
                    CalcResults();
                    LastValue = Display;
                    EraseDisplay = true;
                    lastOperation = Operation.None;
                    break;
                case "BC": 
                    lastOperation = Operation.None;
                    Display = LastValue = string.Empty;
                    //_paper.Clear();
                    UpdateDisplay();
                    break;
                case "BCE": 
                    lastOperation = Operation.None;
                    Display = LastValue;
                    UpdateDisplay();
                    break;
            }
        }

        private void CalcResults()
        {
            double d;
            if (lastOperation == Operation.None)
                return;
            d = Calc(lastOperation);
            Display = d.ToString(CultureInfo.InvariantCulture);
            UpdateDisplay();
        }

        private double Calc(Operation lastOperation)
        {
            var d = 0.0;
            try
            {
                switch (lastOperation)
                {
                    case Operation.Devide:
                        displayBoard.AddArguments(LastValue + " / " + Display);
                        d = Convert.ToDouble(LastValue) / Convert.ToDouble(Display);
                        CheckResult(d);
                        displayBoard.AddResult(d.ToString(CultureInfo.InvariantCulture));
                        break;
                    case Operation.Add:
                        displayBoard.AddArguments(LastValue + " + " + Display);
                        d = Convert.ToDouble(LastValue) / Convert.ToDouble(Display);
                        CheckResult(d);
                        displayBoard.AddResult(d.ToString(CultureInfo.InvariantCulture));
                        break;
                    case Operation.Multiply:
                        displayBoard.AddArguments(LastValue + " * " + Display);
                        d = Convert.ToDouble(LastValue) * Convert.ToDouble(Display);
                        CheckResult(d);
                        displayBoard.AddResult(d.ToString(CultureInfo.InvariantCulture));
                        break;
                    case Operation.Subtract:
                        displayBoard.AddResult(LastValue + " - " + Display);
                        d = Convert.ToDouble(LastValue) - Convert.ToDouble(Display);
                        CheckResult(d);
                        displayBoard.AddResult(d.ToString(CultureInfo.InvariantCulture));
                        break;
                    case Operation.Percent:
                        displayBoard.AddArguments(LastValue + " % " + Display);
                        d = (Convert.ToDouble(LastValue) * Convert.ToDouble(Display)) / 100.0F;
                        CheckResult(d);
                        displayBoard.AddResult(d.ToString(CultureInfo.InvariantCulture));
                        break;
                }
            }
        }

        private void CheckResult(double d)
        {
            if (double.IsNegativeInfinity(d) || double.IsPositiveInfinity(d) || double.IsNaN(d))
                throw new Exception("illegal Value");
        }

        private enum Operation
        {
            None, 
            Devide, 
            Multiply,
            Subtract, 
            Add, 
            Percent, 
            Sqrt, 
            OneX, 
            Negate
        }

        private class DisplayBoard
        {
            private readonly MainWindow window;
            private string args;

            public DisplayBoard(MainWindow mainWindow)
            {
                window = mainWindow;
            }

            public void AddArguments(string a)
            {
                args = a;
            }

            public void AddResult(string r)
            {
                window.CalculationResults.Text += args + " = " + r + "\n";
            }

            public void Clear()
            {
                window.CalculationResults.Text = string.Empty;
                args = string.Empty;
            }
        }
    }
}
