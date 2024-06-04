using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data;

public class RebateDataStore : IRebateDataStore
{
    public async Task<Rebate> GetRebate(string rebateIdentifier)
        => await Task.FromResult(dummyRebates.Find(r => r.Identifier == rebateIdentifier));

    public async Task StoreCalculationResult(Rebate account, decimal rebateAmount)
        => Console.WriteLine($"Rebate amount of {rebateAmount} stored to db for rebate {account.Identifier}");

    private static List<Rebate> dummyRebates =
    [
        new() { Identifier = "200", Incentive = IncentiveType.FixedRateRebate, Percentage = 0.1m },
        new() { Identifier = "201", Incentive = IncentiveType.FixedCashAmount, Amount = 1.00m },
        new() { Identifier = "202", Incentive = IncentiveType.AmountPerUom, Amount = 0.5m },
        new() { Identifier = "203", Incentive = IncentiveType.FixedRateRebate, Percentage = 0.15m },
    ];
}
