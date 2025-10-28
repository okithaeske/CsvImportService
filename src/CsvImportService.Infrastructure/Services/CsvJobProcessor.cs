using CsvImportService.Application.Interfaces;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CsvImportService.Infrastructure.Services;

public class CsvJobProcessor : ICsvJobProcessor
{
    public async Task<string> ProcessCsvAsync(Stream csvStream)
    {
        // Just for now – reads the CSV content as text (later we'll push to Kafka)
        using var reader = new StreamReader(csvStream, Encoding.UTF8);
        string csvContent = await reader.ReadToEndAsync();

        // Simulate a job ID being created and queued (to be replaced with Kafka later)
        string jobId = Guid.NewGuid().ToString();

        Console.WriteLine($"[CsvJobProcessor] Queued job: {jobId}");
        Console.WriteLine(csvContent); // For debug – remove in prod

        return jobId;
    }
}   
