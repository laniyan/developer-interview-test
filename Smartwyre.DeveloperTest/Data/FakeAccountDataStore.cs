using Smartwyre.DeveloperTest.Types;
using System;

namespace Smartwyre.DeveloperTest.Data
{
    public class FakeAccountDataStore : IAccountDataStore
    {
        public Account GetAccount(string accountNumber)
        {
            if (accountNumber == "123")
                return new Account
                {
                    AccountNumber = "123",
                    Balance = 100,
                    AllowedPaymentSchemes = AllowedPaymentSchemes.ExpeditedPayments 
                        | AllowedPaymentSchemes.BankToBankTransfer,
                    Status = AccountStatus.Live
                };
            return null;
        }

        public void UpdateAccount(Account account)
        {
            // Update account in database, code removed for brevity
        }
    }
}
