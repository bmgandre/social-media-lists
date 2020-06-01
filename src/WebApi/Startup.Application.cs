﻿using Microsoft.Extensions.DependencyInjection;
using SocialMediaLists.Application.Contracts.People.Queries;
using SocialMediaLists.Application.Contracts.People.Repositories;
using SocialMediaLists.Application.Contracts.People.Validators;
using SocialMediaLists.Application.Contracts.Posts.Queries;
using SocialMediaLists.Application.Contracts.Posts.Repositories;
using SocialMediaLists.Application.Contracts.Posts.Validators;
using SocialMediaLists.Application.Contracts.SocialLists.Queries;
using SocialMediaLists.Application.Contracts.SocialLists.Repositories;
using SocialMediaLists.Application.People.Queries;
using SocialMediaLists.Application.People.Validators;
using SocialMediaLists.Application.Posts.Queries;
using SocialMediaLists.Application.Posts.Validators;
using SocialMediaLists.Application.SocialLists.Queries;
using SocialMediaLists.Persistence.ElasticSearch.Posts.Repositories;
using SocialMediaLists.Persistence.EntityFramework.People.Repositories;
using SocialMediaLists.Persistence.EntityFramework.SocialLists.Repositories;

namespace SocialMediaLists.WebApi
{
    public partial class Startup
    {
        private void ConfigureApplicationServices(IServiceCollection services)
        {
            services.AddScoped<IReadSocialListsRepository, ReadSocialListsRepository>();

            services.AddScoped<IReadPeopleRepository, ReadPeopleRepository>();
            services.AddScoped<IPeopleFindValidators, PeopleFindValidators>();
            services.AddScoped<IPeopleQuery, PeopleQuery>();

            services.AddScoped<IReadPostRepository, ReadPostRepository>();
            services.AddScoped<IPostSearchRequestValidator, PostSearchRequestValidator>();
            services.AddScoped<IPostQuery, PostQuery>();

            services.AddScoped<IReadSocialListsRepository, ReadSocialListsRepository>();
            services.AddScoped<ISocialListsQuery, SocialListsQuery>();
        }
    }
}