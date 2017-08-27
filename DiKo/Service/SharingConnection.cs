using SharingInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Discovery;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DiKo.Service
{
    class SharingConnection
    {
        private ISharingService channel;
        public static List<string> urisConnected = new List<string>();

        public void addUrisToList (string  uri)
        {
            urisConnected.Add(uri);
        }

        public List<string> getUrisList()
        {
            return urisConnected;
        }

        public int countConnectedChannels()
        {
            return urisConnected.Count;
        }

        public void Sharing_DiscoverChannel()
        {
            urisConnected.Clear();
            var dc = new DiscoveryClient(new UdpDiscoveryEndpoint());
            FindCriteria fc = new FindCriteria(typeof(ISharingService));
            fc.Duration = TimeSpan.FromSeconds(5);
            FindResponse fr = dc.Find(fc);
            foreach (EndpointDiscoveryMetadata edm in fr.Endpoints)
            {
                if (!urisConnected.Contains(edm.Address.Uri.ToString()) && edm.Address.Uri.ToString().StartsWith("http"))
                {
                    addUrisToList(edm.Address.Uri.ToString());
                    
                    Console.WriteLine(edm.Address.Uri.ToString());
                }
                
            }

        }
        public void Sharing_SetupChannel()
        {

            var binding = new WSDualHttpBinding(WSDualHttpSecurityMode.None);
            var factory = new DuplexChannelFactory<ISharingService>(new ClientCallback(), binding);
            string hostname = System.Environment.MachineName;
            var uri = new UriBuilder("http", hostname, 9001, "SharingService");
            Console.WriteLine("creating channel to " + uri.ToString());
            EndpointAddress ea = new EndpointAddress(uri.Uri);
            channel = factory.CreateChannel(ea);
            Console.WriteLine("channel created");

        }

    }
}
