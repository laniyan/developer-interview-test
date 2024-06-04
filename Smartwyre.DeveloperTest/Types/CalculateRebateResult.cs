namespace Smartwyre.DeveloperTest.Types;

public class CalculateRebateResult(bool success = false)
{
    public bool Success { get; } = success;
}
