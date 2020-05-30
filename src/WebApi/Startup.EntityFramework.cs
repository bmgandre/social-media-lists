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
    }
}