namespace Shared.Models;

public class ServiceResponse
{
    public bool Succeeded { get; set; }
    public string? Message { get; set; }
    public object? Result { get; set; }
}
