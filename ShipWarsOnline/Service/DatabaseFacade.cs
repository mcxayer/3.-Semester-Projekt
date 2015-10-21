using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Service
{
    public class DatabaseFacade
    {
        public static readonly DatabaseFacade Instance = new DatabaseFacade();

        private static String connectionString = "user id=group_4;" +
                                       "password=EC2yRDFn;server=tek-mmmi-db0a.tek.c.sdu.dk:5432;" +
                                       "Trusted_Connection=yes;" +
                                       "database=group_4_db; " +
                                       "connection timeout=30";

        private DatabaseFacade()
        {
        }

        // Kun oprettet for at teste systemet. Vil formentlig fjernes i fremtiden.
        public void AddUser(String usernameHash, String passwordHash)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                if (connection.State != System.Data.ConnectionState.Open)
                {
                    return;
                }

                // Parametre tilføjes for at undgå SQL-injection!
                using (SqlCommand insertCommand = connection.CreateCommand())
                {
                    insertCommand.CommandText = "INSERT INTO users VALUES (@username,@password)";
                    insertCommand.Parameters.Add(new SqlParameter("@username", usernameHash));
                    insertCommand.Parameters.Add(new SqlParameter("@password", passwordHash));
                    insertCommand.ExecuteNonQuery();
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

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                }
                catch(SqlException ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                if(connection.State != System.Data.ConnectionState.Open)
                {
                    return false;
                }

                // http://www.codeproject.com/Articles/4416/Beginners-guide-to-accessing-SQL-Server-through-C

                using (SqlCommand fetchCommand = connection.CreateCommand())
                {
                    fetchCommand.CommandText = "SELECT password FROM users WHERE @username = username";
                    fetchCommand.Parameters.Add(new SqlParameter("@username", usernameHash));
                    SqlDataReader reader = fetchCommand.ExecuteReader();

                    if (!reader.GetString(0).Equals(usernameHash))
                    {
                        return false;
                    }

                    if (!reader.GetString(1).Equals(passwordHash))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}