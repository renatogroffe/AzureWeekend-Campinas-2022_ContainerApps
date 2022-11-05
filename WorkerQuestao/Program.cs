using WorkerQuestao;
using WorkerQuestao.Data;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.FeatureManagement;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSingleton<VotacaoRepository>();
        services.AddHostedService<Worker>();

        services.AddFeatureManagement(
            hostContext.Configuration.GetSection("FeatureFlags"));

        services.AddApplicationInsightsTelemetryWorkerService(options =>
            {
                options.ConnectionString =
                    hostContext.Configuration.GetConnectionString("ApplicationInsights");
            });

        if (Convert.ToBoolean(hostContext.Configuration["FeatureFlags:Monitoring"]))
            services.ConfigureTelemetryModule<DependencyTrackingTelemetryModule>(
                (module, o) =>
                {
                    module.EnableSqlCommandTextInstrumentation = true;
                });
    })
    .Build();

await host.RunAsync();