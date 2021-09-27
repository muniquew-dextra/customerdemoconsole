using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleDI.Data
{
    public interface IBaseRepository
    {
        Task<T> QuerySingleOrDefault<T>(string connectionString, string query, object parameters = null);
        Task<IEnumerable<T>> QueryAll<T>(string connectionString, string query, object parameters = null);
        Task<bool> Execute(string connectionString, string query, object parameters = null);
    }
}
