using System;

namespace Smartwyre.DeveloperTest.Services
{
    public class AccountNotFoundException : Exception
    {
        private readonly string accountNumber;

        public AccountNotFoundException(string accountNumber) : base()
        {
            this.accountNumber = accountNumber;
        }

        public string AccountNumber => accountNumber;
    }
}
