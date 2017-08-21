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
        private string variable = "DiKoShared";
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

        public void SetEnvinronmentalVariable(string newPath)
        {
            Environment.SetEnvironmentVariable(variable,newPath, EnvironmentVariableTarget.User);
        }

        public string GetEnvironmentalVariable()
        {
            if (Environment.GetEnvironmentVariable(variable, EnvironmentVariableTarget.User) == null)
            {
                SetEnvinronmentalVariable(path);
                return Environment.GetEnvironmentVariable(variable, EnvironmentVariableTarget.User);
            }
            return Environment.GetEnvironmentVariable(variable, EnvironmentVariableTarget.User);
        }
    }
}
