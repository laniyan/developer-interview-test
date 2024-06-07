using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data;

public interface IProductDataStore
{
    Product GetProduct(string productIdentifier);

    // Add this method to the interface to set the Product for testing purposes using CLI
    void StoreProduct(Product product);
}