using SharingInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using SQLDAL;
using System.Windows;
using System.Windows.Controls;

namespace DiKo.Service
{
    //clients callback
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    class ClientCallback : IClient
    {
        
        public void getDataBaseTables(List<FileShareHandler> files)
        {
            ((MenuWindow)Application.Current.MainWindow).fillSharedFiles(files);
        }

    }
}
