using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Services
{
    public class ServerService : IServerService
    {
        public bool Login(byte[] username, byte[] password)
        {
            return DatabaseFacade.Instance.Verify(username, password);
        }
    }
}
