using SQLDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SharingInterfaces
{

    // sharing interface which implemented in: SharingServer -> SharingService
    [ServiceContract(CallbackContract = typeof(IClient))]
    public interface ISharingService
    {

        [OperationContract]
        void Login(List<string> uri);
        [OperationContract]
        void getTables(string machineName, List<FileShareHandler> files);
        [OperationContract]
        void Logout(List<string> uri);

    }

    
}
