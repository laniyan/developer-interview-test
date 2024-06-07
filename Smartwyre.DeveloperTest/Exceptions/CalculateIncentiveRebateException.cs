using System;

namespace Smartwyre.DeveloperTest.Exceptions
{
    public class CalculateIncentiveRebateException : Exception
    {
        public CalculateIncentiveRebateException(string title, string message) : base(message)
        {
            Title = title;
        }

        public string Title { get; set; }
    }
}