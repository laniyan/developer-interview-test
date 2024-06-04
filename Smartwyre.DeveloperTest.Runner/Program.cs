using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;

var builder = Host.CreateApplicationBuilder(args);

builder.Services
    .AddDataStore()
    .AddValidation()
    .AddServices();

builder.Build();
var serviceProvider = builder.Services.BuildServiceProvider();

Console.WriteLine("** PRODUCT REBATE CALCULATOR **");
Console.WriteLine();

var exit = false;
while (!exit)
{
    var option = ReadOption();

    switch (option)
    {
        case "1":
            var service = serviceProvider.GetRequiredService<IRebateService>();
            var result = await Calc(service);
            Console.WriteLine();
            if (result.Success)
                Console.WriteLine($"Rebate successful!");
            else
                Console.WriteLine($"Rebate failed.");
            break;
        case "2":
            exit = true;
            break;
        default:
            Console.WriteLine("Invalid option selected.");
            break;
    }
}

static string ReadOption()
{
    Console.WriteLine("OPTIONS");
    Console.WriteLine("1. Calculate rebate");
    Console.WriteLine("2. Exit");
    Console.WriteLine();
    Console.Write("Enter option: ");
    return Console.ReadLine();
}

static async Task<CalculateRebateResult> Calc(IRebateService rebateService)
{
    Console.Write("Enter the product identifier: ");
    var productIdentifier = Console.ReadLine();
    Console.Write("Enter the rebate identifier: ");
    var rebateIdentifier = Console.ReadLine();

    var isValidVolume = false;
    var volume = 0m;
    while (!isValidVolume)
    {
        Console.Write("Enter the volume: ");
        var volumeStr = Console.ReadLine();

        if (!decimal.TryParse(volumeStr, out volume))
        {
            Console.WriteLine("Invalid volume entered.");
            continue;
        }

        isValidVolume = true;
    }

    var request = new CalculateRebateRequest
    {
        ProductIdentifier = productIdentifier,
        RebateIdentifier = rebateIdentifier,
        Volume = volume
    };

    return await rebateService.Calculate(request);
}
