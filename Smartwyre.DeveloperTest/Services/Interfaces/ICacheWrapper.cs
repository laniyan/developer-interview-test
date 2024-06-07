namespace Smartwyre.DeveloperTest.Services.Interfaces
{
    public interface ICacheWrapper
    {
        void CacheRebateAmount(string key, decimal amount);

        decimal GetCachedRebateAmount(string key);
    }
}