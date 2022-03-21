using System;

namespace Smartwyre.DeveloperTest.Types
{
    public class InvalidPaymentRequestException : Exception
    {
        private readonly string fieldInError;

        public InvalidPaymentRequestException(string fieldInError) : base()
        {
            this.fieldInError = fieldInError;
        }

        public string FieldInError => fieldInError;
    }
}
