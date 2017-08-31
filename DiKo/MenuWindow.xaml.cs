using DiKo.FileSharing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
using SQLDAL;
using System.Windows.Threading;
using DiKo.SharedFileBrowsing;
using MahApps.Metro.Controls;

namespace DiKo
{
	/// <summary>
	/// Interaction logic for MenuWindow.xaml
	/// </summary>
	public partial class MenuWindow : Window
	{
		getDownloadPath downloadPath = new getDownloadPath();
		LoadingScreen loadingScreen = new LoadingScreen();
	    private List<FileShareHandler> itemList;
	    


        public MenuWindow()
		{
			InitializeComponent();
			downloadPath.setPath(downloadPath.GetEnvironmentalVariable());
			//this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
			FileData datagrid = new FileData(sharedGrid);
			FileData datagridShared = new FileData(itemsSharedWithMeGrid);
			FileData datagridMyWishList = new FileData(MyWishListGrid);
			datagrid.createDataGrid();
			datagridShared.createDataGrid();
			datagridMyWishList.createDataGrid();
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


		private void StartCloseTimer()
		{
			DispatcherTimer timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(3d);
			timer.Tick += TimerTick;
			timer.Start();
		}

		private void TimerTick(object sender, EventArgs e)
		{
			DispatcherTimer timer = (DispatcherTimer) sender;
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

		private void sharedPanelHide()
		{
			sharedContentPanel.Visibility = Visibility.Hidden;
			sharedTreeViewPanel.Visibility = Visibility.Hidden;
			ShareButtonPanel.Visibility = Visibility.Hidden;
		}

		private void sharedPanelShow()
		{
			sharedContentPanel.Visibility = Visibility.Visible;
			sharedTreeViewPanel.Visibility = Visibility.Visible;
			ShareButtonPanel.Visibility = Visibility.Visible;
		}

		private void itemsSharedWithMeHide()
		{
			itemsSharedWithMeGrid.Visibility = Visibility.Hidden;
			itemsSharedWithMePanel.Visibility = Visibility.Hidden;
		}

		private void itemsSharedWithMeShow()
		{
			itemsSharedWithMeGrid.Visibility = Visibility.Visible;
			itemsSharedWithMePanel.Visibility = Visibility.Visible;
		}

		private void bottomControlPanelShow()
		{
			downLoadPanel.Visibility = Visibility.Visible;
			wishListPanel.Visibility = Visibility.Visible;
			refreshPanel.Visibility = Visibility.Visible;
			searchPanel.Visibility = Visibility.Visible;
		}

		private void setBackgroundForSharedFiles()
		{
			SharedDataGrid.Background = Brushes.DimGray;
			TreeGrid.Background = Brushes.DarkGray;
			sharedWithMe.Background = Brushes.Transparent;
		}

		private void setBackgroundForItemsSharedWithMeAndWishList()
		{
			sharedWithMe.Background = Brushes.DimGray;
		}

		private void bottomControlPanelHide()
		{
			downLoadPanel.Visibility = Visibility.Hidden;
			wishListPanel.Visibility = Visibility.Hidden;
			refreshPanel.Visibility = Visibility.Hidden;
			searchPanel.Visibility = Visibility.Hidden;
		}


		private void sharedItemsButton_Click(object sender, RoutedEventArgs e)
		{
			sharedPanelShow();
			itemsSharedWithMeHide();
			bottomControlPanelHide();
			setBackgroundForSharedFiles();
			myWishListHide();
		}

		private void itemsSharedWithMe_Click(object sender, RoutedEventArgs e)
		{
			sharedPanelHide();
			itemsSharedWithMeShow();
			bottomControlPanelShow();
			setBackgroundForItemsSharedWithMeAndWishList();
			myWishListHide();
		}

		private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			downloadPath.getDownloadFolder();
			downloadPath.getPath();
			downloadPath.SetEnvinronmentalVariable(downloadPath.getPath());
		}


		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
		    var textBox = sender as TextBox;
		    string text = textBox.Text;
		    List<FileShareHandler> result = new List<FileShareHandler>();
            Browser myBrowser = new Browser();
            result = myBrowser.RegExSearch(text, itemList);
		    foreach (FileShareHandler data in result)
		    {
		        Console.WriteLine(data.FileName);
		    }

		}

		private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
		{
		}

		private void myWishListShow()
		{
			MyWishListPanel.Visibility = Visibility.Visible;
			MyWishListGrid.Visibility = Visibility.Visible;
		}

		private void myWishListHide()
		{
			MyWishListPanel.Visibility = Visibility.Hidden;
			MyWishListGrid.Visibility = Visibility.Hidden;
		}

		private void WishListMenuButton_Click(object sender, RoutedEventArgs e)
		{
			sharedPanelHide();
			itemsSharedWithMeHide();
			bottomControlPanelShow();
			setBackgroundForItemsSharedWithMeAndWishList();
			myWishListShow();
		}
	}
}