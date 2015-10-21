using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Service
{
    [ServiceContract]
    public interface IServerService
    {
        [OperationContract]
        bool Login(byte[] username, byte[] password);
    }
}
