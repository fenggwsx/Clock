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


namespace Clock
{
    public delegate void WindowCloseDelegate();
    public delegate void StartClockDelegate(double seconds);
    public delegate void TopmostWindowDelegate(bool top);
    public delegate void MaximizeClockWindowDelegate(bool maximize);
    public delegate void ShowClockWindowDelegate(bool show);
    public partial class ConsoleWindow : Window
    {

        public event WindowCloseDelegate WindowCloseEvent;
        public event StartClockDelegate StartClockEvent;
        public event TopmostWindowDelegate TopmostWindowEvent;
        public event MaximizeClockWindowDelegate MaximizeClockWindowEvent;
        public event ShowClockWindowDelegate ShowClockWindowEvent;
        public ConsoleWindow()
        {
            InitializeComponent();
        }

        private void Button_StartClock(object sender, RoutedEventArgs e)
        {
            if (TimeInput.Text != null && System.Text.RegularExpressions.Regex.IsMatch(TimeInput.Text, @"^([1-9]\d*(\.\d+)?|0\.\d*[1-9]\d*)$")) // 正则表达式验证输入合法性
            {
                double time = double.Parse(TimeInput.Text); // 解析
                StartClockEvent(time * 60);
            }
            else
            {
                MessageBox.Show("请输入一个正确的时长", "输入错误", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            WindowCloseEvent();
        }

        private void TopmostWindow_Checked(object sender, RoutedEventArgs e)
        {
            Topmost =  true;
            TopmostWindowEvent(true);
        }

        private void TopmostWindow_Unchecked(object sender, RoutedEventArgs e)
        {
            Topmost = false;
            TopmostWindowEvent(false);
        }

        private void MaximizeClockWindow_Checked(object sender, RoutedEventArgs e)
        {
            MaximizeClockWindowEvent(true);
        }

        private void MaximizeClockWindow_Unchecked(object sender, RoutedEventArgs e)
        {
            MaximizeClockWindowEvent(false);
        }

        private void ShowClockWindow_Checked(object sender, RoutedEventArgs e)
        {
            ShowClockWindowEvent(true);
        }

        private void ShowClockWindow_Unchecked(object sender, RoutedEventArgs e)
        {
            ShowClockWindowEvent(false);
        }
    }
}
