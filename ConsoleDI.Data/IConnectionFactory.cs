using System.Data;

namespace ConsoleDI.Data
{
    public interface IConnectionFactory
    {
        IDbConnection Connection(string connectionString);
    }
}
