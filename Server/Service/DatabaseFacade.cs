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
            return true;
        }
    }
}