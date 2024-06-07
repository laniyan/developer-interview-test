using FluentValidation;
using Microsoft.Extensions.Logging;
using Smartwyre.DeveloperTest.Exceptions;
using Smartwyre.DeveloperTest.Extensions;
using Smartwyre.DeveloperTest.Services.Interfaces;
using Smartwyre.DeveloperTest.Types;
using Smartwyre.DeveloperTest.Types.Dtos;

namespace Smartwyre.DeveloperTest.Services
{
    public class AmountPerUomCalculator : IRebateCalculator
    {
        private readonly IValidator<CalculateAmountPerUomRebate> _validator;
        private readonly ILogger<AmountPerUomCalculator> _log;

        public AmountPerUomCalculator(IValidator<CalculateAmountPerUomRebate> validator, ILogger<AmountPerUomCalculator> log)
        {
            _validator = validator;
            _log = log;
        }
        public decimal CalculateRebate(Rebate rebate, Product product, CalculateRebateRequest request)
        {
            _log.LogInformation("Calculating rebate for RebateId: {RebateId}, ProductId: {ProductId}",
            request.RebateIdentifier, request.ProductIdentifier);

            EnsureValidRebateData(rebate, product, request);

            var rebateAmount = rebate.Amount * request.Volume;
            _log.LogInformation("Rebate calculated successfully: {Amount}", rebateAmount);

            return rebateAmount;
        }

        private void EnsureValidRebateData(Rebate rebate, Product product, CalculateRebateRequest request)
        {
            _log.LogInformation("Validating rebate data for RebateId: {RebateId}, ProductId: {ProductId}",
            request.RebateIdentifier, request.ProductIdentifier);
            var calculate = new CalculateAmountPerUomRebate();

            var validationResult = _validator.Validate(calculate.AsCalculateAmountPerUomRebateDto(rebate, product, request));
            if (!validationResult.IsValid)
            {
                _log.LogError("Rebate data validation failed: {ValidationErrors}", validationResult.ToString());
                throw new CalculateIncentiveRebateException("Invalid Input", validationResult.ToString());
            }

            _log.LogInformation("Rebate data validation successful.");
        }
    }
}