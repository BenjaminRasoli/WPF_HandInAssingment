using Newtonsoft.Json;
using Shared.Models;

namespace Shared.Services;

public class ProductService
{
    private List<Product> _products = [];

    private static readonly string _filePath = Path.Combine(AppContext.BaseDirectory, "file.json");
    private readonly FileService _fileService;

    public ProductService(FileService fileService)
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
			if(string.IsNullOrEmpty(product.ProductName) || string.IsNullOrEmpty(product.ProductPrice))
			{
                return new ServiceResponse
                {
                    Succeeded = false,
                    Message = "Please enter a product name and price"
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

    public IEnumerable<Product> GetProducts()
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

    public ServiceResponse UpdateProduct(string id, string newName, string newPrice)
    {
        try
        {

            if (string.IsNullOrEmpty(newName) || string.IsNullOrEmpty(newPrice))
            {
                return new ServiceResponse
                {
                    Succeeded = false,
                    Message = "Please enter a product name and price"
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
