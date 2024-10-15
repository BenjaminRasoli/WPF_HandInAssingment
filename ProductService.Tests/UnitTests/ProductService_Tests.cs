using System.IO;
using System.Linq;
using Moq;
using Newtonsoft.Json;
using Shared.Interfaces;
using Shared.Models;
using Shared.Services;
using Xunit;

namespace ProductTests.UnitTests;

public class ProductService_Tests
{

        private readonly Mock<IFileService> _fileServiceMock;
        private readonly ProductService _productService;

        public ProductService_Tests()
        {
            _fileServiceMock = new Mock<IFileService>();
            _productService = new ProductService(_fileServiceMock.Object);
        }

        [Fact]
        public void AddProduct_ValidProduct_ShouldReturnTrueAndAddProduct()
        {
            var product = new Product
            {
                Id = "1",
                ProductName = "Test Product",
                ProductPrice = "100",
                Category = new Category { CategoryName = "Test Category" }
            };

            _fileServiceMock
                .Setup(fs => fs.SaveToFile(It.IsAny<string>()))
                .Returns(new ServiceResponse { Succeeded = true });

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

        _fileServiceMock
            .Setup(fs => fs.SaveToFile(It.IsAny<string>()))
            .Returns(new ServiceResponse { Succeeded = true });

        var result1 = _productService.AddProduct(product1);
        var result2 = _productService.AddProduct(product2);

        Assert.False(result2.Succeeded);
    }

    [Fact]

    public void RemoveProduct__FromList__ShouldReturnTrue()
    {
        var product = new Product
        {
            Id = "1",
            ProductName = "Test",
            ProductPrice = "100",
            Category = new Category { CategoryName = "Test Category" }
        }; 


        _fileServiceMock
            .Setup(fs => fs.SaveToFile(It.IsAny<string>()))
            .Returns(new ServiceResponse { Succeeded = true });

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

        _fileServiceMock
    .Setup(fs => fs.SaveToFile(It.IsAny<string>()))
    .Returns(new ServiceResponse { Succeeded = true });

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
    public void SaveAndLoadProducts_FromFile_ShouldReturnTrue()
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

        var json = JsonConvert.SerializeObject(new List<Product> { product1, product2 });

        _fileServiceMock
            .Setup(fs => fs.SaveToFile(It.IsAny<string>()))
            .Returns(new ServiceResponse 
            { 
                Succeeded = true
            });

        _fileServiceMock
            .Setup(fs => fs.GetFromFile())
            .Returns(new ServiceResponse
            {
                Succeeded = true,
                Result = json
            });

        _productService.AddProduct(product1);
        _productService.AddProduct(product2);
        var result = _productService.LoadProductsFromFile();

        Assert.True(result.Succeeded);
        var loadedProducts = _productService.GetProducts().ToList();

        Assert.Equal(2, loadedProducts.Count);
        Assert.Contains(loadedProducts, p => p.ProductName == product1.ProductName);
        Assert.Contains(loadedProducts, p => p.ProductName == product2.ProductName);
    }

}

