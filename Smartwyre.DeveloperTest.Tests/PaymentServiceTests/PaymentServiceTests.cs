using FluentAssertions;
using Moq;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.PaymentServiceTests
{
    public class PaymentServiceTests
    {
        private Mock<IAccountDataStore> accountDataStoreMock = new Mock<IAccountDataStore>();

        private Account account;
        private string debtorAccountNumber;
        private MakePaymentRequest paymentRequest;

        public PaymentServiceTests()
        {
            debtorAccountNumber = "1234";
            account = BuildValidAccount(debtorAccountNumber, balance: 100);
            accountDataStoreMock.Setup(x => x.GetAccount(debtorAccountNumber))
                .Returns(account);
            paymentRequest = BuildValidPaymentRequest(debtorAccountNumber, amount: 50, PaymentScheme.ExpeditedPayments);
        }

        [Theory]
        [InlineData(PaymentScheme.ExpeditedPayments)]
        [InlineData(PaymentScheme.BankToBankTransfer)]
        [InlineData(PaymentScheme.AutomatedPaymentSystem)]
        public void MakePayment_WhenAccountAllowsRequestedPaymentScheme_ShouldSucceed(PaymentScheme paymentScheme)
        {
            paymentRequest = BuildValidPaymentRequest(debtorAccountNumber, amount: 50, paymentScheme);

            var result = BuildPaymentService().MakePayment(paymentRequest);

            result.Should().BeEquivalentTo(new MakePaymentResult
            {
                Success = true,
                Message = "New balance: 50"
            });
        }

        [Fact]
        public void MakePayment_WhenAccountAllowsRequestedPaymentScheme_ShouldCallUpdateAccountWithNewBalance()
        {
            Account account = null;
            accountDataStoreMock.Setup(c => c.UpdateAccount(It.IsAny<Account>()))
                .Callback<Account>(a => account = a);

            var result = BuildPaymentService().MakePayment(paymentRequest);

            Assert.Equal(50, account.Balance);
        }

        [Fact]
        public void MakePayment_WhenAccountIsNull_ShouldFail()
        {
            accountDataStoreMock.Setup(x => x.GetAccount(debtorAccountNumber))
                .Returns<Account>(null);

            var result = BuildPaymentService().MakePayment(paymentRequest);

            result.Should().BeEquivalentTo(new MakePaymentResult
            {
                Success = false,
                Message = "Account '1234' not found"
            });
        }

        [Fact]
        public void MakePayment_WhenAccountDoesNotAllowRequestedPaymentScheme_ShouldFail()
        {
            account.AllowedPaymentSchemes = 0;

            var result = BuildPaymentService().MakePayment(paymentRequest);

            result.Should().BeEquivalentTo(new MakePaymentResult
            {
                Success = false,
                Message = "Payment scheme 'ExpeditedPayments' not allowed by account"
            });
        }

        [Fact]
        public void MakePayment_WhenAccountBalanceIsLessThanRequestAmount_ShouldFail()
        {
            account.Balance = 49;

            var result = BuildPaymentService().MakePayment(paymentRequest);

            result.Should().BeEquivalentTo(new MakePaymentResult
            {
                Success = false,
                Message = "Payment amount '50' is greater than balance '49'"
            });
        }

        [Fact]
        public void MakePayment_WhenAccountIsNotLive_ShouldFail()
        {
            account.Status = AccountStatus.Disabled;

            var result = BuildPaymentService().MakePayment(paymentRequest);

            result.Should().BeEquivalentTo(new MakePaymentResult
            {
                Success = false,
                Message = "Account '1234' is not live"
            });
        }

        private IPaymentService BuildPaymentService()
        {
            return new PaymentService(accountDataStoreMock.Object);
        }

        private Account BuildValidAccount(string debtorAccountNumber, decimal balance)
        {
            return new Account
            {
                AccountNumber = debtorAccountNumber,
                AllowedPaymentSchemes = AllowedPaymentSchemes.BankToBankTransfer
                    | AllowedPaymentSchemes.AutomatedPaymentSystem
                    | AllowedPaymentSchemes.ExpeditedPayments,
                Balance = balance,
                Status = AccountStatus.Live
            };
        }

        private MakePaymentRequest BuildValidPaymentRequest(string debtorAccountNumber, decimal amount, PaymentScheme paymentScheme)
        {
            return new MakePaymentRequest
            {
                Amount = amount,
                DebtorAccountNumber = debtorAccountNumber,
                PaymentScheme = paymentScheme,
            };
        }
    }
}
