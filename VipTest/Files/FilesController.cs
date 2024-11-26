using Microsoft.AspNetCore.Mvc;
using VipTest.Files.Serivce;
using VipTest.Utlity.Basic;

namespace VipTest.Files;

[Route("files/")]
public class FilesController : BaseController
{
    private readonly IFileService _fileService;

    public FilesController(IFileService fileService)
    {
        _fileService = fileService;
    }


    [HttpPost("upload-multiple")]
    [Consumes("multipart/form-data")]  // Indicate that this endpoint accepts multipart form data
    public async Task<IActionResult> UploadMultipleFiles([FromForm] IFormFile[] files)
    {
        var savedFiles = await _fileService.SaveFilesAsync(files);
        return Ok(savedFiles);
    }
}