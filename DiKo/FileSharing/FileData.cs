using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Collections;
using SQLDAL;

namespace DiKo.FileSharing
{
    class FileData
    {
        private static List<FileShareHandler> fileShareList = new List<FileShareHandler>();
        DataGrid fileData;
        public FileData(DataGrid data)
        {
            this.fileData = data;
        }

        public void createDataGrid()
        {
            DataGridTextColumn filename = new DataGridTextColumn();
            filename.Header = "File Name";
            filename.Binding = new Binding("fileName");
            fileData.Columns.Add(filename);

            DataGridTextColumn fileex = new DataGridTextColumn();
            fileex.Header = "File Extension";
            fileex.Binding = new Binding("fileEx");
            fileData.Columns.Add(fileex);

            DataGridTextColumn filepath = new DataGridTextColumn();
            filepath.Header = "File Path";
            filepath.Binding = new Binding("filePath");
            fileData.Columns.Add(filepath);

            DataGridTextColumn filesize = new DataGridTextColumn();
            filesize.Header = "File Size";
            filesize.Binding = new Binding("fileSize");
            fileData.Columns.Add(filesize);

            fileData.IsReadOnly = true;
            fileData.MouseDoubleClick += DeleteContent;
         
        }

        private void DeleteContent(object sender, MouseButtonEventArgs e)
        {

            fileData.Items.Remove(fileData.SelectedItem);
        
            
         ;
        }
      
    }
}
