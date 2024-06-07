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

public class AmountPerUomCalculatorTests
{
    private readonly IRebateCalculator _calculator;
    private readonly Mock<ILogger<AmountPerUomCalculator>> _mockLog = new Mock<ILogger<AmountPerUomCalculator>>();
    private readonly Rebate _rebate;
    private readonly Product _product;
    private readonly CalculateRebateRequest _request;


    public AmountPerUomCalculatorTests()
    {
        _calculator = new AmountPerUomCalculator(new CalculateAmountPerUomRebateValidator(), _mockLog.Object);
        _rebate = new Rebate { Incentive = IncentiveType.AmountPerUom, Amount = 10 };
        _product = new Product { SupportedIncentives = SupportedIncentiveType.AmountPerUom };
        _request = new CalculateRebateRequest { Volume = 1 };
    }

    [Fact]
    public void CalculateRebate_ShouldReturnCorrectRebateAmount_WhenRebateDataIsValid()
    {
        // Arrange
        var exceptedAmount = 10;

        // Act
        var result = _calculator.CalculateRebate(_rebate, _product, _request);

        // Assert
        result.Should().Be(exceptedAmount);
    }

    [Theory]
    [InlineData(SupportedIncentiveType.FixedRateRebate)]
    [InlineData(SupportedIncentiveType.FixedCashAmount)]
    public void CalculateRebate_ShouldThrowException_WhenRebateAndProductIncentiveTypeNotMatched(SupportedIncentiveType supportedIncentive)
    {
        // Arrange
        _product.SupportedIncentives = supportedIncentive;

        // Act
        var result = () => _calculator.CalculateRebate(_rebate, _product, _request);

        // Assert
        result.Should().Throw<CalculateIncentiveRebateException>()
            .WithMessage($"'Supported Incentive' must be 'AmountPerUom' for a AmountPerUom calculation. Current value: {_product.SupportedIncentives}");
    }

    [Theory]
    [InlineData(IncentiveType.FixedRateRebate)]
    [InlineData(IncentiveType.FixedCashAmount)]
    public void CalculateRebate_ShouldThrowException_WhenIncentiveTypeIsInvalid(IncentiveType incentive)
    {
        // Arrange
        _rebate.Incentive = incentive;

        // Act
        var result = () => _calculator.CalculateRebate(_rebate, _product, _request);

        // Assert
        result.Should().Throw<CalculateIncentiveRebateException>()
            .WithMessage($"'Incentive Type' must be 'AmountPerUom' for a AmountPerUom calculation. Current value: {_rebate.Incentive}");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void CalculateRebate_ShouldThrowException_WhenAmountIsInvalid(decimal amount)
    {
        // Arrange
        _rebate.Amount = amount;

        // Act
        var result = () => _calculator.CalculateRebate(_rebate, _product, _request);

        // Assert
        result.Should().Throw<CalculateIncentiveRebateException>()
            .WithMessage($"The Amount value must be greater than zero. Current value: {_rebate.Amount}");
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