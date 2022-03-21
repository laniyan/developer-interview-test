using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Services.PaymentValidate;
using Smartwyre.DeveloperTest.Types;
using System;

namespace Smartwyre.DeveloperTest.Runner
{
    class Program
    {
        private static IPaymentValidateFactory paymentValidateFactory = new PaymentValidateFactory();
        private static IAccountDataStore accountDataStore = new AccountDataStore(paymentValidateFactory);
        private static IPaymentService paymentService = new PaymentService(accountDataStore);

        static void Main(string[] args)
        {
            DependencyInjection();

            ExecuteRequest(new MakePaymentRequest()
            {
                CreditorAccountNumber = "1234-456-789",
                DebtorAccountNumber = "9876-543-210",
                Amount = 10000.00M,
                PaymentDate = DateTime.Now,
                PaymentScheme = PaymentScheme.AutomatedPaymentSystem
            });

            ExecuteRequest(new MakePaymentRequest()
            {
                CreditorAccountNumber = "1234-456-789",
                DebtorAccountNumber = string.Empty,
                Amount = 10000.00M,
                PaymentDate = DateTime.Now,
                PaymentScheme = PaymentScheme.AutomatedPaymentSystem
            });
        }

        private static void DependencyInjection()
        {
            paymentValidateFactory = new PaymentValidateFactory();
            accountDataStore = new AccountDataStore(paymentValidateFactory);
            paymentService = new PaymentService(accountDataStore);
        }

        private static void ExecuteRequest(MakePaymentRequest makePaymentRequest)
        {
            try
            {
                var result = paymentService.MakePayment(makePaymentRequest);
                Console.WriteLine($"Result of transaction: [{result.Success}]");
            }
            catch (InvalidPaymentRequestException e)
            {
                Console.WriteLine($"Transaction failed due to error in request field: [{e.FieldInError}]");
            }
        }
    }
}
