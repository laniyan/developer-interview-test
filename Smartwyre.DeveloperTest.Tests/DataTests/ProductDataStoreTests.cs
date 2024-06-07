using FluentAssertions;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.DataTests
{
    public class ProductDataStoreTests
    {
        [Fact]
        public void GetProduct_ShouldReturnProduct_WhenProductHasBeenStored()
        {
            // Arrange
            var dataStore = new ProductDataStore();
            var product = new Product();

            // Act
            dataStore.StoreProduct(product);
            var result = dataStore.GetProduct("someIdentifier");

            // Assert
            result.Should().Be(product);
        }

        [Fact]
        public void GetProduct_ShouldReturnNull_WhenProductHasNotBeenStored()
        {
            // Arrange
            var dataStore = new ProductDataStore();

            // Act
            var result = dataStore.GetProduct("someIdentifier");

            // Assert
            result.Should().BeNull();
        }
    }
}