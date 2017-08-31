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
        private static bool programFirstStart = true;
        private static List<FileShareHandler> myFileShareList = SharedFileBrowsing.Browser.GetMySharedFiles();
        private object dummyNode = null;
        private static List<FileShareHandler> myWishList = SharedFileBrowsing.Browser.GetWishListData();
        private DataGrid dataGrid;
        private static DataGrid wishlistGrid;
        private static DataGrid mySharedGrid;
        private static DataGrid currentDatagrid;
        private static DataGrid datagridInUse;
        public static bool itemsShared = false;
        private TreeView tree;
        private string path;
        public static List<FileShareHandler> currentFileList = new List<FileShareHandler>();

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
            tree.MouseDoubleClick += new MouseButtonEventHandler((senderx, ex) => AddingToShared(senderx, ex, dataGrid, tree.SelectedItem,currentFileList));

        }


        public static void SetMyShareListAsCurrent()
        {
          
           
                datagridInUse = mySharedGrid;
                       
                currentFileList = myFileShareList;
         
       
        }
        public static void SetWishListAsCurrent()
        {

            currentFileList = myWishList;
 
            
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

        private void AddingToShared(object sender, MouseButtonEventArgs e, DataGrid dataGrid, object selected,List<FileShareHandler> list)
        {
            FileAttributes attr = File.GetAttributes(@"" + path);
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
            }
            if (list != null && programFirstStart == true)
            {
                FillDataGrid(list,dataGrid);
            }
            if(list!= null && programFirstStart == false)
            {
                try
                {   FileInfo file = new FileInfo(path);
                    if (CheckItemInList(list, new FileShareHandler(file.Name.Substring(0, file.Name.Length - 4), file.Extension.Substring(1, file.Extension.Length - 1), getSize(file.Length), file.FullName)) == true)
                    {
                        MessageBox.Show("Item already in list");
                    }
                    else if (CheckItemInList(list, new FileShareHandler(file.Name.Substring(0, file.Name.Length - 4), file.Extension.Substring(1, file.Extension.Length - 1), getSize(file.Length), file.FullName)) == false)
                    {
                        AddToWishList(new FileShareHandler(file.Name.Substring(0, file.Name.Length - 4), file.Extension.Substring(1, file.Extension.Length - 1), getSize(file.Length), file.FullName));
                        dataGrid.Items.Add(new DataItem { fileName = file.Name.Substring(0, file.Name.Length - 4), fileEx = file.Extension.Substring(1, file.Extension.Length - 1), fileSize = getSize(file.Length), filePath = file.FullName });
                        WriteSharedFileList(dataGrid);                     
                        currentDatagrid = this.dataGrid;
                    }                    
                }
                catch
                {
                    MessageBox.Show("Sorry, you can't share this content!");
                }

            }

        }
        public static void FillDataGrid(List<FileShareHandler> list, DataGrid dataGrid)
        {
            try {
                if (programFirstStart == false)
                {
                    dataGrid.Items.Clear();
                }
                if (list == null)
                {
                    list = currentFileList;
                }
                foreach (FileShareHandler fsh in list)
                {
                    Console.WriteLine(fsh.FileName);
                    dataGrid.Items.Add(new DataItem { fileName = fsh.FileName, fileEx = fsh.FileExtension, fileSize = fsh.FileSize, filePath = fsh.FilePath });
                    WriteSharedFileList(dataGrid);
                    currentDatagrid = dataGrid;
                    programFirstStart = false;
                }


            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public static void FillWishListGrid(List<FileShareHandler> list, DataGrid dataGrid)
        {
            dataGrid.Items.Clear();
            foreach (FileShareHandler fsh in list)
            {
                dataGrid.Items.Add(new DataItem { fileName = fsh.FileName, fileEx = fsh.FileExtension, fileSize = fsh.FileSize, filePath = fsh.FilePath });
                currentDatagrid = dataGrid;
                programFirstStart = false;
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
            myFileShareList = mySharedFiles;
            currentFileList = mySharedFiles;

        }

        public static void WriteWishList(DataGrid datagrid)
        {
            List<FileShareHandler> mySharedFiles = new List<FileShareHandler>();
            foreach (DataItem dI in datagrid.Items)
            {
                mySharedFiles.Add(new FileShareHandler(dI.fileName, dI.fileEx, dI.filePath, dI.fileSize));
                Console.WriteLine(dI.fileName);
            }
            myWishList = mySharedFiles;
            currentFileList = mySharedFiles;

        }


        public static List<FileShareHandler> GetSharedFileListByDataGrid(DataGrid datagrid)
        {
            List<FileShareHandler> mySharedFiles = new List<FileShareHandler>();

            foreach (DataItem dI in datagrid.Items)
            {
                mySharedFiles.Add(new FileShareHandler(dI.fileName, dI.fileEx, dI.filePath, dI.fileSize));
            }
            myFileShareList = mySharedFiles;
            currentFileList = mySharedFiles;
            return mySharedFiles;
        }

      
        public static void AddToWishList(FileShareHandler wishFile)
        {
            if (CheckItemInList(myWishList,wishFile) == true)
            {
                MessageBox.Show("Item already in list");
            }
            else if (CheckItemInList(myWishList, wishFile) == false)
            {
                myWishList.Add(wishFile);
            }
        }
        public static void DeleteFromWishList(FileShareHandler wishfile)
        {
            for(int i = 0; i < myWishList.Count(); i++)
            {
                if (myWishList[i].FileName.Equals(wishfile.FileName) && myWishList[i].FileSize.Equals(wishfile.FileSize) && myWishList[i].FilePath.Equals(wishfile.FilePath) && myWishList[i].FileExtension.Equals(wishfile.FileExtension));
                {
                    myWishList.RemoveAt(i);
                }
            }
        }
   

        public static Boolean CheckItemInList(List<FileShareHandler> fshList,FileShareHandler fsh)
        {
            Boolean returnValue = false;
            Console.WriteLine("Ezt keresed : " + fsh.FileName);
            foreach(FileShareHandler fshI in fshList)
            {
                Console.WriteLine(fshI.FileName);
                if (fshI.FileName.ToString().ToLower().Equals(fsh.FileName.ToString().ToLower()))
                {
                    returnValue = true;
                }
            }
            Console.WriteLine(returnValue);
            return returnValue;
            
        }
        
       
        
    }
}
