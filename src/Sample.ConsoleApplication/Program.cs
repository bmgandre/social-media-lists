using ConsoleTableExt;
using Elasticsearch.Net;
using Microsoft.EntityFrameworkCore;
using Nest;
using SocialMediaLists.Application.Contracts.Common.Models;
using SocialMediaLists.Application.Contracts.Posts.Models;
using SocialMediaLists.Application.Posts.Queries;
using SocialMediaLists.Application.Posts.Validators;
using SocialMediaLists.Persistence.ElasticSearch.Common.Database;
using SocialMediaLists.Persistence.ElasticSearch.Posts.Repositories;
using SocialMediaLists.Persistence.EntityFramework.Common.Database;
using SocialMediaLists.Persistence.EntityFramework.People.Repositories;
using SocialMediaLists.Persistence.EntityFramework.SocialLists.Repositories;
using SocialMediaLists.Sample.ConsoleApplication.Data;
using SocialMediaLists.Sample.ConsoleApplication.Logging;
using SocialMediaLists.Sample.ConsoleApplication.Models;
using SocialMediaLists.Sample.Data.Common;
using SocialMediaLists.Sample.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SocialMediaLists.Sample.ConsoleApplication
{
    internal class Program
    {
        private static async Task Main()
        {
            using var pool = new SingleNodeConnectionPool(new Uri("http://localhost:9200"));
            using var connectionSettings = new ConnectionSettings(pool)
                .PrettyJson()
                .DefaultIndex(nameof(SocialMediaLists).ToLower())
                .DisableDirectStreaming()
                .OnRequestCompleted(ElasticSearchLogHandler.Handle);
            var elasticClient = new ElasticClient(connectionSettings);

            using var dbContext = new SocialMediaListsContextFactory().CreateDbContext(null);

            var cancellationToken = CancellationToken.None;
            await UpdateDatabaseAsync(dbContext, elasticClient, cancellationToken);

            var seedSource = new DataSeedCollection();
            await SeedDataAsync(dbContext, elasticClient, seedSource, cancellationToken);

            var queryTerm = GenerateQueryTermFromSample(seedSource);
            var filter = GenerateSearchPostRequest(queryTerm);

            var result = await GetPostQueryService(dbContext, elasticClient)
                .SearchAsync(filter, cancellationToken);
            PrintPostSearchResponse(queryTerm, result);
        }

        private static async Task UpdateDatabaseAsync(DbContext dbContext,
            IElasticClient elasticClient,
            CancellationToken cancellationToken)
        {
            await new IndexManager(elasticClient).CreateIndexesAsync(cancellationToken);
            dbContext.Database.Migrate();
        }

        private static async Task SeedDataAsync(SocialMediaListsDbContext dbContext,
            IElasticClient elasticClient,
            DataSeedCollection seedCollection,
            CancellationToken cancellationToken)
        {
            var dataSeeder = new SampleDataSeeder(elasticClient, dbContext, seedCollection);
            await dataSeeder.SeedAsync(cancellationToken);
        }

        private static string GenerateQueryTermFromSample(DataSeedCollection seedSource)
        {
            char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
            var word = seedSource.Posts.GetSample()
                .Take(10)
                .SelectMany(x => x.Content.Split(delimiterChars))
                .First(x => x.Length > 5);
            return word;
        }

        private static PostSearchRequest GenerateSearchPostRequest(string queryTerm)
        {
            var network = new string[] { "facebook", "twitter" }[new Random().Next(0, 2)];
            return new PostSearchRequest
            {
                DateRange = new DateRangeModel
                {
                    Begin = DateTime.Now.Subtract(TimeSpan.FromDays(365)),
                    End = DateTime.Now
                },
                Content = queryTerm,
                Network = network,
                Page = new PageModel
                {
                    From = 0,
                    Size = 100
                }
            };
        }

        private static PostQuery GetPostQueryService(DbContext dbContext,
            IElasticClient elasticClient)
        {
            var readSocialListsRepository = new ReadSocialListsRepository(dbContext);
            var readPeopleRepository = new ReadPeopleRepository(dbContext);
            var postSearchValidator = new PostSearchRequestValidator(readSocialListsRepository, readPeopleRepository);
            var postRepository = new ReadPostRepository(elasticClient);

            var postQuery = new PostQuery(postRepository, readSocialListsRepository, postSearchValidator);

            return postQuery;
        }

        private static void PrintPostSearchResponse(string queryTerm,
            IEnumerable<PostSearchResponse> responses)
        {
            Console.WriteLine();
            Console.WriteLine($"Searching for: {queryTerm}");
            Console.WriteLine();

            ConsoleTableBuilder
                .From(ReduceTextContent(responses, queryTerm).Take(10).ToDataTable())
                .WithFormat(ConsoleTableBuilderFormat.MarkDown)
                .ExportAndWriteLine();

            Console.WriteLine($"Results: {responses.Count()}");
        }

        private static IEnumerable<FlatPostSearchResponseModel> ReduceTextContent(IEnumerable<PostSearchResponse> postSearchResponse,
            string queryTerm)
        {
            var result = postSearchResponse.Select(response =>
            {
                var tokenIndex = response.Content.IndexOf(queryTerm);
                var startIndex = tokenIndex - 15 > 0 ? tokenIndex - 15 : 0;
                var reducedContent = response.Content.Substring(startIndex, 35);

                var linkWithoutProtocol = response.Link.Replace("https://", string.Empty);
                var reducedLink = linkWithoutProtocol.Length > 35 ? $"{linkWithoutProtocol.Substring(0, 32)}..." : linkWithoutProtocol;

                return new FlatPostSearchResponseModel
                {
                    Date = response.Date,
                    Network = response.Network,
                    Link = reducedLink,
                    Content = reducedContent,
                    Account = response.Account,
                    Lists = string.Join(",", response.Lists)
                };
            });

            return result;
        }
    }
}