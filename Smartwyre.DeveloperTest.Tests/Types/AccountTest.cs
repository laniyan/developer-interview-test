using FluentAssertions;
using Moq;
using Smartwyre.DeveloperTest.Services.PaymentValidate;
using Smartwyre.DeveloperTest.Types;
using System;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.Types
{
    public class AccountTest
    {
        private readonly Mock<IPaymentValidateFactory> mockPaymentValidateFactory = new ();
        private readonly Mock<IPaymentValidate> mockPaymentValidate = new();

        private readonly Account account;
        private readonly decimal accountBalance = 100M;
        private readonly PaymentScheme paymentScheme = PaymentScheme.ExpeditedPayments;

        public AccountTest()
        {
            account = CreateValidAccount(accountBalance);
        }

        [Fact]
        public void ShouldThrowExceptionWhenDebitAmountIsNegative()
        {
            // Arrange
            var debitAmount = -1M;

            // Act
            Func<MakePaymentResult> action = () => account.ExecuteDebit(paymentScheme, debitAmount);

            //Assert
            action.Should().Throw<InvalidPaymentRequestException>();
        }

        [Fact]
        public void ShouldSuccessfullyDebitAccountWhenRequestIsValid()
        {
            // Arrange
            var debitAmount = 1M;
            mockPaymentValidateFactory
                .Setup(s => s.Create(It.IsAny<PaymentScheme>(), It.IsAny<Account>(), It.IsAny<decimal>()))
                .Returns(mockPaymentValidate.Object);
            mockPaymentValidate.Setup(s => s.IsValid()).Returns(MakePaymentResult.CreateSuccess());

            // Act
            var result = account.ExecuteDebit(paymentScheme, debitAmount);

            //Assert
            result.Success.Should().BeTrue();
            account.Balance.Should().Be(accountBalance - debitAmount);
        }

        [Fact]
        public void ShouldFailToDebitAccountWhenRequestIsInvalid()
        {
            // Arrange
            var debitAmount = 1M;
            mockPaymentValidateFactory
                .Setup(s => s.Create(It.IsAny<PaymentScheme>(), It.IsAny<Account>(), It.IsAny<decimal>()))
                .Returns(mockPaymentValidate.Object);
            mockPaymentValidate.Setup(s => s.IsValid()).Returns(MakePaymentResult.CreateFail());

            // Act
            var result = account.ExecuteDebit(paymentScheme, debitAmount);

            //Assert
            result.Success.Should().BeFalse();
            account.Balance.Should().Be(accountBalance);
        }

        private Account CreateValidAccount(
            decimal balance,
            string accountNumber = "123-456-7890",
            AccountStatus status = AccountStatus.Live,
            AllowedPaymentSchemes allowedPaymentSchemes =
                  AllowedPaymentSchemes.ExpeditedPayments
                | AllowedPaymentSchemes.BankToBankTransfer
                | AllowedPaymentSchemes.AutomatedPaymentSystem)
        {
            return new Account(accountNumber, balance, status, allowedPaymentSchemes, mockPaymentValidateFactory.Object);
        }
    }
}
