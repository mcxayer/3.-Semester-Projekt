using System;
using System.Linq;
using System.Text;
using Types;

namespace GeneralServices
{
    public class DatabaseFacade
    {
        public void CreateAccount(string username, byte[] passwordHash, string email, byte[] salt)
        {
            if(string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("username can not be null or empty!");
            }

            if (passwordHash == null)
            {
                throw new ArgumentNullException("passwordHash");
            }

            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("email can not be null or empty!");
            }

            // Tjek længden af byte arrays, hvis begrænsning sættes på i databasen.

            using (DataModelContainer db = new DataModelContainer())
            {
                Account account = new Account()
                {
                    Username = username,
                    Password = passwordHash,
                    Email = email,
                    Salt = salt
                };

                db.AccountSet.Add(account);
                db.SaveChanges();
            }
        }

        public bool Verify(string username, byte[] passwordHash)
        {
            // https://rmanimaran.wordpress.com/2010/06/24/creating-and-using-c-web-service-over-https-%E2%80%93-ssl-2/
            // https://msdn.microsoft.com/en-us/library/vstudio/bfsktky3(v=vs.100).aspx

            if(string.IsNullOrEmpty(username) || passwordHash == null)
            {
                return false;
            }

            using (DataModelContainer db = new DataModelContainer())
            {
                var accounts = from a in db.AccountSet
                               where a.Username.Equals(username) && a.Password.Equals(passwordHash)
                               select a;

                if(accounts.Count() == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public string GetSalt(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("username can not be null or empty!");
            }

            using (DataModelContainer db = new DataModelContainer())
            {
                Account account = (from a in db.AccountSet
                               where a.Username.Equals(username)
                               select a).FirstOrDefault();

                if (account == null)
                {
                    throw new NullReferenceException("username not valid!");
                }

                return Encoding.UTF8.GetString(account.Salt);
            }
        }
    }
}