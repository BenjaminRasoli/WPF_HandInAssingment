using Shared.Models;

namespace Shared.Interfaces;

public interface IFileService
{
    ServiceResponse SaveToFile(string content);
    ServiceResponse GetFromFile();
}
