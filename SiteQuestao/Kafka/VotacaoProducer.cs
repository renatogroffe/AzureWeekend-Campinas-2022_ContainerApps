using System.Diagnostics;
using System.Text.Json;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Confluent.Kafka;
using SiteQuestao.Models;

namespace SiteQuestao.Kafka;

public class VotacaoProducer
{
    private readonly ILogger<VotacaoProducer> _logger;
    private readonly IConfiguration _configuration;
    private readonly TelemetryConfiguration _telemetryConfig;
    private readonly JsonSerializerOptions _serializerOptions;

    public VotacaoProducer(
        ILogger<VotacaoProducer> logger,
        IConfiguration configuration,
        TelemetryConfiguration telemetryConfig)
    {
        _logger = logger;
        _configuration = configuration;
        _telemetryConfig = telemetryConfig;
        _serializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task Send(string tecnologia)
    {
        using (var producer = CreateProducer())
        {
            var idVoto = Guid.NewGuid().ToString();
            var horario = $"{DateTime.UtcNow.AddHours(-3):yyyy-MM-dd HH:mm:ss}";

            await SendEventDataAsync<Voto>(producer,
                new()
                {
                    IdVoto = idVoto,
                    Horario = horario,
                    Tecnologia = tecnologia,
                    Producer = Environment.MachineName
                });
        }

        _logger.LogInformation("Concluido o envio dos eventos!");
    }

    private async Task SendEventDataAsync<T>(IProducer<Null, string> producer, T eventData)
    {
        var start = DateTime.Now;
        var watch = new Stopwatch();
        watch.Start();

        string topic = _configuration["ApacheKafka:Topic"];

        string data = JsonSerializer.Serialize(eventData, _serializerOptions);
        _logger.LogInformation($"Evento: {data}");

        var result = await producer.ProduceAsync(
            topic,
            new Message<Null, string>
            { Value = data });

        _logger.LogInformation(
            $"Apache Kafka - Envio para o tópico {topic} concluído | " +
            $"{data} | Status: { result.Status.ToString()}");

        watch.Stop();
        TelemetryClient client = new(_telemetryConfig);
        client.TrackDependency(
            "Kafka", $"Produce {topic}", data, start, watch.Elapsed, true);
    }

    private IProducer<Null, string> CreateProducer()
    {
        var password = _configuration["ApacheKafka:Password"];
        if (!String.IsNullOrWhiteSpace(password))
            return new ProducerBuilder<Null, string>(
                new ProducerConfig()
                {
                    BootstrapServers = _configuration["ApacheKafka:Host"],
                    SecurityProtocol = SecurityProtocol.SaslSsl,
                    SaslMechanism = SaslMechanism.Plain,
                    SaslUsername = _configuration["ApacheKafka:Username"],
                    SaslPassword = password
                }).Build();
        else
            return new ProducerBuilder<Null, string>(
                new ProducerConfig()
                {
                    BootstrapServers = _configuration["ApacheKafka:Host"]
                }).Build();
    }
}