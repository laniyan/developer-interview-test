using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data;

public class RebateDataStore : IRebateDataStore
{
    private Rebate Rebate { get; set; }
    public Rebate GetRebate(string rebateIdentifier)
    {
        // Access database to retrieve account, code removed for brevity 
        return Rebate;
    }

    public void StoreCalculationResult(Rebate account, decimal rebateAmount)
    {
        account.Amount = rebateAmount;
        // Update account in database, code removed for brevity
    }

    public void StoreRebate(Rebate r)
    {
        Rebate = r;
    }
}