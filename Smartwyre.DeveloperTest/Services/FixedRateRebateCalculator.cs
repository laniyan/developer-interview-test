using FluentValidation;
using Microsoft.Extensions.Logging;
using Smartwyre.DeveloperTest.Exceptions;
using Smartwyre.DeveloperTest.Extensions;
using Smartwyre.DeveloperTest.Services.Interfaces;
using Smartwyre.DeveloperTest.Types;
using Smartwyre.DeveloperTest.Types.Dtos;

namespace Smartwyre.DeveloperTest.Services
{
    public class FixedRateRebateCalculator : IRebateCalculator
    {
        private readonly IValidator<CalculateFixedRateRebate> _validator;
        private readonly ILogger<FixedRateRebateCalculator> _log;

        public FixedRateRebateCalculator(IValidator<CalculateFixedRateRebate> validator, ILogger<FixedRateRebateCalculator> log)
        {
            _validator = validator;
            _log = log;
        }

        public decimal CalculateRebate(Rebate rebate, Product product, CalculateRebateRequest request)
        {
            _log.LogInformation("Calculating rebate for RebateId: {RebateId}, ProductId: {ProductId}",
            request.RebateIdentifier, request.ProductIdentifier);

            EnsureValidRebateData(rebate, product, request);

            var rebateAmount = product.Price * rebate.Percentage * request.Volume;
            _log.LogInformation("Rebate calculated successfully: {Amount}", rebateAmount);

            return rebateAmount;
        }

        private void EnsureValidRebateData(Rebate rebate, Product product, CalculateRebateRequest request)
        {
            _log.LogInformation("Validating rebate data for RebateId: {RebateId}, ProductId: {ProductId}",
            request.RebateIdentifier, request.ProductIdentifier);
            var calculate = new CalculateFixedRateRebate();
            var calculateFixedRateRebateDto = calculate.AsCalculateFixedRateRebateDto(rebate, product, request);

            var validationResult = _validator.Validate(calculateFixedRateRebateDto);
            if (!validationResult.IsValid)
            {
                _log.LogError("Rebate data validation failed: {ValidationErrors}", validationResult.ToString());
                throw new CalculateIncentiveRebateException("Invalid Input", validationResult.ToString());
            }

            _log.LogInformation("Rebate data validation successful.");
        }

    }
}