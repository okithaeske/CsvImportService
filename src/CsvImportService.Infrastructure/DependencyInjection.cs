using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvImportService.Application.Interfaces;
using CsvImportService.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Confluent.Kafka;
using StackExchange.Redis;

namespace CsvImportService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var kafkaConfig = new ProducerConfig
        {
            BootstrapServers = config["Kafka:BootstrapServers"]
        };

        services.AddSingleton(kafkaConfig);

        // Register Kafka producer service so CsvJobProcessor can be constructed
        services.AddSingleton<KafkaProducerService>();

        services.AddScoped<ICsvJobProcessor, CsvJobProcessor>();

        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(
            config.GetConnectionString("Redis")));

        return services;
    }
}
