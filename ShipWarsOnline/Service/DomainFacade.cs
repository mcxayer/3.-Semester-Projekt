using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class DomainFacade
    {
        public static readonly DomainFacade Instance = new DomainFacade();

        public DatabaseFacade DatabaseAccess { get; private set; }

        private DomainFacade()
        {
            DatabaseAccess = new DatabaseFacade("user id=postgres;" +
                                    "password=test1234;" +
                                    "server=localhost;" +
                                    "database=ShipWarsOnlineTestDatabase;");
        }
    }
}
