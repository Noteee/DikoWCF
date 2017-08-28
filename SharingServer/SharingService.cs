using SharingInterfaces;
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
using System.Drawing;

namespace SharingServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
    public class SharingService : ISharingService
    {
       public ConcurrentDictionary<string, ConnectedClient> _connectedClients = new ConcurrentDictionary<string, ConnectedClient>();

        // opening the host
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

        // happens automaticly, when discovery finds a new channel
        public void Login(List<string> uris)
        {

            foreach(string uri in uris)

            {
                Uri getUri = new Uri(uri);
                if (!_connectedClients.ContainsKey(getUri.Host))
                {
                    ConnectedClient newClient = new ConnectedClient();

                    newClient.MachineName = getUri.Host;

                    _connectedClients.TryAdd(getUri.Host, newClient);
                    if (newClient.MachineName != Environment.MachineName.ToLower())
                    {
                        PopupNotifier popup = new PopupNotifier();
                        popup.TitleText = "Notification";
                        popup.ContentText = ("Client is online: " + newClient.MachineName + " @ " + DateTime.UtcNow);
                        popup.ContentColor = System.Drawing.Color.Blue;
                        popup.ContentFont = new Font("Tahoma", 12);
                        popup.Popup();
                    }
                }
          
            }

        }
        // this will return to clients the files list from database
        public void getTables(string machineName, List<FileShareHandler> files) { 
        /*{
            foreach (var client in _connectedClients)
            {
                if (client.Key.ToLower() != machineName.ToLower())
                {
                    client.Value.connection.getDataBaseTables(files);
                }
            }*/
        }
        
        public ConnectedClient connectedClients()
        {
           
             foreach(var client in _connectedClients)
            {
                if (client.Value.connection == null)
                {
                    return client.Value;
                }
            }
            return null;
        }
        //log off, that discovery can't find you in the network
        public void Logout(List<string> uris)
        {
            List<string> getUris = new List<string>();
            foreach (string uri in uris)

            {
                Uri getUri = new Uri(uri);
                getUris.Add(getUri.Host);
            }
            foreach (var client in _connectedClients)
            {
                if (!getUris.Contains(client.Key))
                {
                    ConnectedClient removedClient;
                    _connectedClients.TryRemove(client.Key, out removedClient);
                    if (client.Key != Environment.MachineName.ToLower())
                    {
                        PopupNotifier popup = new PopupNotifier();
                        popup.TitleText = "Notification";
                        popup.ContentText = ("Client is offline: " + client.Key + " @ " + DateTime.UtcNow);
                        popup.ContentColor = System.Drawing.Color.Magenta;
                        popup.ContentFont = new Font("Tahoma", 12);
                        popup.Popup();
                    }
                }

            }

        }
    }
}
