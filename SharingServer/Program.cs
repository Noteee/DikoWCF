using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Discovery;
using System.ServiceModel.Description;
using System.Runtime.InteropServices;

namespace SharingServer
{
    class Program
    {
        public static SharingService _server = new SharingService();

        // creating host 
        static void Main(string[] args)
        {

              _server.hostOpen();

        }

       
    }
}
