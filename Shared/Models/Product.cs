namespace Shared.Models;

public class Product
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string ProductName { get; set; } = null!;
    public string ProductPrice { get; set; } = null!;
    public Category Category { get; set; } = new Category();


}
