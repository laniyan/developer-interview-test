using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Services.Interfaces;
using Smartwyre.DeveloperTest.Types;
using System.Collections.Generic;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.ServicesTests;

public class RebateServiceTests
{
    Mock<IRebateDataStore> _mockRebateDataStore = new Mock<IRebateDataStore>();
    Mock<IProductDataStore> _mockProductDataStore = new Mock<IProductDataStore>();
    Mock<IRebateCalculatorFactory> _mockCalculatorFactory = new Mock<IRebateCalculatorFactory>();
    Mock<IRebateCalculator> _mockCalculator = new Mock<IRebateCalculator>();
    Mock<ILogger<RebateService>> _mockLog = new Mock<ILogger<RebateService>>();
    Mock<ICacheWrapper> _mockMemoryCache = new Mock<ICacheWrapper>();
    RebateService _rebateService;

    public RebateServiceTests()
    {
        _rebateService = new RebateService(_mockRebateDataStore.Object, _mockProductDataStore.Object, _mockCalculatorFactory.Object, _mockLog.Object, _mockMemoryCache.Object); ;
    }
    public static IEnumerable<object[]> CalculateRequestTestData()
    {
        yield return new object[] { null, new Rebate() };
        yield return new object[] { new Product(), null };
        yield return new object[] { null, null };
    }

    [Fact]
    public void Calculate_ShouldReturnTrue_WhenValidRequestAndNotInCache()
    {
        // Arrange
        var rebate = new Rebate() { Incentive = IncentiveType.FixedRateRebate, Percentage = 10 };
        var product = new Product() { SupportedIncentives = SupportedIncentiveType.FixedRateRebate, Price = 10 };
        var request = new CalculateRebateRequest() { Volume = 10 };

        _mockMemoryCache.Setup(x => x.GetCachedRebateAmount(It.IsAny<string>())).Returns(0);
        _mockMemoryCache.Setup(x => x.CacheRebateAmount(It.IsAny<string>(), It.IsAny<decimal>()));
        _mockRebateDataStore.Setup(r => r.GetRebate(It.IsAny<string>())).Returns(rebate);
        _mockProductDataStore.Setup(p => p.GetProduct(It.IsAny<string>())).Returns(product);
        _mockCalculatorFactory.Setup(c => c.GetInstance(rebate.Incentive)).Returns(_mockCalculator.Object);

        // Act
        var result = _rebateService.Calculate(request);

        // Assert
        result.Success.Should().BeTrue();
    }

    [Fact]
    public void Calculate_ShouldReturnTrue_WhenValidRequestIsCached()
    {
        // Arrange
        var request = new CalculateRebateRequest();

        _mockMemoryCache.Setup(x => x.GetCachedRebateAmount(It.IsAny<string>())).Returns(1);

        // Act
        var result = _rebateService.Calculate(request);

        // Assert
        result.Success.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(CalculateRequestTestData))]
    public void Calculate_ShouldReturnFalse_WhenProductOrRebateIsNull(Product product, Rebate rebate)
    {
        // Arrange
        var request = new CalculateRebateRequest();

        _mockMemoryCache.Setup(x => x.GetCachedRebateAmount(It.IsAny<string>())).Returns(0);
        _mockRebateDataStore.Setup(r => r.GetRebate(It.IsAny<string>())).Returns(rebate);
        _mockProductDataStore.Setup(p => p.GetProduct(It.IsAny<string>())).Returns(product);

        // Act
        var result = _rebateService.Calculate(request);

        // Assert
        result.Success.Should().BeFalse();
    }

}