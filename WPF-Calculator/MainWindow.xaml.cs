using System;
using System.Collections.Generic;
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
        private string Display { get; set; }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnMenuExit (object sender, RoutedEventArgs e)
        {

        }

        private void OnMenuView(object sender, RoutedEventArgs e)
        {

        }

        private void OnMenuAbout(object sender, RoutedEventArgs e)
        {

        }

        private void NumericButton_Click(object sender, RoutedEventArgs e)
        {
            var num = ((Button)sender).Content.ToString();
            var numId = num.ToCharArray();
            ProcessKey(numId[0]);
        }

        private void ProcessKey(char c)
        {
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
                else if(c == '\b')                 //delete all with backSpace
                {
                    if (Display.Length >= 1)
                        Display = string.Empty;
                    else
                    {
                        var i = Display.Length;
                        Display = Display.Remove(i - 1, 1); //delete last char
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

        }
    }
}
