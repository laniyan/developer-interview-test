using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using System;

namespace Smartwyre.DeveloperTest.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new PaymentService(new FakeAccountDataStore());
            while (true)
            {
                try
                {
                    Console.WriteLine("---------------------------");
                    Console.WriteLine("Enter debtor account number");
                    var debtorAccountNumber = Console.ReadLine();

                    Console.WriteLine("Enter amount");
                    var amount = int.Parse(Console.ReadLine());

                    Console.WriteLine("Select payment scheme:");
                    Console.WriteLine("0 - Expedited payments");
                    Console.WriteLine("1 - Bank to bank transfer");
                    Console.WriteLine("2 - Automated payment system");
                    var scheme = (PaymentScheme)int.Parse(Console.ReadLine());

                    var request = new MakePaymentRequest
                    {
                        DebtorAccountNumber = debtorAccountNumber,
                        Amount = amount,
                        PaymentScheme = scheme,
                        PaymentDate = DateTime.UtcNow,
                       
                    };
                    var result = service.MakePayment(request);
                    Console.WriteLine(result.Success ? "Success!" : "Failed!");
                    Console.WriteLine(result.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
