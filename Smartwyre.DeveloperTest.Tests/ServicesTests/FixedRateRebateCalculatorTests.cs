using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Smartwyre.DeveloperTest.Exceptions;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Services.Interfaces;
using Smartwyre.DeveloperTest.Types;
using Smartwyre.DeveloperTest.Validations;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.ServicesTests;

public class FixedRateRebateCalculatorTests
{
    private readonly IRebateCalculator _calculator;
    private readonly Mock<ILogger<FixedRateRebateCalculator>> _mockLog = new Mock<ILogger<FixedRateRebateCalculator>>();
    private readonly Rebate _rebate;
    private readonly Product _product;
    private readonly CalculateRebateRequest _request;

    public FixedRateRebateCalculatorTests()
    {
        _calculator = new FixedRateRebateCalculator(new CalculateFixedRateRebateValidator(), _mockLog.Object);
        _rebate = new Rebate { Incentive = IncentiveType.FixedRateRebate, Percentage = 10 };
        _product = new Product { SupportedIncentives = SupportedIncentiveType.FixedRateRebate, Price = 10 };
        _request = new CalculateRebateRequest { Volume = 1 };
    }

    [Fact]
    public void CalculateRebate_ShouldReturnCorrectRebateAmount_WhenRebateDataIsValid()
    {
        // Arrange
        var exceptedAmount = 100;

        // Act
        var result = _calculator.CalculateRebate(_rebate, _product, _request);

        // Assert
        result.Should().Be(exceptedAmount);
    }

    [Theory]
    [InlineData(SupportedIncentiveType.AmountPerUom)]
    [InlineData(SupportedIncentiveType.FixedCashAmount)]
    public void CalculateRebate_ShouldThrowException_WhenRebateAndProductIncentiveTypeNotMatched(SupportedIncentiveType supportedIncentive)
    {
        // Arrange
        _product.SupportedIncentives = supportedIncentive;

        // Act
        var result = () => _calculator.CalculateRebate(_rebate, _product, _request);

        // Assert
        result.Should().Throw<CalculateIncentiveRebateException>()
            .WithMessage($"'Supported Incentive' must be 'FixedRateRebate' for a FixedRateRebate calculation. Current value: {_product.SupportedIncentives}");
    }

    [Theory]
    [InlineData(IncentiveType.AmountPerUom)]
    [InlineData(IncentiveType.FixedCashAmount)]
    public void CalculateRebate_ShouldThrowException_WhenIncentiveTypeIsInvalid(IncentiveType incentive)
    {
        // Arrange
        _rebate.Incentive = incentive;


        // Act
        var result = () => _calculator.CalculateRebate(_rebate, _product, _request);

        // Assert
        result.Should().Throw<CalculateIncentiveRebateException>()
            .WithMessage($"'Incentive Type' must be 'FixedRateRebate' for a FixedRateRebate calculation. Current value: {_rebate.Incentive}");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void CalculateRebate_ShouldThrowException_WhenPercentageIsInvalid(decimal percentage)
    {
        // Arrange
        _rebate.Percentage = percentage;

        // Act
        var result = () => _calculator.CalculateRebate(_rebate, _product, _request);

        // Assert
        result.Should().Throw<CalculateIncentiveRebateException>()
            .WithMessage($"The percentage value must be greater than zero. Current value: {_rebate.Percentage}");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void CalculateRebate_ShouldThrowException_WhenPriceIsInvalid(decimal price)
    {
        // Arrange
        _product.Price = price;

        // Act
        var result = () => _calculator.CalculateRebate(_rebate, _product, _request);

        // Assert
        result.Should().Throw<CalculateIncentiveRebateException>()
            .WithMessage($"The price value must be greater than zero. Current value: {_product.Price}");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void CalculateRebate_ShouldThrowException_WhenVolumeIsInvalid(decimal volume)
    {
        // Arrange
        _request.Volume = volume;

        // Act
        var result = () => _calculator.CalculateRebate(_rebate, _product, _request);

        // Assert
        result.Should().Throw<CalculateIncentiveRebateException>()
            .WithMessage($"The volume value must be greater than zero. Current value: {_request.Volume}");
    }
}