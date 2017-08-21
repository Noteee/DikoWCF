using DiKo.FileSharing;
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
using System.Windows.Threading;
using WpfAnimatedGif;
using SQLDAL;

namespace DiKo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
   
    public partial class MainWindow : Window
    {
        private static List<Object> myShareList = new List<object>();
        MenuWindow menuWindow = new MenuWindow();
        public MainWindow()
        {
            InitializeComponent();
            setSplashScreen();
            StartCloseTimer();
        }

        public void setSplashScreen()
        {
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

        }
        private void StartCloseTimer()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(4d);
            timer.Tick += TimerTick;
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            DispatcherTimer timer = (DispatcherTimer)sender;
            timer.Stop();
            timer.Tick -= TimerTick;
            menuWindow.Show();
            Close();
        }
    }
}
