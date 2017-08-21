using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ookii.Dialogs.Wpf;

namespace DiKo.FileSharing
{
    class getDownloadPath
    {
        private string path = @"C:\Shared Files";
        public void getDownloadFolder()
        {
            var dialog = new VistaFolderBrowserDialog();
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                path =  dialog.SelectedPath;
            }
            
        }

        public string getPath()
        {
            return path;
        }
    }
}
