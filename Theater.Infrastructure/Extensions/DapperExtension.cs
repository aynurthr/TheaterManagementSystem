using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Theater.Infrastructure.Extensions
{
    public static partial class Extension
    {
        public static Task<IEnumerable<T>> QueryAsync<T>(this DbContext db, string query, object param = null, CommandType commandType = CommandType.StoredProcedure)
            where T : class
        {
            return db.Database.GetDbConnection().QueryAsync<T>(query, commandType: commandType, param: param);
        }

        public static Task<T?> QueryFirstOrDefaultAsync<T>(this DbContext db, string query, object param = null, CommandType commandType = CommandType.StoredProcedure)
        where T : class
        {
            return db.Database.GetDbConnection().QueryFirstOrDefaultAsync<T>(query, commandType: commandType, param: param);
        }
    }
}
