using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Exceptions;
using Smartwyre.DeveloperTest.Extensions;
using Smartwyre.DeveloperTest.Services.Interfaces;
using Smartwyre.DeveloperTest.Types;
using System;
using System.IO;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Smartwyre.DeveloperTest.Runner;

class Program
{
    private static readonly Rebate _rebate = new();
    private static readonly Product _product = new();
    private static readonly CalculateRebateRequest _request = new();
    private static IProductDataStore _productDataStore;
    private static IRebateDataStore _rebateDataStore;
    private static IRebateService _rebateService;
    private static CalculateRebateResult _result;
    private static ILogger _log;

    static void Main(string[] args)
    {
        try
        {
            var host = Initialize();
            StartApplication();
            host.Run();
        }
        catch (Exception ex)
        {
            ErrorHandling.ExceptionHandler(ex, _log);
            Continue();
        }
    }

    static IHost Initialize()
    {
        var builder = new ConfigurationBuilder();
        BuildConfig(builder);

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Build())
            .Enrich.FromLogContext()
            .CreateLogger();
        //If this was a real application I'll log to the Console and write to a file and set the log levels according to the enviroment

        var host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddDeveloperTest();
            })
            .UseSerilog()
            .Build();

        var serviceProvider = host.Services;
        _productDataStore = serviceProvider.GetService<IProductDataStore>();
        _rebateDataStore = serviceProvider.GetService<IRebateDataStore>();
        _rebateService = serviceProvider.GetService<IRebateService>();
        _log = serviceProvider.GetService<ILogger<Program>>();

        return host;
    }

    static void BuildConfig(IConfigurationBuilder builder)
    {
        builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    }

    static void StartApplication()
    {
        PopulateData();
        StoreData();
        CalculateResult();
        DisplayResult();
        Continue();
    }

    static void Continue()
    {
        Console.WriteLine("Do you want to calculate another rebate? (Y/N)");
        var response = Console.ReadLine();
        if (response.ToLower() == "y")
        {
            StartApplication();
        }
        else if (response.ToLower() == "n")
        {
            UserQuitApplication();
        }
        else
        {
            Console.WriteLine("Invalid response. Please try again");
            Continue();
        }
    }

    private static void DisplayResult()
    {
        Console.WriteLine("Result Success : " + _result.Success.ToString());
        if (_result.Success)
        {
            Console.WriteLine("Rebate Amount : " + _rebate.Amount);
        }
    }

    private static void PopulateData()
    {
        Console.WriteLine("Smartwyre Developer Test");
        GetRebateData();
        GetProductData();
        GetRequestData();
    }

    private static void GetRebateData()
    {
        Console.WriteLine("Please Enter Rebate Details");

        Console.WriteLine("Rebate Identifier: ");
        _rebate.Identifier = Console.ReadLine();

        Console.WriteLine("Rebate Incentive: ");
        var incentiveType = ConvertUserInputToIncentiveTypeProcess<IncentiveType>();
        _rebate.Incentive = incentiveType;

        GetRebateAmountBasedOnIncentive();
    }

    private static void GetRebateAmountBasedOnIncentive()
    {
        if (_rebate.Incentive == IncentiveType.FixedCashAmount || _rebate.Incentive == IncentiveType.AmountPerUom)
        {
            Console.WriteLine("Rebate Amount: ");
            _rebate.Amount = ConvertUserInputToDecimalProcess();
        }
    }

    private static void GetProductData()
    {
        Console.WriteLine("Please Enter Product Details");

        Console.WriteLine("Product Identifier: ");
        _product.Identifier = Console.ReadLine();

        Console.WriteLine("Product supported Incentives: ");
        var supportedIncentiveType = ConvertUserInputToIncentiveTypeProcess<SupportedIncentiveType>();
        _product.SupportedIncentives = supportedIncentiveType;

        GetFixedRateRebateInventiveTypePriceAndPercentage();

    }

    private static void GetFixedRateRebateInventiveTypePriceAndPercentage()
    {
        if (_rebate.Incentive == IncentiveType.FixedRateRebate)
        {
            Console.WriteLine("Product Price: ");
            _product.Price = ConvertUserInputToDecimalProcess();

            Console.WriteLine("Rebate Percentage: ");
            _rebate.Percentage = ConvertUserInputToDecimalProcess();
        }
    }

    private static void GetRequestData()
    {
        _request.ProductIdentifier = _product.Identifier;
        _request.RebateIdentifier = _rebate.Identifier;
        GetRequestDataBasedOnIncentiveType();
    }

    private static void GetRequestDataBasedOnIncentiveType()
    {
        if (_rebate.Incentive == IncentiveType.AmountPerUom || _rebate.Incentive == IncentiveType.FixedRateRebate)
        {
            Console.WriteLine("Request Volume: ");
            _request.Volume = ConvertUserInputToDecimalProcess();
        }

    }

    #region Helper Methods 
    private static decimal ConvertUserInputToDecimalProcess()
    {
        decimal amount;
        var input = Console.ReadLine();
        while (!decimal.TryParse(input, out amount))
        {
            Console.WriteLine($"{input} is not a digit, please enter a valid digit or press 'q' to quit");
            input = Console.ReadLine();

            if (input.ToLower().Trim() == "q")
            {
                UserQuitApplication();
            }
        }
        return amount;
    }

    private static T ConvertUserInputToIncentiveTypeProcess<T>() where T : Enum
    {
        T rebateIncentive;

        var input = FormatIncentiveInput(Console.ReadLine());
        while (!Enum.IsDefined(typeof(T), input))
        {
            Console.WriteLine($"{input} is not a valid Incentive type, please enter a valid Incentive type or press 'q' to quit");
            input = FormatIncentiveInput(Console.ReadLine());
            if (input.ToLower().Trim() == "q")
            {
                UserQuitApplication();
            }
        }

        rebateIncentive = (T)Enum.Parse(typeof(T), input);

        return rebateIncentive;
    }

    private static void UserQuitApplication()
    {
        Console.WriteLine("Thank you for using Smartwyre Developer Test");
        Environment.Exit(0);
    }

    private static string FormatIncentiveInput(string input)
    {
        return string.Join("", input.Split(' ')).Trim();
    }

    private static void CalculateResult()
    {
        _result = _rebateService.Calculate(_request);
    }

    private static void StoreData()
    {
        _productDataStore.StoreProduct(_product);
        _rebateDataStore.StoreRebate(_rebate);
    }
    #endregion
}