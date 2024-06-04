using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Validation;
public class RebateIdentifierExistsValidator(IRebateDataStore rebateDataStore) : AsyncPropertyValidator<CalculateRebateRequest, string>
{
    private readonly IRebateDataStore rebateDataStore = rebateDataStore;

    public override string Name => "RebateIdentifierExistsValidator";

    public override async Task<bool> IsValidAsync(ValidationContext<CalculateRebateRequest> context, string value, CancellationToken cancellation)
    {
        var exists = await rebateDataStore.GetRebate(value);
        var result = exists is not null;

        return result;
    }
}
