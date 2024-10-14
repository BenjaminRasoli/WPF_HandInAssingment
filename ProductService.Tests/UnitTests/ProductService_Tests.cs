using System.IO;
using System.Linq;
using Shared.Models;
using Shared.Services;
using Xunit;

namespace ProductTests.UnitTests;

public class ProductService_Tests : IDisposable
{
    private readonly string TestFilePath = Path.Combine(AppContext.BaseDirectory, "testfile.json");
    private readonly FileService _fileService;
    private readonly ProductService _productService;

    public ProductService_Tests()
    {
        _fileService = new FileService(TestFilePath);
        _productService = new ProductService(_fileService);
    }

    public void Dispose()
    {
        if (File.Exists(TestFilePath))
        {
            File.Delete(TestFilePath);
        }
    }



    [Fact]
    public void AddProduct__ToList__ShouldReturnTrue()
    {

        var product = new Product
        {
            Id = "1",
            ProductName = "Test Product",
            ProductPrice = "100",
            Category = new Category { CategoryName = "Test Category" }
        };

        var result = _productService.AddProduct(product);

        Assert.True(result.Succeeded);
        Assert.Single(_productService.GetProducts());
    }

    [Fact]

    public void AddProduct_ToList__WithSameName_ShouldReturnFalse()
    {
        var product1 = new Product
        {
            Id = "1",
            ProductName = "Test Product",
            ProductPrice = "100",
            Category = new Category { CategoryName = "Test Category" }
        };
        var product2 = new Product
        {
            Id = "2",
            ProductName = "Test Product",
            ProductPrice = "100",
            Category = new Category { CategoryName = "Test Category" }
        };

        var result1 = _productService.AddProduct(product1);
        var result2 = _productService.AddProduct(product2);

        Assert.True(result1.Succeeded);
        Assert.Single(_productService.GetProducts());

        Assert.False(result2.Succeeded);
        Assert.Single(_productService.GetProducts());
    }


    [Fact]
    public void RemoveProduct__FromList__ShouldReturnTrue()
    {
        var product = new Product
        {
            Id = "1",
            ProductName = "Test Product",
            ProductPrice = "100",
            Category = new Category { CategoryName = "Test Category" }
        };

        _productService.AddProduct(product);

        var result = _productService.RemoveProduct(product.Id);

        Assert.True(result.Succeeded);
        Assert.Empty(_productService.GetProducts());
    }

    [Fact]
    public void UpdateProduct__FromList__ShouldUpdateProduct_ReturnTrue()
    {
        var product = new Product
        {
            Id = "1",
            ProductName = "Old Product",
            ProductPrice = "100",
            Category = new Category { CategoryName = "Old Category" }
        };

        _productService.AddProduct(product);

        var newName = "Updated Product";
        var newPrice = "150";
        var newCategory = "Updated Category";

        var updateResponse = _productService.UpdateProduct(product.Id, newName, newPrice, newCategory);

        Assert.True(updateResponse.Succeeded);
        var updatedProduct = _productService.GetProducts().First();

        Assert.Equal(newName, updatedProduct.ProductName);
        Assert.Equal(newPrice, updatedProduct.ProductPrice);
        Assert.Equal(newCategory, updatedProduct.Category.CategoryName);
    }

    [Fact]
    public void SaveAndLoadProducts__FromFile__ShouldReturnTrue()
    {
        var product1 = new Product
        {
            Id = "1",
            ProductName = "Product A",
            ProductPrice = "100",
            Category = new Category { CategoryName = "Category A" }
        };

        var product2 = new Product
        {
            Id = "2",
            ProductName = "Product B",
            ProductPrice = "200",
            Category = new Category { CategoryName = "Category B" }
        };

        _productService.AddProduct(product1);
        _productService.AddProduct(product2);

        var result = _productService.LoadProductsFromFile();

        Assert.True(result.Succeeded);
        var loadedProducts = _productService.GetProducts().ToList();

        Assert.Equal(2, loadedProducts.Count);
        Assert.Equal(product1.ProductName, loadedProducts[0].ProductName);
        Assert.Equal(product1.ProductPrice, loadedProducts[0].ProductPrice);
        Assert.Equal(product1.Category.CategoryName, loadedProducts[0].Category.CategoryName);
    }
}
