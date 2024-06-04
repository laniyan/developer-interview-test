using FluentValidation;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Validation;
public static class IRuleBuilderOptionsExtensions
{
    public static IRuleBuilderOptions<CalculateRebateRequest, string> ProductMustExist(
        this IRuleBuilder<CalculateRebateRequest, string> ruleBuilder,
        IProductDataStore productDataStore)
            => ruleBuilder.SetAsyncValidator(new ProductIdentifierExistsValidator(productDataStore));

    public static IRuleBuilderOptions<CalculateRebateRequest, string> RebateMustExist(
        this IRuleBuilder<CalculateRebateRequest, string> ruleBuilder,
        IRebateDataStore rebateDataStore)
            => ruleBuilder.SetAsyncValidator(new RebateIdentifierExistsValidator(rebateDataStore));
}
