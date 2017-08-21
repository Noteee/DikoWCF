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

namespace DiKo
{
    /// <summary>
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        public MenuWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            FileData datagrid = new FileData(sharedGrid);
            datagrid.createDataGrid();
            Treeview tree = new Treeview(whatToShareTreeView, sharedGrid);
            tree.Window_Loaded();
            SQLDAL.SQLDAL.ConnecToDB();
        }

        private void shareButton_Click(object sender, RoutedEventArgs e)
        {
            SQLDAL.SQLDAL.WriteListToDB(Treeview.getFileShareList());
        }
    }
}
