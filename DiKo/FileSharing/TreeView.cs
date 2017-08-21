using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SQLDAL;

namespace DiKo.FileSharing
{

    class Treeview
    {
        private static List<FileShareHandler> myFileShareList = new List<FileShareHandler>();
        private object dummyNode = null;
        private DataGrid dataGrid;
        private static DataGrid currentDatagrid;
        private TreeView tree;
        private string path;

        public Treeview(TreeView tree, DataGrid dataGrid)
        {
            this.tree = tree;
            this.dataGrid = dataGrid;
        }

        public string SelectedImagePath { get; set; }

        public void Window_Loaded()
        {

            foreach (string s in Directory.GetLogicalDrives())
            {
                TreeViewItem item = new TreeViewItem();
                item.Header = s;
                item.Tag = s;
                item.FontWeight = FontWeights.Normal;
                item.Items.Add(dummyNode);
                item.Expanded += new RoutedEventHandler(folder_Expanded);
                tree.Items.Add(item);
            }
            tree.SelectedItemChanged += foldersItem_SelectedItemChanged;
            tree.MouseDoubleClick += new MouseButtonEventHandler((senderx, ex) => AddingToShared(senderx, ex, dataGrid, tree.SelectedItem));

        }

        public static List<FileShareHandler> getFileShareList()
        {
          
            return myFileShareList;
        }

        public void folder_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)sender;
            if (item.Items.Count == 1 && item.Items[0] == dummyNode)
            {
                item.Items.Clear();
                try
                {
                    foreach (string s in Directory.GetDirectories(item.Tag.ToString()))
                    {
                        TreeViewItem subitem = new TreeViewItem();
                        subitem.Header = s.Substring(s.LastIndexOf("\\") + 1);
                        subitem.Tag = s;
                        subitem.FontWeight = FontWeights.Normal;
                        subitem.Items.Add(dummyNode);
                        subitem.Expanded += new RoutedEventHandler(folder_Expanded);
                        item.Items.Add(subitem);
                    }
                    foreach (string s in Directory.GetFiles(item.Tag.ToString()))
                    {
                        TreeViewItem subitem = new TreeViewItem();
                        subitem.Header = s.Substring(s.LastIndexOf("\\") + 1);
                        subitem.Tag = s;
                        subitem.FontWeight = FontWeights.Normal;
                        subitem.Items.Add(dummyNode);
                        subitem.Expanded += new RoutedEventHandler(folder_Expanded);
                        item.Items.Add(subitem);



                    }
                }
                catch (Exception) { }
            }
        }

        private void AddingToShared(object sender, MouseButtonEventArgs e, DataGrid dataGrid, object selected)
        {
            // get the file attributes for file or directory
            FileAttributes attr = File.GetAttributes(@"" + path);

            //detect whether its a directory or file
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {

            }

            else
            {
                try
                {
                    FileInfo file = new FileInfo(path);
                    dataGrid.Items.Add(new DataItem { fileName = file.Name.Substring(0, file.Name.Length - 4), fileEx = file.Extension.Substring(1, file.Extension.Length - 1), filePath = file.FullName, fileSize = getSize(file.Length) });
                    /* myFileShareList.Add(new FileShareHandler(file.Name.Substring(0, file.Name.Length - 4), file.Extension.Substring(1, file.Extension.Length - 1), file.FullName, getSize(file.Length).ToString()));
                     foreach(DataItem v in dataGrid.Items)
                     {
                         Console.WriteLine(v.fileName);
                     }
                     */
                     WriteSharedFileList(dataGrid);
                     currentDatagrid = this.dataGrid;
                    
                }
                catch
                {
                    MessageBox.Show("Sorry, you can't share this content!");
                }

            }

        }

        public static DataGrid GetCurrentDataGrid()
        {
            return currentDatagrid;
        }

        private void foldersItem_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeView tree = (TreeView)sender;
            TreeViewItem temp = ((TreeViewItem)tree.SelectedItem);

            if (temp == null)
                return;
            SelectedImagePath = "";
            string temp1 = "";
            string temp2 = "";
            while (true)
            {
                temp1 = temp.Header.ToString();
                if (temp1.Contains(@"\"))
                {
                    temp2 = "";
                }
                SelectedImagePath = temp1 + temp2 + SelectedImagePath;
                if (temp.Parent.GetType().Equals(typeof(TreeView)))
                {
                    break;
                }
                temp = ((TreeViewItem)temp.Parent);
                temp2 = @"\";
            }
            //show user selected path
            //MessageBox.Show(SelectedImagePath);
            path = SelectedImagePath;
        }

        public class DataItem
        {
            public string fileName { get; set; }
            public string fileEx { get; set; }
            public string filePath { get; set; }
            public string fileSize { get; set; }

        }

        public string getSize(long size)
        {
            if (size / 1024 / 1024 / 1024 > 0 && size / 1024 / 1024 / 1024 < 1024)
            {
                return size / 1024 / 1024 / 1024 + " GB";
            }
            if (size / 1024 / 1024 > 0 && size / 1024 / 1024 < 1024)
            {
                return size / 1024 / 1024 + " MB";
            }
            if (size / 1024 > 0 && size / 1024 < 1024)
            {
                return size / 1024 + " KB";
            }

            else
            {
                return size + " Byte";
            }
        }


        private void logToConsole(List<FileShareHandler> fs)
        {
            try
            {
                foreach (var v in fs)
                {
                    Console.WriteLine(v.FileName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
       
      

        public static void WriteSharedFileList(DataGrid datagrid)
        {
            List<FileShareHandler> mySharedFiles = new List<FileShareHandler>();
            foreach (DataItem dI in datagrid.Items)
            {
                mySharedFiles.Add(new FileShareHandler(dI.fileName, dI.fileEx, dI.filePath, dI.fileSize));
                Console.WriteLine(dI.fileName);
            }
        }

        public static List<FileShareHandler> GetSharedFileList(DataGrid datagrid)
        {
            List<FileShareHandler> mySharedFiles = new List<FileShareHandler>();
            foreach (DataItem dI in datagrid.Items)
            {
                mySharedFiles.Add(new FileShareHandler(dI.fileName, dI.fileEx, dI.filePath, dI.fileSize));
                Console.WriteLine(dI.fileName);
            }
            return mySharedFiles;
        }

    }
}
