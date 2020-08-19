using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;
using WMSDataAccess.UserManagement.DBContexts;
using WMSTools;

namespace WMSDataAccess
{
    public class UserDBContextFactory : IUserDBContextFactory
    {
        private static DbContextOptionsBuilder<UserDBContext> _optionsBuilder;

        public UserDBContextFactory() { }

        public UserDBContextFactory(IOptions<RuntimeOptions> runtimeOptions)
        {
            _optionsBuilder = new DbContextOptionsBuilder<UserDBContext>();
            _optionsBuilder.UseNpgsql(runtimeOptions.Value.ConnectionString);
        }
        public UserDBContext Create()
        {
            return new UserDBContext(_optionsBuilder.Options);
        }
    }
}