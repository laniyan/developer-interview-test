using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services
{
    public interface IPaymentService
    {
        /// <summary>
        /// Executes payment, given payment request
        /// </summary>
        /// <param name="request">Details of the payment request</param>
        /// <exception cref="AccountNotFoundException">The account provided in the <paramref name="request"/>could not be found</exception>
        /// <exception cref="InvalidPaymentRequestException">The payment request was invalid</exception>
        /// <returns>Result of executing the payment request</returns>
        MakePaymentResult MakePayment(MakePaymentRequest request);
    }
}
