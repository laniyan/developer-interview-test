using FluentValidation;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Validation;

public class CalculateRebateRequestValidator : AbstractValidator<CalculateRebateRequest>
{
    public CalculateRebateRequestValidator(IProductDataStore productDataStore, IRebateDataStore rebaseDataStore)
    {
        RuleFor(x => x.RebateIdentifier)
            .NotEmpty().WithMessage("RebateIdentifier is required.")
            .RebateMustExist(rebaseDataStore).WithMessage("Rebaste not found.");

        RuleFor(x => x.ProductIdentifier)
            .NotEmpty().WithMessage("ProductIdentifier is required.")
            .ProductMustExist(productDataStore).WithMessage("Product not found.");

        RuleFor(x => x.Volume)
            .GreaterThan(0).WithMessage("Invalid volume amount.");
    }
}
