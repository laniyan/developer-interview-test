using System.Threading.Tasks;
using FluentValidation;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services;

public class RebateService(
    IValidator<CalculateRebateRequest> validator,
    IRebateDataStore rebateDataStore,
    IProductDataStore productDataStore,
    IRebateCalculatorFactory rebateCalculatorFactory
    ) : IRebateService
{
    private readonly IValidator<CalculateRebateRequest> validator = validator;
    private readonly IRebateDataStore rebateDataStore = rebateDataStore;
    private readonly IProductDataStore productDataStore = productDataStore;
    private readonly IRebateCalculatorFactory rebateCalculatorFactory = rebateCalculatorFactory;

    public async Task<CalculateRebateResult> Calculate(CalculateRebateRequest request)
    {
        var valResult = await validator.ValidateAsync(request);
        if (!valResult.IsValid)
            return Fail();

        var (productId, rebateId) = request;
        var (product, rebate) = await GetProductAndRebate(productId, rebateId);

        var calculator = rebateCalculatorFactory.Create(rebate.Incentive);
        if (!calculator.CanCalculate(rebate, product, request))
            return Fail();

        var rebateAmount = await calculator.Calculate(rebate, product, request);
        await rebateDataStore.StoreCalculationResult(rebate, rebateAmount);

        return Success();
    }

    private async Task<(Product, Rebate)> GetProductAndRebate(string productId, string rebateId)
    {
        Product product = await productDataStore.GetProduct(productId);
        Rebate rebate = await rebateDataStore.GetRebate(rebateId);

        return (product, rebate);
    }

    private static CalculateRebateResult Success() => new(true);
    private static CalculateRebateResult Fail() => new(false);
}
