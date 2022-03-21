using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.PaymentValidate
{
    public interface IPaymentValidateFactory
    {
        IPaymentValidate Create(PaymentScheme paymentScheme, Account account, decimal debitAmount);
    }

    public class PaymentValidateFactory : IPaymentValidateFactory
    {
        public IPaymentValidate Create(PaymentScheme paymentScheme, Account account, decimal debitAmount)
        {
            return paymentScheme switch
            {
                PaymentScheme.BankToBankTransfer => new PaymentValidateBankToBank(account),
                PaymentScheme.ExpeditedPayments => new PaymentValidateExpeditedPayments(account, debitAmount),
                PaymentScheme.AutomatedPaymentSystem => new PaymentValidateAutomatedPaymentSystem(account),
                _ => throw new PaymentTypeNotSupportedException(paymentScheme),
            };
        }
    }
}
