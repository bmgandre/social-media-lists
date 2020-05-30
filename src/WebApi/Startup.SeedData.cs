using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using SocialMediaLists.Persistence.EntityFramework.Common.Database;
using SocialMediaLists.Sample.Data.Common;
using SocialMediaLists.Sample.Data.Database;

namespace SocialMediaLists.WebApi
{
    public partial class Startup
    {
        public void ConfigureSeedData(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            using var dbContext = serviceScope.ServiceProvider.GetService<SocialMediaListsDbContext>();
            var elasticClient = serviceScope.ServiceProvider.GetService<IElasticClient>();

            var seedSource = new DataSeedCollection();
            var dataSeeder = new SampleDataSeeder(elasticClient, dbContext, seedSource);
            dataSeeder.Seed();
        }
    }
}