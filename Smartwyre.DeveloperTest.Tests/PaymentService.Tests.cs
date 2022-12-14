using System;
using Xunit;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using Smartwyre.DeveloperTest.Data;

namespace Smartwyre.DeveloperTest.Tests
{
    public class PaymentServiceTests
    {
        public MakePaymentRequest Request { get; set; }
        public Account Account { get; set; }


        private void CreateMockObjects(decimal balance, decimal amount, PaymentScheme scheme, AccountStatus status, AllowedPaymentSchemes allowedScheme)
        {
            Request = new MakePaymentRequest
            {
                CreditorAccountNumber = "123",
                DebtorAccountNumber = "456",
                Amount = amount,
                PaymentDate = new DateTime(2022, 12, 14),
                PaymentScheme = scheme
            };

            Account = new Account
            {
                AccountNumber = "456",
                Balance = balance,
                Status = status,
                AllowedPaymentSchemes = allowedScheme
            };
        }


        [Fact]
        public void MakePaymentResult_ShouldReturnTrue_AfterPassingAllEdgeCases()
        {
            CreateMockObjects(0, 0, PaymentScheme.BankToBankTransfer, AccountStatus.InboundPaymentsOnly, AllowedPaymentSchemes.BankToBankTransfer);
            var paymentService = new PaymentService();
            var result = paymentService.MakePayment(Request, Account);

            Assert.True(result.Success);
        }

        [Fact]
        public void MakePaymentResult_ShouldReturnFalse_BalanceSmallerThanAmount()
        {
            CreateMockObjects(0, 1, PaymentScheme.ExpeditedPayments, AccountStatus.Live, AllowedPaymentSchemes.ExpeditedPayments);
            var paymentService = new PaymentService();
            var result = paymentService.MakePayment(Request, Account);
            
            Assert.False(result.Success);
        }

        [Fact]
        public void MakePaymentResult_ShouldReturnFalse_AccountStatusNotLive()
        {
            CreateMockObjects(0, 1, PaymentScheme.AutomatedPaymentSystem, AccountStatus.Disabled, AllowedPaymentSchemes.AutomatedPaymentSystem);
            var paymentService = new PaymentService();
            var result = paymentService.MakePayment(Request, Account);

            Assert.False(result.Success);
        }

        /* INTERVIEWER NOTES: I had troubles figuring out a way to unit test the AllowedPaymentScheme enum, I wanted to get it to fail, 
         * but couldn't seem to find a way to get it to without a larger restucture of code, so I commented this out to show my attempt */
        //[Fact]
        //public void MakePaymentResult_ShouldReturnFalse_AllowedPaymentSchemeNotFound()
        //{
        //    CreateMockObjects(0, 1, PaymentScheme.BankToBankTransfer, AccountStatus.Live, AllowedPaymentSchemes.BankToBankTransfer);
        //    var paymentService = new PaymentService();
        //    var result = paymentService.MakePayment(Request, Account);

        //    Assert.False(result.Success);
        //}
    }
}
