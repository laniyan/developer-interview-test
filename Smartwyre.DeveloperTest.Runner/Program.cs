using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using System;

namespace Smartwyre.DeveloperTest.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var payment = new PaymentService().MakePayment(IncomingRequest(), QueryAccount());
            Console.WriteLine(payment.Success ? "Payment was success" : "Payment failed");
        }

        private static Account QueryAccount()
        {
            return new Account()
            {
                AccountNumber = "7694823-79847552",
                Balance = 56060.89M,
                Status = AccountStatus.Live,
                AllowedPaymentSchemes = AllowedPaymentSchemes.ExpeditedPayments
            };
        }

        private static MakePaymentRequest IncomingRequest()
        {
            return new MakePaymentRequest()
            {
                CreditorAccountNumber = "23199-1948785-72974",
                DebtorAccountNumber = "7694823-79847552",
                Amount = 3123.12M,
                PaymentDate = DateTime.Now,
                PaymentScheme = PaymentScheme.ExpeditedPayments
            };
        }
    }
}
