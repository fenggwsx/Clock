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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Clock
{
    public partial class ClockWindow : Window
    {
        private int startTick;
        private int curTick;
        private int totTick;
        private readonly DispatcherTimer disTimer = new DispatcherTimer();
        private readonly SolidColorBrush colorNormal = new SolidColorBrush(Colors.Green);
        private readonly SolidColorBrush colorWarning = new SolidColorBrush(Colors.Red);
        private readonly ConsoleWindow consoleWindow = new ConsoleWindow();
        public ClockWindow()
        {
            InitializeComponent();
        }

        public void StartClock(double seconds)
        {
            totTick = (int)(seconds * 1000);
            startTick = Environment.TickCount;
            disTimer.Tick += new EventHandler(DisTimer_Tick);
            disTimer.Interval = new TimeSpan(1); // 0为一直执行 占用CPU过高
            disTimer.Start();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // 相应消息以支持鼠标在任意地方拖动窗口
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }


        public void DisTimer_Tick(object sender, EventArgs e)
        {
            curTick = Environment.TickCount;
            if (curTick > startTick + totTick)
            {
                ProgressBar.Value = 100; // 计时结束设为100%
                Tip.Text = "时间到"; // 计时结束提示文本
                disTimer.Stop(); // 计时停止
            }
            else
            {
                double percentage = (curTick - startTick) * 100 / (double)totTick; // 计算百分比
                if (percentage > 90) ProgressBar.Foreground = colorWarning; // 颜色改为警告的红色
                else ProgressBar.Foreground = colorNormal; // 颜色为普通的绿色
                ProgressBar.Value = percentage; // 设置进度
                Tip.Text = 
                    string.Format("{0:D2}", (totTick - curTick + startTick) / 1000 / 60) + ':' + 
                    string.Format("{0:D2}",(totTick - curTick + startTick) / 1000 % 60); // 格式化输出
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            consoleWindow.Owner = this;
            consoleWindow.WindowCloseEvent += Close;
            consoleWindow.StartClockEvent += StartClock;
            consoleWindow.TopmostWindowEvent += delegate (bool top) { Topmost = top; };
            consoleWindow.MaximizeClockWindowEvent += delegate (bool maximize) { WindowState = maximize ? WindowState.Maximized : WindowState.Normal; };
            consoleWindow.ShowClockWindowEvent += delegate (bool show) { Visibility = show ? Visibility.Visible : Visibility.Hidden; };
            consoleWindow.TopmostWindow.IsChecked = true;
            consoleWindow.ShowClockWindow.IsChecked = true;
            consoleWindow.Show();
        }
    }
}
