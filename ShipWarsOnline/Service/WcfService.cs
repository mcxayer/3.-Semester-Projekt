using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "WcfService" in both code and config file together.
    public class WcfService : IWcfService
    {
        private int c;
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }


        public int DoMath(int a, int b)
        {
            c = a + b;
            return c;
        }
    }
}
