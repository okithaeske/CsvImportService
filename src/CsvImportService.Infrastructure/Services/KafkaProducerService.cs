using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading.Tasks;

namespace CsvImportService.Infrastructure.Services
{
    public class KafkaProducerService
    {
        private readonly string _topic;
        private readonly IProducer<Null, string> _producer;
        private readonly ILogger<KafkaProducerService> _logger;

        public KafkaProducerService(IConfiguration config, ILogger<KafkaProducerService> logger)
        {
            _topic = config["Kafka:Topic"];
            _logger = logger;

            var configProducer = new ProducerConfig
            {
                BootstrapServers = config["Kafka:BootstrapServers"]
            };

            _producer = new ProducerBuilder<Null, string>(configProducer).Build();
        }

        public async Task ProduceAsync<T>(T message)
        {
            try
            {
                var json = JsonSerializer.Serialize(message);
                await _producer.ProduceAsync(_topic, new Message<Null, string> { Value = json });

                _logger.LogInformation($"Produced message to Kafka: {json}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Kafka produce failed: {ex.Message}");
            }
        }
    }
}
