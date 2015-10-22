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
        string GetData(int value);

        [OperationContract]
        int DoMath(String tokenID, int a, int b);

        [OperationContract]
        String Login(String username, String password);
    }
}