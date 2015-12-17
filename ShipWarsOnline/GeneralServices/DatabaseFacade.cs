﻿using System;
using System.Linq;
using System.Text;
using Types;

namespace GeneralServices
{
    public class DatabaseFacade
    {
        public void CreateAccount(string username, byte[] saltedPasswordHash, string email, byte[] salt)
        {
            if(string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("username can not be null or empty!");
            }

            if (saltedPasswordHash == null)
            {
                throw new ArgumentNullException("passwordHash");
            }

            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("email can not be null or empty!");
            }

            using (DataModelContainer db = new DataModelContainer())
            {
                Account account = new Account()
                {
                    Username = username,
                    Password = saltedPasswordHash,
                    Email = email,
                    Salt = salt
                };

                db.AccountSet.Add(account);
                db.SaveChanges();
            }
        }

        public bool VerifyUser(string username, byte[] saltedPasswordHash)
        {
            if(string.IsNullOrEmpty(username) || saltedPasswordHash == null)
            {
                return false;
            }

            using (DataModelContainer db = new DataModelContainer())
            {
                var accounts = from a in db.AccountSet
                               where a.Username.Equals(username) && a.Password.Equals(saltedPasswordHash)
                               select a;

                if(accounts.Count() == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public string GetUserSalt(string username)
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