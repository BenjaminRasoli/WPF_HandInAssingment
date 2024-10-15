using Shared.Models;

namespace Shared.Interfaces;

public interface IProductService
{
    ServiceResponse AddProduct(Product product);
    IEnumerable <Product> GetProducts();
    ServiceResponse LoadProductsFromFile();
    ServiceResponse RemoveProduct(string id);
    ServiceResponse UpdateProduct(string id, string newName, string newPrice, string newCategory);
}
