using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Services
{
    public class DatabaseFacade
    {
        public static readonly DatabaseFacade Instance = new DatabaseFacade();

        private DatabaseFacade() { }

        public bool Verify(byte[] username, byte[] password)
        {
            // https://rmanimaran.wordpress.com/2010/06/24/creating-and-using-c-web-service-over-https-%E2%80%93-ssl-2/
            // https://msdn.microsoft.com/en-us/library/vstudio/bfsktky3(v=vs.100).aspx
            return true;
        }
    }
}