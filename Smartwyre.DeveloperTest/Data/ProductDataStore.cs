using System.Collections.Generic;
using System.Threading.Tasks;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data;

public class ProductDataStore : IProductDataStore
{
    public async Task<Product> GetProduct(string productIdentifier)
        => await Task.FromResult(dummyProducts.Find(p => p.Identifier == productIdentifier));

    private static List<Product> dummyProducts =
    [
        new() { Id = 1, Identifier = "100", Price = 35.50m, SupportedIncentives = SupportedIncentiveType.FixedRateRebate, Uom = "Kg" },
        new() { Id = 2, Identifier = "101", Price = 25.50m, SupportedIncentives = SupportedIncentiveType.FixedCashAmount, Uom = "Kg" },
        new() { Id = 3, Identifier = "102", Price = 15.50m, SupportedIncentives = SupportedIncentiveType.AmountPerUom, Uom = "Kg" },
        new() { Id = 4, Identifier = "103", Price = 5.50m, SupportedIncentives = SupportedIncentiveType.FixedRateRebate & SupportedIncentiveType.FixedCashAmount, Uom = "Kg" },
    ];
}
