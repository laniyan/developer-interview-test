using FluentAssertions;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.DataTests
{
    public class RebateDataStoreTests
    {
        private readonly IRebateDataStore _rebateDataStore;

        public RebateDataStoreTests()
        {
            _rebateDataStore = new RebateDataStore();
        }

        [Fact]
        public void GetRebate_ShouldReturnRebate_WhenRebateHasBeenStored()
        {
            // Arrange
            var validIdentifier = "someValidId";
            var rebate = new Rebate();

            // Act
            _rebateDataStore.StoreRebate(rebate);
            var result = _rebateDataStore.GetRebate(validIdentifier);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void GetRebate_ShouldReturnNull_WhenRebateHasNotBeenStored()
        {
            // Arrange
            var validIdentifier = "someValidId";

            // Act
            var result = _rebateDataStore.GetRebate(validIdentifier);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void StoreCalculationResult_ShouldUpdateAccountAmount_WhenInputsIsValid()
        {
            // Arrange
            var account = new Rebate { Amount = 0 };
            var rebateAmount = 42;

            // Act
            _rebateDataStore.StoreCalculationResult(account, rebateAmount);

            // Assert
            account.Amount.Should().Be(rebateAmount);
        }
    }
}