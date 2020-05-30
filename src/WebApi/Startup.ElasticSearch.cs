using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using SocialMediaLists.WebApi.Settings;
using System;

namespace SocialMediaLists.WebApi
{
    public partial class Startup
    {
        private void ConfigureElasticSearchServices(IServiceCollection services)
        {
            services.AddScoped<IConnectionPool, SingleNodeConnectionPool>((provider) =>
            {
                var settings = Configuration.GetSection(SocialMediaListSettings.SectionName)
                    .Get<SocialMediaListSettings>();
                return new SingleNodeConnectionPool(new Uri(settings.ElasticSearch.Url));
            });
            services.AddScoped<IConnectionSettingsValues, ConnectionSettings>((provider) =>
            {
                var pool = provider.GetService<IConnectionPool>();
                var connectionSettings = new ConnectionSettings(pool)
                    .DefaultIndex(nameof(SocialMediaLists).ToLower());
                return connectionSettings;
            });
            services.AddScoped<IElasticClient, ElasticClient>();
        }
    }
}