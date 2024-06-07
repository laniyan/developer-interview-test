using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data;

public interface IRebateDataStore
{
    Rebate GetRebate(string rebateIdentifier);
    void StoreCalculationResult(Rebate account, decimal rebateAmount);

    // Add this method to the interface to set the Rebate for testing purposes using CLI
    void StoreRebate(Rebate r);
}