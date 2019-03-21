namespace ProjectsSoftuni.Data
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using ProjectsSoftuni.Data.Common;

    public class DbQueryRunner : IDbQueryRunner
    {
        public DbQueryRunner(ProjectsSoftuniDbContext context)
        {
            this.Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public ProjectsSoftuniDbContext Context { get; set; }

        public Task RunQueryAsync(string query, params object[] parameters)
        {
            return this.Context.Database.ExecuteSqlCommandAsync(query, parameters);
        }

        public void Dispose()
        {
            this.Context?.Dispose();
        }
    }
}
