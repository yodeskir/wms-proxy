namespace WMSAuthentication.Interfaces
{
    public interface IWMSRuleFactory
    {
        IWMSRule Create<T>() where T : IWMSRule, new();
    }
}