using Microsoft.Extensions.Logging;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Exceptions;
using Smartwyre.DeveloperTest.Services.Interfaces;
using Smartwyre.DeveloperTest.Types;
using System;

namespace Smartwyre.DeveloperTest.Services;

public class RebateService : IRebateService
{
    private readonly IRebateDataStore _rebateDataStore;
    private readonly IProductDataStore _productDataStore;
    private readonly IRebateCalculatorFactory _calculatorFactory;
    private IRebateCalculator _calculator;
    private readonly ILogger<RebateService> _log;
    private readonly ICacheWrapper _memoryCache;

    public RebateService(IRebateDataStore rebateDataStore, IProductDataStore productDataStore, IRebateCalculatorFactory calculatorFactory,
        ILogger<RebateService> log, ICacheWrapper memoryCache)
    {
        _rebateDataStore = rebateDataStore;
        _productDataStore = productDataStore;
        _calculatorFactory = calculatorFactory;
        _log = log;
        _memoryCache = memoryCache;
    }

    public CalculateRebateResult Calculate(CalculateRebateRequest request)
    {
        try
        {
            var result = new CalculateRebateResult
            {
                Success = false
            };

            var calculateRebateKey = $"{request.RebateIdentifier}-{request.ProductIdentifier}";

            var rebateAmount = _memoryCache.GetCachedRebateAmount(calculateRebateKey);
            if (rebateAmount > 0)
            {
                _log.LogInformation("Rebate amount retrieved from cache: {Amount}", rebateAmount);
                result.Success = true;
                return result;
            }
            _log.LogInformation("Rebate amount not found in cache.");

            _log.LogInformation("Retrieving rebate and product data for identifiers: {RebateId}, {ProductId}",
            request.RebateIdentifier, request.ProductIdentifier);
            var rebate = _rebateDataStore.GetRebate(request.RebateIdentifier);
            var product = _productDataStore.GetProduct(request.ProductIdentifier);

            if (rebate == null || product == null)
            {
                _log.LogWarning("Rebate or product data not found for identifiers: {RebateId}, {ProductId}",
                   request.RebateIdentifier, request.ProductIdentifier);

                return result;
            }
            _log.LogInformation("Rebate and product data retrieved successfully.");

            _calculator = _calculatorFactory.GetInstance(rebate.Incentive);
            rebateAmount = _calculator.CalculateRebate(rebate, product, request);
            _log.LogInformation("Rebate amount calculated: {Amount}", rebateAmount);

            _rebateDataStore.StoreCalculationResult(rebate, rebateAmount);
            _memoryCache.CacheRebateAmount(calculateRebateKey, rebateAmount);
            _log.LogInformation("Rebate amount calculated and stored: {Amount}", rebateAmount);

            result.Success = true;

            return result;
        }
        catch (CalculateIncentiveRebateException ex)
        {
            ErrorHandling.ExceptionHandler(ex, _log, ex.Title);
            return new CalculateRebateResult { Success = false };
        }
        catch (Exception ex)
        {
            ErrorHandling.ExceptionHandler(ex, _log);
            return new CalculateRebateResult { Success = false };
        }
    }
}