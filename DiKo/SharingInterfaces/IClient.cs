using SQLDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SharingInterfaces
{
    //methods that clienta get back
    public interface IClient
    {
        [OperationContract]
        void getDataBaseTables(List<FileShareHandler> files);
    }
}
