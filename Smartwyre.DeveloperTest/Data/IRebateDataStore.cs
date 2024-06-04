using System.Threading.Tasks;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data;
public interface IRebateDataStore
{
    Task<Rebate> GetRebate(string rebateIdentifier);
    Task StoreCalculationResult(Rebate account, decimal rebateAmount);
}