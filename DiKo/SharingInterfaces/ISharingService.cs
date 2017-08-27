using SQLDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SharingInterfaces
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISharingService" in both code and config file together.
    [ServiceContract(CallbackContract = typeof(IClient))]
    public interface ISharingService
    {
        /*[OperationContract]

        string hello();*/
        [OperationContract]
        int Login(List<string> uri);
        [OperationContract]
        void getTables(string machineName, List<FileShareHandler> files);
        [OperationContract]
        void Logout(List<string> uri);

    }

    
}
