using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Types;

namespace Service
{
    public class DatabaseFacade
    {
        private RNGCryptoServiceProvider rngCSP;

        public void CreateAccount(string usernameHash, string passwordHash, string email, string salt)
        {
            if(string.IsNullOrEmpty(usernameHash))
            {
                throw new ArgumentException("usernameHash can not be null or empty!");
            }

            if (string.IsNullOrEmpty(passwordHash))
            {
                throw new ArgumentException("passwordHash can not be null or empty!");
            }

            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("email can not be null or empty!");
            }

            using (DataModelContainer db = new DataModelContainer())
            {
                Account account = new Account()
                {
                    Username = usernameHash,
                    Password = passwordHash,
                    Email = email,
                    Salt = salt
                };

                db.AccountSet.Add(account);
                db.SaveChanges();
            }
        }

        public bool Verify(String usernameHash, String passwordHash)
        {
            // https://rmanimaran.wordpress.com/2010/06/24/creating-and-using-c-web-service-over-https-%E2%80%93-ssl-2/
            // https://msdn.microsoft.com/en-us/library/vstudio/bfsktky3(v=vs.100).aspx

            if(String.IsNullOrEmpty(usernameHash) || String.IsNullOrEmpty(passwordHash))
            {
                return false;
            }

            using (DataModelContainer db = new DataModelContainer())
            {
                var accounts = from a in db.AccountSet
                               where a.Username.Equals(usernameHash) && a.Password.Equals(passwordHash)
                               select a;

                if(accounts.Count() == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public string GetSalt(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("email can not be null or empty!");
            }

            using (DataModelContainer db = new DataModelContainer())
            {
                Account account = (from a in db.AccountSet
                               where a.Email.Equals(email)
                               select a).FirstOrDefault();

                if (account == null)
                {
                    throw new NullReferenceException("email not valid!");
                }

                return account.Salt;
            }
        }
    }
}