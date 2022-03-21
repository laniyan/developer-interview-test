using FluentAssertions;
using Moq;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Services.PaymentValidate;
using Smartwyre.DeveloperTest.Types;
using System;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.Services
{
    public class PaymentServiceTests
    {
        private readonly Mock<IAccountDataStore> mockAccountDataStore = new();

        private readonly PaymentService paymentService;

        public PaymentServiceTests()
        {
            paymentService = new PaymentService(mockAccountDataStore.Object);
        }

        [Fact]
        public void ShouldFailToDebitAccountWhenDebtorAccountNumberIsEmpty()
        {
            // Arrange
            var debitAmount = 100M;
            var debitAccountBalance = debitAmount + 10;
            var account = CreateValidAccount(balance: debitAccountBalance);
            var request = CreateValidPaymentRequest(
                debitAmount: debitAmount, 
                paymentScheme: PaymentScheme.BankToBankTransfer, 
                debtorAccountNumber: string.Empty);

            mockAccountDataStore.Setup(s => s.GetAccount(It.IsAny<string>())).Returns(account);

            // Act
            Func<MakePaymentResult> action = () => paymentService.MakePayment(request);

            // Assert
            action.Should().Throw<InvalidPaymentRequestException>();
            account.Balance.Should().Be(debitAccountBalance);
            mockAccountDataStore.Verify(s => s.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }

        [Fact]
        public void ShouldSuccessfullyDebitAccountWhenBankToBankTransferSucceeds()
        {
            // Arrange
            var debitAmount = 100M;
            var debitAccountBalance = debitAmount + 10;
            var account = CreateValidAccount(balance: debitAccountBalance);
            var request = CreateValidPaymentRequest(debitAmount: debitAmount, paymentScheme: PaymentScheme.BankToBankTransfer);

            mockAccountDataStore.Setup(s => s.GetAccount(It.IsAny<string>())).Returns(account);

            // Act
            var result = paymentService.MakePayment(request);

            // Assert
            result.Success.Should().BeTrue();
            account.Balance.Should().Be(debitAccountBalance - debitAmount);
            mockAccountDataStore.Verify(s => s.UpdateAccount(It.IsAny<Account>()), Times.Once);
        }

        [Fact]
        public void ShouldFailToDebitAccountWhenBankToBankTransferFailsDueToUnknownAccount()
        {
            // Arrange
            var debitAmount = 100M;
            var debitAccountBalance = debitAmount + 10;
            var account = (Account)null;
            var request = CreateValidPaymentRequest(debitAmount: debitAmount, paymentScheme: PaymentScheme.BankToBankTransfer);

            mockAccountDataStore.Setup(s => s.GetAccount(It.IsAny<string>())).Returns(account);

            // Act
            Func<MakePaymentResult> action = () => paymentService.MakePayment(request);

            // Assert
            action.Should().Throw<AccountNotFoundException>();
            account.Should().BeNull();
            mockAccountDataStore.Verify(s => s.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }

        [Fact]
        public void ShouldFailToDebitAccountWhenBankToBankTransferFailsDueToIncorrectFlag()
        {
            // Arrange
            var debitAmount = 100M;
            var debitAccountBalance = debitAmount + 10;
            var account = CreateValidAccount(
                balance: debitAccountBalance, 
                allowedPaymentSchemes: AllowedPaymentSchemes.ExpeditedPayments | AllowedPaymentSchemes.AutomatedPaymentSystem);
            var request = CreateValidPaymentRequest(debitAmount: debitAmount, paymentScheme: PaymentScheme.BankToBankTransfer);

            mockAccountDataStore.Setup(s => s.GetAccount(It.IsAny<string>())).Returns(account);

            // Act
            var result = paymentService.MakePayment(request);

            // Assert
            result.Success.Should().BeFalse();
            account.Balance.Should().Be(debitAccountBalance);
            mockAccountDataStore.Verify(s => s.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }

        [Fact]
        public void ShouldSuccessfullyDebitAccountWhenExpeditedPaymentsTransferSucceeds()
        {
            // Arrange
            var debitAmount = 100M;
            var debitAccountBalance = debitAmount + 10;
            var account = CreateValidAccount(balance: debitAccountBalance);
            var request = CreateValidPaymentRequest(debitAmount: debitAmount, paymentScheme: PaymentScheme.ExpeditedPayments);

            mockAccountDataStore.Setup(s => s.GetAccount(It.IsAny<string>())).Returns(account);

            // Act
            var result = paymentService.MakePayment(request);

            // Assert
            result.Success.Should().BeTrue();
            account.Balance.Should().Be(debitAccountBalance - debitAmount);
            mockAccountDataStore.Verify(s => s.UpdateAccount(It.IsAny<Account>()), Times.Once);
        }

        [Fact]
        public void ShouldFailToDebitAccountWhenExpeditedPaymentsTransferFailsDueToUnknownAccount()
        {
            // Arrange
            var debitAmount = 100M;
            var debitAccountBalance = debitAmount + 10;
            var account = (Account)null;
            var request = CreateValidPaymentRequest(debitAmount: debitAmount, paymentScheme: PaymentScheme.ExpeditedPayments);

            mockAccountDataStore.Setup(s => s.GetAccount(It.IsAny<string>())).Returns(account);

            // Act
            Func<MakePaymentResult> action = () => paymentService.MakePayment(request);

            // Assert
            action.Should().Throw<AccountNotFoundException>();
            account.Should().BeNull();
            mockAccountDataStore.Verify(s => s.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }

        [Fact]
        public void ShouldFailToDebitAccountWhenExpeditedPaymentsTransferFailsDueToIncorrectFlag()
        {
            // Arrange
            var debitAmount = 100M;
            var debitAccountBalance = debitAmount + 10;
            var account = CreateValidAccount(
                balance: debitAccountBalance,
                allowedPaymentSchemes: AllowedPaymentSchemes.BankToBankTransfer | AllowedPaymentSchemes.AutomatedPaymentSystem);
            var request = CreateValidPaymentRequest(debitAmount: debitAmount, paymentScheme: PaymentScheme.ExpeditedPayments);

            mockAccountDataStore.Setup(s => s.GetAccount(It.IsAny<string>())).Returns(account);

            // Act
            var result = paymentService.MakePayment(request);

            // Assert
            result.Success.Should().BeFalse();
            account.Balance.Should().Be(debitAccountBalance);
            mockAccountDataStore.Verify(s => s.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }

        [Fact]
        public void ShouldFailToDebitAccountWhenExpeditedPaymentsTransferFailsDueToInsufficientFunds()
        {
            // Arrange
            var debitAmount = 100M;
            var debitAccountBalance = debitAmount - 10;
            var account = CreateValidAccount(balance: debitAccountBalance);
            var request = CreateValidPaymentRequest(debitAmount: debitAmount, paymentScheme: PaymentScheme.ExpeditedPayments);

            mockAccountDataStore.Setup(s => s.GetAccount(It.IsAny<string>())).Returns(account);

            // Act
            var result = paymentService.MakePayment(request);

            // Assert
            result.Success.Should().BeFalse();
            account.Balance.Should().Be(debitAccountBalance);
            mockAccountDataStore.Verify(s => s.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }

        [Fact]
        public void ShouldSuccessfullyDebitAccountWhenAutomatedPaymentSystemTransferSucceeds()
        {
            // Arrange
            var debitAmount = 100M;
            var debitAccountBalance = debitAmount + 10;
            var account = CreateValidAccount(balance: debitAccountBalance);
            var request = CreateValidPaymentRequest(debitAmount: debitAmount, paymentScheme: PaymentScheme.AutomatedPaymentSystem);

            mockAccountDataStore.Setup(s => s.GetAccount(It.IsAny<string>())).Returns(account);

            // Act
            var result = paymentService.MakePayment(request);

            // Assert
            result.Success.Should().BeTrue();
            account.Balance.Should().Be(debitAccountBalance - debitAmount);
            mockAccountDataStore.Verify(s => s.UpdateAccount(It.IsAny<Account>()), Times.Once);
        }

        [Fact]
        public void ShouldFailToDebitAccountWhenAutomatedPaymentSystemTransferFailsDueToUnknownAccount()
        {
            // Arrange
            var debitAmount = 100M;
            var debitAccountBalance = debitAmount + 10;
            var account = (Account)null;
            var request = CreateValidPaymentRequest(debitAmount: debitAmount, paymentScheme: PaymentScheme.AutomatedPaymentSystem);

            mockAccountDataStore.Setup(s => s.GetAccount(It.IsAny<string>())).Returns(account);

            // Act
            Func<MakePaymentResult> action = () => paymentService.MakePayment(request);

            // Assert
            action.Should().Throw<AccountNotFoundException>();
            account.Should().BeNull();
            mockAccountDataStore.Verify(s => s.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }

        [Fact]
        public void ShouldFailToDebitAccountWhenAutomatedPaymentSystemTransferFailsDueToIncorrectFlag()
        {
            // Arrange
            var debitAmount = 100M;
            var debitAccountBalance = debitAmount + 10;
            var account = CreateValidAccount(
                balance: debitAccountBalance,
                allowedPaymentSchemes: AllowedPaymentSchemes.BankToBankTransfer | AllowedPaymentSchemes.ExpeditedPayments);
            var request = CreateValidPaymentRequest(debitAmount: debitAmount, paymentScheme: PaymentScheme.AutomatedPaymentSystem);

            mockAccountDataStore.Setup(s => s.GetAccount(It.IsAny<string>())).Returns(account);

            // Act
            var result = paymentService.MakePayment(request);

            // Assert
            result.Success.Should().BeFalse();
            account.Balance.Should().Be(debitAccountBalance);
            mockAccountDataStore.Verify(s => s.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }

        [Fact]
        public void ShouldFailToDebitAccountWhenAutomatedPaymentSystemTransferFailsDueToIncorrectStatus()
        {
            // Arrange
            var debitAmount = 100M;
            var debitAccountBalance = debitAmount + 10;
            var account = CreateValidAccount(balance: debitAccountBalance, status: AccountStatus.Disabled);
            var request = CreateValidPaymentRequest(debitAmount: debitAmount, paymentScheme: PaymentScheme.AutomatedPaymentSystem);

            mockAccountDataStore.Setup(s => s.GetAccount(It.IsAny<string>())).Returns(account);

            // Act
            var result = paymentService.MakePayment(request);

            // Assert
            result.Success.Should().BeFalse();
            account.Balance.Should().Be(debitAccountBalance);
            mockAccountDataStore.Verify(s => s.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }

        private static Account CreateValidAccount(
            decimal balance,
            string accountNumber = "123-456-7890",
            AccountStatus status = AccountStatus.Live,
            AllowedPaymentSchemes allowedPaymentSchemes =
                  AllowedPaymentSchemes.ExpeditedPayments
                | AllowedPaymentSchemes.BankToBankTransfer
                | AllowedPaymentSchemes.AutomatedPaymentSystem)
        {
            return new Account(accountNumber, balance, status, allowedPaymentSchemes, new PaymentValidateFactory());
        }

        private static MakePaymentRequest CreateValidPaymentRequest(
            decimal debitAmount,
            PaymentScheme paymentScheme,
            string creditorAccountNumber = "098-765-4321",
            string debtorAccountNumber = "123-456-7890",
            DateTime? paymentDate = null)
        {
            return new MakePaymentRequest()
            {
                CreditorAccountNumber = creditorAccountNumber,
                DebtorAccountNumber= debtorAccountNumber,
                Amount = debitAmount,
                PaymentDate = paymentDate ?? DateTime.UtcNow,
                PaymentScheme = paymentScheme,
            };
        }
    }
}
