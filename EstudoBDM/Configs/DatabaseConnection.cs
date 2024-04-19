#pragma warning disable CS8603

using Microsoft.EntityFrameworkCore;
using EstudoBDM.Models;

namespace EstudoBDM.Configs
{
    public class DatabaseConnection(DbContextOptions<DatabaseConnection> options) : DbContext(options)
    {
        public DbSet<Employee> Employees { get; set; }

        #region Call procedure
        public async Task<T> CallProcedureSingle<T>(string procName, dynamic[] procParams)
        {
            var sql = $"CALL {procName}(";

            var totalParams = procParams.Length;

            for (int paramIndex = 0; paramIndex < totalParams; paramIndex++)
            {
                sql += "{" + paramIndex + "}";

                if (paramIndex < totalParams - 1)
                {
                    sql += ", "; 
                } else
                {
                    sql += ")";
                }
            }

            var result = await Database.SqlQueryRaw<T>(sql, procParams).ToArrayAsync();

            return result.Length > 0 ? result[0] : default;
        }

        public async Task<T> CallProcedureSingle<T>(string procName, dynamic param)
        {
            var sql = $"CALL {procName}({param})"; 

            var result = await Database.SqlQueryRaw<T>(sql).ToArrayAsync();

            return result.Length > 0 ? result[0] : default;
        }

        public async Task<T> CallProcedureSingle<T>(string procName)
        {
            var sql = $"CALL {procName}()";

            var result = await Database.SqlQueryRaw<T>(sql).ToArrayAsync();

            return result.Length > 0 ? result[0] : default;
        }

        public async Task<T[]> CallProcedureList<T>(string procName, dynamic[] procParams)
        {
            var sql = $"CALL {procName}(";

            var totalParams = procParams.Length;

            for (int paramIndex = 0; paramIndex < totalParams; paramIndex++)
            {
                sql += "{" + paramIndex + "}";

                if (paramIndex < totalParams - 1)
                {
                    sql += ", ";
                }
                else
                {
                    sql += ")";
                }
            }

            var result = await Database.SqlQueryRaw<T>(sql, procParams).ToArrayAsync();

            return result;
        }

        public async Task<T[]> CallProcedureList<T>(string procName, dynamic param)
        {
            var sql = $"CALL {procName}({param})";

            var result = await Database.SqlQueryRaw<T>(sql).ToArrayAsync();

            return result;
        }

        public async Task<T[]> CallProcedureList<T>(string procName)
        {
            var sql = $"CALL {procName}()";

            var result = await Database.SqlQueryRaw<T>(sql).ToArrayAsync();

            return result;
        }
        #endregion Call procedure
    }
}
