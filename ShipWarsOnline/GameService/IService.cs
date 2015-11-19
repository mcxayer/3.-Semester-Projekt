using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GameService
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        [FaultContract(typeof(FaultException))]
        void DoStuff();
    }
}
