using SharingInterfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SharingServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
    public class SharingService : ISharingService
    {
        /*public string hello()
        {
            return "Hello";
        }*/

        public ConcurrentDictionary<string, ConnectedClient> _connectedClients = new ConcurrentDictionary<string, ConnectedClient>();

        public int Login(string machineName)
        {
            foreach(var client in _connectedClients)
            {
                if (client.Key.ToLower() == machineName.ToLower())
                {
                    return 1;
                }
            }
            var establisedUserConnection = OperationContext.Current.GetCallbackChannel<IClient>();

            ConnectedClient newClient = new ConnectedClient();
            newClient.connection = establisedUserConnection;
            newClient.MachineName = machineName;

            _connectedClients.TryAdd(machineName, newClient);

            return 0;
        }
    }
}
