using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SocialMediaLists.Persistence.EntityFramework.Common.Database;

namespace SocialMediaLists.WebApi
{
    public partial class Startup
    {
        private void ConfigureEntityFrameworkServices(IServiceCollection services)
        {
            services.AddScoped<DbContext>((provider) => provider.GetService<SocialMediaListsDbContext>());
            services.AddScoped((provider) => new SocialMediaListsContextFactory().CreateDbContext(null));
        }

        public void ConfigureEntityFramework(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using var dbContext = new SocialMediaListsContextFactory().CreateDbContext(null);
            dbContext.Database.Migrate();
        }
    }
}