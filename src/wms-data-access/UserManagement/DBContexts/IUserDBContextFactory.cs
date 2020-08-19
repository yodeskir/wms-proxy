namespace WMSDataAccess.UserManagement.DBContexts
{
    public interface IUserDBContextFactory
    {
        UserDBContext Create();
    }
}