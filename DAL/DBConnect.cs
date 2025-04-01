using Microsoft.Data.SqlClient;
using System.Data;

namespace DAL
{
    public class DBConnect
    {

            private static readonly string connectionString = @"Data Source=RUDY\MSSQL;Initial Catalog=QLVT;Integrated Security=True;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            public static SqlConnection GetConnection()
            {
                return new SqlConnection(connectionString);
            }
    }

}
