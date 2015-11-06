using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IdentityModel.Tokens;

namespace Service
{
    [ServiceContract]
    public interface IWcfService
    {
        [OperationContract]
        String Login(String username, String password);

        [OperationContract]
        bool Logout(String tokenId);
    }
}