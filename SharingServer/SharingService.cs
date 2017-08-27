﻿using SharingInterfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SQLDAL;
using System.ServiceModel.Discovery;
using System.ServiceModel.Description;
using Tulpep.NotificationWindow;

namespace SharingServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
    public class SharingService : ISharingService
    {
        public const string magicString = "djeut73bch58sb4"; // this is random, just to see if you get the right result
        public string Ping() { return magicString; }
        /*public string hello()
        {
            return "Hello";
        }*/

        public ConcurrentDictionary<string, ConnectedClient> _connectedClients = new ConcurrentDictionary<string, ConnectedClient>();

        public void hostOpen()
        {
            string hostname = System.Environment.MachineName;
            var baseAddress = new UriBuilder("http", hostname, 9001, "SharingService");
            var h = new ServiceHost(typeof(SharingService), baseAddress.Uri);

            // enable processing of discovery messages.  use UdpDiscoveryEndpoint to enable listening. use EndpointDiscoveryBehavior for fine control.
            h.Description.Behaviors.Add(new ServiceDiscoveryBehavior());
            h.AddServiceEndpoint(new UdpDiscoveryEndpoint());

            // enable wsdl, so you can use the service from WcfStorm, or other tools.
            var smb = new ServiceMetadataBehavior();
            smb.HttpGetEnabled = true;
            smb.HttpsGetEnabled = false;
            smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
            h.Description.Behaviors.Add(smb);

            // create endpoint
            var binding = new WSDualHttpBinding(WSDualHttpSecurityMode.None);
            h.AddServiceEndpoint(typeof(ISharingService), binding, "");
            h.Open();
            
        }

        public int Login(List<string> uris)
        {

            foreach(string uri in uris)
            {
                if (_connectedClients.ContainsKey(Environment.MachineName))
                {
                    return 1;
                }
                else
                {
                    ConnectedClient newClient = new ConnectedClient();
                    Uri getUri = new Uri(uri);
                    newClient.MachineName = getUri.Host;
                    _connectedClients.TryAdd(getUri.Host, newClient);
                    if (newClient.MachineName != Environment.MachineName.ToLower())
                    {
                        PopupNotifier popup = new PopupNotifier();
                        popup.TitleText = "Notification";
                        popup.ContentText = ("Client login: " + newClient.MachineName + " @ " + DateTime.UtcNow);
                        popup.ContentColor = System.Drawing.Color.Blue;
                        popup.Popup();
                    }


                }
            }
                        
            
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
           // var establisedUserConnection = OperationContext.Current.GetCallbackChannel<IClient>();
           // foreach(var client in _connectedClients)
           // {
           //     if (client.Value.connection == establisedUserConnection)
                {
           //         return client.Value;
                }
           // }
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
