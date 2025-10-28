using CsvHelper;
using CsvHelper.Configuration;
using CsvImportService.Application.Interfaces;
using CsvImportService.Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text;

namespace CsvImportService.Infrastructure.Services;

public class CsvJobProcessor : ICsvJobProcessor
{
    private readonly KafkaProducerService _kafkaProducer;
    private readonly ILogger<CsvJobProcessor> _logger;
        
    public CsvJobProcessor(KafkaProducerService kafkaProducer, ILogger<CsvJobProcessor> logger)
    {
        _kafkaProducer = kafkaProducer;
        _logger = logger;
    }

    public async Task<string> ProcessCsvAsync(Stream csvStream)
    {
        using var reader = new StreamReader(csvStream, Encoding.UTF8);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HeaderValidated = null,
            MissingFieldFound = null
        });

        var students = csv.GetRecords<Student>().ToList();

        foreach (var student in students)
        {
            await _kafkaProducer.ProduceAsync(student); // Send to Kafka topic
            _logger.LogInformation($"Published to Kafka: {student.Name} | {student.Email}");
        }

        return $"{students.Count} records published to Kafka.";
    }
}
