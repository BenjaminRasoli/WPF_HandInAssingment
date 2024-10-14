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
}
