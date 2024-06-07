using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace Smartwyre.DeveloperTest.Exceptions
{
    public static class ErrorHandling
    {
        public static void ExceptionHandler(Exception ex, ILogger log, string title = "ERROR")
        {
            log.LogError(
                    ex,
                    "Could not process a request on machine {Machine}. TraceId: {TraceId}",
                    Environment.MachineName,
                    Activity.Current?.TraceId);

            var errorDetails = new CustomErrorDetails
            {
                Title = title,
                Message = ex?.Message,
                TraceId = Activity.Current?.TraceId.ToString(),
                Detail = ex.ToString()
            };

            HandleError(errorDetails);
        }

        private static void HandleError(CustomErrorDetails errorDetails)
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"Title: {errorDetails.Title}");
            Console.WriteLine($"Message: {errorDetails.Message}");
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }

    public class CustomErrorDetails
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string Detail { get; set; }
        public string TraceId { get; set; }
    }
}
