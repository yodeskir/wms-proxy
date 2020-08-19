using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using WMSDataAccess.UserManagement.DBContexts;

namespace wmsDataAccess
{
    public class TemporaryDbContextFactory : IDesignTimeDbContextFactory<UserDBContext>
    {
        public UserDBContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(@"C:\Repos\appgis-wms-aas\wmsproxy")
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var builder = new DbContextOptionsBuilder<UserDBContext>();

            var connectionString = configuration.GetConnectionString("UsersDatabase");

            builder.UseNpgsql(connectionString);

            // Stop client query evaluation
            //builder.ConfigureWarnings(w =>w.Throw(RelationalEventId.QueryClientEvaluationWarning));

            return new UserDBContext(builder.Options);
        }
    }
}
