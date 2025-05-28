using CarRentalSystem.Api.Configurations;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.Elasticsearch;

namespace CarRentalSystem.Api.ServiceRegistration;

public class LoggerRegistration
{
    public static Logger RegisterLogger(ElasticSearchConfigurations configurations)
    {
        var logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(configurations.ElasticSearchUri))
            {
                AutoRegisterTemplate = true,
                IndexFormat = configurations.IndexFormat,
                ModifyConnectionSettings = x => x.BasicAuthentication(configurations.Username, configurations.Password)
            })
            .CreateLogger();
        
        return logger;
    }
}