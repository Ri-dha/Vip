using VipProjectV0._1.Db;
using VipTest.Files.Models;

namespace VipTest.Files.Serivce;

public interface IFileService
{
    Task<ProjectFiles> SaveFileAsync(IFormFile file);
    Task<ProjectFiles?> GetFileAsync(Guid fileId);  // Method to retrieve files if needed

    Task<List<ProjectFiles>> SaveFilesAsync(IFormFile[] files);
}
public class FileService : IFileService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly string _fileDirectory;

    public FileService(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _fileDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

        if (!Directory.Exists(_fileDirectory))
        {
            Directory.CreateDirectory(_fileDirectory);
        }
    }

    public async Task<ProjectFiles> SaveFileAsync(IFormFile file)
    {
        if (file == null || file.Length == 0) return null;

        var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
        var filePath = Path.Combine(_fileDirectory, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Create the relative file path (e.g., "uploads\\fileName")
        var relativeFilePath = Path.Combine("uploads", fileName);

        // Create the File entity
        var fileEntity = new ProjectFiles
        {
            FileName = file.FileName,
            FilePath = relativeFilePath,  // Save only the relative path
            FileSize = file.Length,
            ContentType = file.ContentType
        };

        // Save the File entity using the FileRepository
        await _repositoryWrapper.FileRepository.Add(fileEntity);

        return fileEntity;
    }

    public async Task<ProjectFiles?> GetFileAsync(Guid fileId)
    {
        return await _repositoryWrapper.FileRepository.GetById(fileId);
    }

    public async Task<List<ProjectFiles>> SaveFilesAsync(IFormFile[] files)
    {
        var savedFiles = new List<ProjectFiles>();

        foreach (var file in files)
        {
            var savedFile = await SaveFileAsync(file);
            if (savedFile != null)
            {
                savedFiles.Add(savedFile);
            }
        }

        return savedFiles;
    }
}