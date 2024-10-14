using Shared.Models;

namespace Shared.Services;

public class ProductService
{
    private List<Product> _products = [];


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
}
