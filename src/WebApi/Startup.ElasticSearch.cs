using Elasticsearch.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using SocialMediaLists.Persistence.ElasticSearch.Common.Database;
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
                    .PrettyJson()
                    .DefaultIndex(nameof(SocialMediaLists).ToLower())
                    .DisableDirectStreaming();
                return connectionSettings;
            });
            services.AddScoped<IElasticClient, ElasticClient>();
        }

        private void ConfigureElasticSearch(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var elasticClient = serviceScope.ServiceProvider.GetService<IElasticClient>();
            var indexManager = new IndexManager(elasticClient);
            indexManager.CreateIndexes();
        }
    }
}