using FluentAssertions;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.Types
{
    public class MakePaymentResultTest
    {
        [Fact]
        public void ShouldCreateResultIndicatingSuccess()
        {
            // Arrange

            // Act
            var result = MakePaymentResult.CreateSuccess();

            // Assert
            result.Success.Should().BeTrue();
        }

        [Fact]
        public void ShouldCreateResultIndicatingFailure()
        {
            // Arrange

            // Act
            var result = MakePaymentResult.CreateFail();

            // Assert
            result.Success.Should().BeFalse();
        }
    }
}
