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
using static DiKo.FileSharing.Treeview;
using System.ServiceModel.Discovery;
using System.Runtime.InteropServices;
using System.IO;
using SharingServer;
using Tulpep.NotificationWindow;

namespace DiKo
{
    /// <summary>
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {

        SharingService Server = new SharingService();
        private static DuplexChannelFactory<ISharingService> _channelFactory;
        private int channels = 0;
        private List<string> connectedClients = new List<string>();
        SharingConnection conn = new SharingConnection();

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

            /*_channelFactory = new DuplexChannelFactory<ISharingService>(new ClientCallback(), "FileSharingEndPoint");
            Server = _channelFactory.CreateChannel();
            int loginValue = Server.Login(Environment.MachineName);*/
            conn.Sharing_SetupChannel();
            conn.Sharing_DiscoverChannel();

            connectedClients = conn.getUrisList();
            int loginvalue = Server.Login(connectedClients);
            Console.WriteLine(conn.countConnectedChannels().ToString());
            channels = conn.countConnectedChannels();

            updateTimerForClients();


            //if (loginValue == 1)
            //{
            //  MessageBox.Show("Already running! 1 window is allowed!");
            //}
            //else
            //{
            //List<FileShareHandler> getData = SharedFileBrowsing.Browser.GetMySharedFiles();
            //    Server.getTables(Environment.MachineName, getData);
            /*List<FileShareHandler> testList = new List<FileShareHandler>();
            testList.Add(new FileShareHandler("nev", "kiterjesztes", "eleres", "meret"));
            testList.Add(new FileShareHandler("na", "ne", "mar", "megint"));
            testList.Add(new FileShareHandler("ott", "vagyunk", "mar", "bleeh"));
            fillSharedFiles(testList);*/


            //}

        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            downloadPath.getDownloadFolder();
            downloadPath.getPath();
            downloadPath.SetEnvinronmentalVariable(downloadPath.getPath());
            
        }

        public void fillSharedFiles(List<FileShareHandler> files)
        {
            
            DataGrid grid = itemsSharedWithMeGrid;
            grid.Items.Clear();
            if (files.Count == 0)
            {
                MessageBox.Show("No shared files right now!");
               
                
            }
            else
            {
                foreach (FileShareHandler file in files)
                {
                    
                    grid.Items.Add(new DataItem{ fileName = file.FileName, fileEx = file.FileExtension, filePath = file.FilePath, fileSize =  file.FileSize });
                }
            }
            
            
        }
        private void updateTimerForClients()
        {
            DispatcherTimer updateTimer =new DispatcherTimer();
            updateTimer.Tick += new EventHandler(updateTimer_Tick);
            updateTimer.Interval = TimeSpan.FromSeconds(1);
            updateTimer.Start();
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            conn.Sharing_DiscoverChannel();
            List<string> connectedClientsUpdate = conn.getUrisList();
            int channelUpdate = conn.countConnectedChannels();

            if (channelUpdate > channels)
            {
                connectedClients = connectedClientsUpdate;
                channels = channelUpdate;
                MessageBox.Show(channels.ToString());
                int value = Server.Login(connectedClients);
            }
            if (channelUpdate < channels)
            {
                connectedClients = connectedClientsUpdate;
                channels = channelUpdate;
                MessageBox.Show(channels.ToString());
                Server.Logout();
            }

        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Server.Logout();

            base.OnClosing(e);
        }




    }
}
