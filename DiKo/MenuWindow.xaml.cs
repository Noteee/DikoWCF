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
using System.Windows.Threading;

namespace DiKo
{
    /// <summary>
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        LoadingScreen loadingScreen = new LoadingScreen();
        public MenuWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            FileData datagrid = new FileData(sharedGrid);
            FileData datagridShared = new FileData(itemsSharedWithMeGrid);
            datagrid.createDataGrid();
            datagridShared.createDataGrid();
            Treeview tree = new Treeview(whatToShareTreeView, sharedGrid);
            tree.Window_Loaded();
            SQLDAL.SQLDAL.ConnecToDB();
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

        private void shareButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            StartCloseTimer();
            loadingScreen.Show();
            loadingScreen.Topmost = true;
            SQLDAL.SQLDAL.WriteListToDB(Treeview.GetSharedFileList(Treeview.GetCurrentDataGrid()));
        }

        private void shareButtonLabel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            StartCloseTimer();
            loadingScreen.Show();
            SQLDAL.SQLDAL.WriteListToDB(Treeview.GetSharedFileList(Treeview.GetCurrentDataGrid()));
            loadingScreen.Topmost = true;


        }

        public void test1()
        {
            sharedContentPanel.Visibility = Visibility.Visible;
            sharedTreeViewPanel.Visibility = Visibility.Visible;
            itemsSharedWithMePanel.Visibility = Visibility.Hidden;
            SharedDataGrid.Background = Brushes.DimGray;
            TreeGrid.Background = Brushes.DarkGray;
            sharedWithMe.Background = Brushes.Transparent;
            ShareButtonPanel.Visibility = Visibility.Visible;
            itemsSharedWithMeGrid.Visibility = Visibility.Hidden;
        }

        public void test2()
        {
            sharedContentPanel.Visibility = Visibility.Hidden;
            sharedTreeViewPanel.Visibility = Visibility.Hidden;
            itemsSharedWithMePanel.Visibility = Visibility.Visible;
            sharedWithMe.Background = Brushes.DimGray;
            ShareButtonPanel.Visibility = Visibility.Hidden;
            itemsSharedWithMeGrid.Visibility = Visibility.Visible;

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
            
        }
    }
}
