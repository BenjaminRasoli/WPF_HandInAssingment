using Newtonsoft.Json;
using Shared.Interfaces;
using Shared.Models;

namespace Shared.Services;

public class ProductService : IProductService
{
    private List<Product> _products = [];

    private static readonly string _filePath = Path.Combine(AppContext.BaseDirectory, "file.json");
    private readonly IFileService _fileService;
    public ProductService(IFileService fileService)
    {
        _fileService = fileService;
        LoadProductsFromFile();
    }

    public ServiceResponse LoadProductsFromFile()
    {
        try
        {
            var response = _fileService.GetFromFile();
            if (response.Succeeded && response.Result is string content)
            {
                _products = JsonConvert.DeserializeObject<List<Product>>(content) ?? new List<Product>();
                return new ServiceResponse
                {
                    Succeeded = true,
                    Message = "Products loaded successfully."
                };
            }
            else
            {
                _products = new List<Product>();
                return new ServiceResponse
                {
                    Succeeded = false,
                    Message = response.Message
                };
            }
        }
        catch (Exception ex)
        {
            _products = [];
            return new ServiceResponse
            {
                Succeeded = false,
                Message = ex.Message
            };
        }
    }


    public ServiceResponse AddProduct(Product product)
    {
        try
        {
            if (string.IsNullOrEmpty(product.ProductName) || string.IsNullOrEmpty(product.ProductPrice) || string.IsNullOrEmpty(product.Category.CategoryName))
            {
                return new ServiceResponse
                {
                    Succeeded = false,
                    Message = "Please enter a product  name,price and category"
                };
            }

            if (_products.Any(p => p.ProductName.Equals(product.ProductName, StringComparison.OrdinalIgnoreCase)))
            {
                return new ServiceResponse
                {
                    Succeeded = false,
                    Message = "Product with that name already exists"
                };
            }

            _products.Add(product);
            var json = JsonConvert.SerializeObject(_products, Newtonsoft.Json.Formatting.Indented);
            var result = _fileService.SaveToFile(json);

            return new ServiceResponse
            {
                Succeeded = true,
                Message = "Product was added successfully",
                Result = product
            };
        }
        catch (Exception ex)
        {
            return new ServiceResponse
            {
                Succeeded = false,
                Message = ex.Message,
            };
        }
    }

    public IEnumerable <Product> GetProducts()
    {
        return _products;
    }

    public ServiceResponse RemoveProduct(string id)
    {
        try
        {

            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return new ServiceResponse
                {
                    Succeeded = false,
                    Message = "No product with that id"
                };
            }
            _products.Remove(product);
            var json = JsonConvert.SerializeObject(_products, Newtonsoft.Json.Formatting.Indented);
            _fileService.SaveToFile(json);

            return new ServiceResponse
            {
                Succeeded = true,
                Message = "Product was deleted suecesfully"
            };
        }
        catch (Exception ex)
        {
            return new ServiceResponse
            {
                Succeeded = false,
                Message = ex.Message
            };
        }
    }

    public ServiceResponse UpdateProduct(string id, string newName, string newPrice, string newCategory)
    {
        try
        {

            if (string.IsNullOrEmpty(newName) || string.IsNullOrEmpty(newPrice) || string.IsNullOrEmpty(newCategory))
            {
                return new ServiceResponse
                {
                    Succeeded = false,
                    Message = "Please enter a product name,price and category"
                };
            }
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return new ServiceResponse
                {
                    Succeeded = false,
                    Message = "No product with that id"
                };
            }

            product.ProductName = newName;
            product.ProductPrice = newPrice;
            product.Category.CategoryName = newCategory;

            var json = JsonConvert.SerializeObject(_products, Newtonsoft.Json.Formatting.Indented);
            var result = _fileService.SaveToFile(json);

            return new ServiceResponse
            {
                Succeeded = true,
                Message = "Product updated successfully"
            };
        }
        catch (Exception ex)
        {
            return new ServiceResponse
            {
                Succeeded = false,
                Message = ex.Message
            };
        }
    }
}
