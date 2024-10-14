using Shared.Interfaces;
using Shared.Models;
namespace Shared.Services;

public class FileService : IFileService
{
    private readonly string _filePath;

    public FileService(string filePath)
    {
        _filePath = filePath;
    }

    public ServiceResponse SaveToFile(string content)
    {
        try
        {
            using var sw = new StreamWriter(_filePath);
            sw.WriteLine(content);
            return new ServiceResponse
            {
                Succeeded = true,
                Message = "File created successfully"
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

    public ServiceResponse GetFromFile()
    {
        try
        {
            if (File.Exists(_filePath))
            {
                using var sr = new StreamReader(_filePath);
                var content = sr.ReadToEnd();
                return new ServiceResponse
                {
                    Succeeded = true,
                    Message = "File read successfully",
                    Result = content
                };
            }
            else
            {
                return new ServiceResponse
                {
                    Succeeded = false,
                    Message = "File does not exist"
                };
            }
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
