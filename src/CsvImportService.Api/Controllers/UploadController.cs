using CsvImportService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CsvImportService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UploadController : ControllerBase
{
    private readonly ICsvJobProcessor _processor;

    public UploadController(ICsvJobProcessor processor)
    {
        _processor = processor;
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        // Convert to stream here
        using var stream = file.OpenReadStream();
        string jobId = await _processor.ProcessCsvAsync(stream);

        return Ok(new { jobId, message = "CSV queued for validation." });
    }
}
