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

        public Uri Sharing_DiscoverChannel()
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
                if (edm.Address.Uri.ToString().Contains(Environment.MachineName.ToLower()))
                {
                    return edm.Address.Uri;
                }
                
            }
            // here is the really nasty part
            // i am just returning the first channel, but it may not work.
            // you have to do some logic to decide which uri to use from the discovered uris
            // for example, you may discover "127.0.0.1", but that one is obviously useless.
            // also, catch exceptions when no endpoints are found and try again.

            return fr.Endpoints[0].Address.Uri;

        }
        public void Sharing_SetupChannel()
        {
              /*_channelFactory = new DuplexChannelFactory<ISharingService>(new ClientCallback(), "FileSharingEndPoint");
            Server = _channelFactory.CreateChannel();
            */
            var binding = new WSDualHttpBinding(WSDualHttpSecurityMode.None);
            var factory = new DuplexChannelFactory<ISharingService>(new ClientCallback(), binding);
            var uri = Sharing_DiscoverChannel();
            Console.WriteLine("creating channel to " + uri.ToString());
            EndpointAddress ea = new EndpointAddress(uri);
            channel = factory.CreateChannel(ea);
            Console.WriteLine("channel created");
            //Console.WriteLine("pinging host");
            //string result = channel.Ping();
            //Console.WriteLine("ping result = " + result);
        }
        public void Sharing_Ping()
        {
            Console.WriteLine("pinging host");
            string result = channel.Ping();
            Console.WriteLine("ping result = " + result);
        }

    }
}
