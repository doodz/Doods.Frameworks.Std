using Doods.Framework.Std;
using SQLite;
using System.Threading.Tasks;

namespace Doods.Framework.Repository.Std.Interfaces
{
    public interface IDatabase
    {
        bool IsInitialize { get; }

        Task<SQLiteAsyncConnection> GetConnection(ITimeWatcher timer);

        Task Reset();
    }
}
