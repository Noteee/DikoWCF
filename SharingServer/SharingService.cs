using SharingInterfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SQLDAL;

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
            foreach (var client in _connectedClients)
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

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Client login: {0} @ {1}", newClient.MachineName, DateTime.UtcNow);
            Console.ResetColor();

            return 0;
        }

        public void getTables(string machineName, List<FileShareHandler> files)
        {
            foreach (var client in _connectedClients)
            {
                if (client.Key.ToLower() != machineName.ToLower())
                {
                    client.Value.connection.getDataBaseTables(files);
                }
            }
        }
        public ConnectedClient connectedClients()
        {
            var establisedUserConnection = OperationContext.Current.GetCallbackChannel<IClient>();
            foreach(var client in _connectedClients)
            {
                if (client.Value.connection == establisedUserConnection)
                {
                    return client.Value;
                }
            }
            return null;
        }

        public void Logout()
        {
            ConnectedClient client = connectedClients();
            if (client != null)
            {
                ConnectedClient removedClient;
                _connectedClients.TryRemove(client.MachineName, out removedClient);

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Client logoff: {0} @ {1}", removedClient.MachineName, DateTime.UtcNow);
                Console.ResetColor();
            }
        }

        public int Clients()
        {
            return _connectedClients.Count;
        }
    }
}
