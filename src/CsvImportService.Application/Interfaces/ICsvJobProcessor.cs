using System.IO;

namespace CsvImportService.Application.Interfaces;

public interface ICsvJobProcessor
{
    Task<string> ProcessCsvAsync(Stream csvStream);
}
