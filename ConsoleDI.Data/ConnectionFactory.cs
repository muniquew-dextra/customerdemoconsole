using System.Data;
using System.Data.SqlClient;

namespace ConsoleDI.Data
{
    public class ConnectionFactory : IConnectionFactory
    {
        public IDbConnection Connection(string conectionString)
        {
            return new SqlConnection(conectionString);
        }
    }
}
