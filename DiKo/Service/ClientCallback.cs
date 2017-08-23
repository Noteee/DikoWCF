using SharingInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DiKo.Service
{

    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    class ClientCallback : IClient
    {
        public void PlaceHolder()
        {
            throw new NotImplementedException();
        }
    }
}
