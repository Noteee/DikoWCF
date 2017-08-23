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
using System.Windows.Shapes;
using SQLDAL;
using System.Windows.Threading;
using MahApps.Metro.Controls;
using SharingInterfaces;
using System.ServiceModel;
using DiKo.Service;

namespace DiKo
{
    /// <summary>
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        public static ISharingService Server;
        private static DuplexChannelFactory<ISharingService> _channelFactory;

        getDownloadPath downloadPath = new getDownloadPath();
        LoadingScreen loadingScreen = new LoadingScreen();
        public MenuWindow()
        {
            InitializeComponent();
            downloadPath.setPath(downloadPath.GetEnvironmentalVariable());
            //this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            FileData datagrid = new FileData(sharedGrid);
            FileData datagridShared = new FileData(itemsSharedWithMeGrid);
            datagrid.createDataGrid();
            datagridShared.createDataGrid();
            Treeview tree = new Treeview(whatToShareTreeView, sharedGrid);
            tree.Window_Loaded();
            SQLDAL.SQLDAL.ConnecToDB();
            sharedContentPanel.Visibility = Visibility.Visible;
            sharedTreeViewPanel.Visibility = Visibility.Visible;
            itemsSharedWithMePanel.Visibility = Visibility.Hidden;
            SharedDataGrid.Background = Brushes.DimGray;
            TreeGrid.Background = Brushes.DarkGray;
            sharedWithMe.Background = Brushes.Transparent;
            ShareButtonPanel.Visibility = Visibility.Visible;
            itemsSharedWithMeGrid.Visibility = Visibility.Hidden;
            downLoadPanel.Visibility = Visibility.Hidden;
            wishListPanel.Visibility = Visibility.Hidden;
            refreshPanel.Visibility = Visibility.Hidden;
            searchPanel.Visibility = Visibility.Hidden;
        }

        private void shareButton_Click(object sender, RoutedEventArgs e)
        {
            SQLDAL.SQLDAL.WriteListToDB(Treeview.GetSharedFileList(Treeview.GetCurrentDataGrid()));
        }

        private void StartCloseTimer()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3d);
            timer.Tick += TimerTick;
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            DispatcherTimer timer = (DispatcherTimer)sender;
            timer.Stop();
            timer.Tick -= TimerTick;
            loadingScreen.Hide();
            MessageBox.Show("Shared Files Added!");
        }


        private void shareButtonLabel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            StartCloseTimer();
            loadingScreen.Show();
            loadingScreen.Topmost = true;


        }

        private void sharedItemsButton_Click(object sender, RoutedEventArgs e)
        {
            sharedContentPanel.Visibility = Visibility.Visible;
            sharedTreeViewPanel.Visibility = Visibility.Visible;
            itemsSharedWithMePanel.Visibility = Visibility.Hidden;
            SharedDataGrid.Background = Brushes.DimGray;
            TreeGrid.Background = Brushes.DarkGray;
            sharedWithMe.Background = Brushes.Transparent;
            ShareButtonPanel.Visibility = Visibility.Visible;
            itemsSharedWithMeGrid.Visibility = Visibility.Hidden;
            downLoadPanel.Visibility = Visibility.Hidden;
            wishListPanel.Visibility = Visibility.Hidden;
            refreshPanel.Visibility = Visibility.Hidden;
            searchPanel.Visibility = Visibility.Hidden;



        }

        private void itemsSharedWithMe_Click(object sender, RoutedEventArgs e)
        {
            sharedContentPanel.Visibility = Visibility.Hidden;
            sharedTreeViewPanel.Visibility = Visibility.Hidden;
            itemsSharedWithMePanel.Visibility = Visibility.Visible;
            sharedWithMe.Background = Brushes.DimGray;
            ShareButtonPanel.Visibility = Visibility.Hidden;
            itemsSharedWithMeGrid.Visibility = Visibility.Visible;
            downLoadPanel.Visibility = Visibility.Visible;
            wishListPanel.Visibility = Visibility.Visible;
            refreshPanel.Visibility = Visibility.Visible;
            searchPanel.Visibility = Visibility.Visible;

            _channelFactory = new DuplexChannelFactory<ISharingService>(new ClientCallback(), "FileSharingEndPoint");
            Server = _channelFactory.CreateChannel();
            //MessageBox.Show(Server.hello()); 
            
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            downloadPath.getDownloadFolder();
            downloadPath.getPath();
            downloadPath.SetEnvinronmentalVariable(downloadPath.getPath());
            
        }
    }
}
