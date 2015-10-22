using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Service
{
    public class DatabaseFacade
    {
        public static readonly DatabaseFacade Instance = new DatabaseFacade();

        private static String connectionString = "user id=group_4;" +
                                       "password=EC2yRDFn;" +
                                       "server=tek-mmmi-db0a.tek.c.sdu.dk:5432;" +
                                       "Trusted_Connection=yes;" +
                                       "database=group_4_db; " +
                                       "connection timeout=30";

        private static String testConnectionString = "user id=postgres;" +
                                                     "password=test1234;" +
                                                     "server=localhost;" +
                                                     "database=ShipWarsOnlineTestDatabase; ";

        private DatabaseFacade()
        {
        }

        // Kun oprettet for at teste systemet. Vil formentlig fjernes i fremtiden.
        public void AddUser(String usernameHash, String passwordHash)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    //Parametre tilføjes for at undgå SQL-injection!
                    using (NpgsqlCommand insertCommand = new NpgsqlCommand())
                    {
                        insertCommand.CommandText = "INSERT INTO users VALUES (@username,@password)";
                        insertCommand.Connection = connection;
                        insertCommand.Parameters.Add(new NpgsqlParameter("@username", usernameHash));
                        insertCommand.Parameters.Add(new NpgsqlParameter("@password", passwordHash));
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.ToString());
                }
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

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // http://www.codeproject.com/Articles/4416/Beginners-guide-to-accessing-SQL-Server-through-C

                    using (NpgsqlCommand fetchCommand = new NpgsqlCommand())
                    {
                        fetchCommand.CommandText = "SELECT password FROM users WHERE @username = username";
                        fetchCommand.Connection = connection;
                        fetchCommand.Parameters.Add(new NpgsqlParameter("@username", usernameHash));
                        NpgsqlDataReader reader = fetchCommand.ExecuteReader();

                        if (reader.HasRows && !reader.GetString(0).Equals(passwordHash))
                        {
                            return false;
                        }
                    }
                }
                catch(SqlException ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            return true;
        }

        public static String GetHashedString(HashAlgorithm hashAlgorithm, String s)
        {
            byte[] sHash = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(s));

            StringBuilder hashSB = new StringBuilder();
            for (int i = 0; i < sHash.Length; i++)
            {
                hashSB.Append(sHash[i]);
            }

            return hashSB.ToString();
        }
    }
}