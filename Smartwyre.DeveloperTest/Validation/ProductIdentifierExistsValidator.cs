using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Validation;

public class ProductIdentifierExistsValidator(IProductDataStore productStore) : AsyncPropertyValidator<CalculateRebateRequest, string>
{
    private readonly IProductDataStore productStore = productStore;

    public override string Name => "ProductIdentifierExistsValidator";

    public override async Task<bool> IsValidAsync(ValidationContext<CalculateRebateRequest> context, string value, CancellationToken cancellation)
    {
        var exists = await productStore.GetProduct(value);
        var result = exists is not null;

        return result;
    }
}
