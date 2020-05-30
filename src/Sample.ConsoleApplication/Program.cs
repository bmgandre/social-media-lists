using Elasticsearch.Net;
using Microsoft.EntityFrameworkCore;
using Nest;
using SocialMediaLists.Application.Contracts.Common.Models;
using SocialMediaLists.Application.Contracts.Posts.Models;
using SocialMediaLists.Application.Posts.Queries;
using SocialMediaLists.Application.Posts.Validators;
using SocialMediaLists.Domain.Posts;
using SocialMediaLists.Persistence.ElasticSearch.Posts.Repositories;
using SocialMediaLists.Persistence.EntityFramework.Common.Database;
using SocialMediaLists.Persistence.EntityFramework.SocialLists.Repositories;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestEnvironment.Docker;
using TestEnvironment.Docker.Containers.Elasticsearch;

namespace SocialMediaLists.Sample.ConsoleApplication
{
    internal class Program
    {
        private static async Task Main()
        {
            Console.WriteLine("Start");

            using (var pool = new SingleNodeConnectionPool(new Uri("http://localhost:9200")))
            using (var connectionSettings = DefaultConnectionSettings(pool))
            {
                var client = new ElasticClient(connectionSettings);
                await client.Indices.CreateAsync(nameof(Post).ToLower(), c => c
                    .Map<Post>(m => m
                        .AutoMap()
                        .Properties(ps => ps.Keyword(k => k.Name(n => n.Network)))
                        .Properties(ps => ps.Keyword(k => k.Name(n => n.Link)))
                    ));

                await client.IndexAsync(new Post
                {
                    Date = DateTime.Now.Subtract(TimeSpan.FromHours(1)),
                    Network = "Facebook",
                    Content = "The term query looks for the exact term in the field’s inverted index"
                }, idx => idx.Index(nameof(Post).ToLower())
                );

                var options = new DbContextOptionsBuilder<SocialMediaListsDbContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .Options;
                var dbContext = new SocialMediaListsDbContext(options);

                var readSocialListsRepository = new ReadSocialListsRepository(dbContext);
                var validator = new PostSearchRequestValidator(readSocialListsRepository);
                var postRepository = new ReadPostRepository(client);
                var postQuery = new PostQuery(postRepository, validator);

                var filter = new PostSearchRequest
                {
                    DateRange = new DateRangeModel
                    {
                        Begin = DateTime.Now.Subtract(TimeSpan.FromDays(1)),
                        End = DateTime.Now
                    },
                    Text = "looks inverted",
                    Network = "Facebook",
                    Page = new PageModel
                    {
                        From = 0,
                        Size = 100
                    }
                };
                var result = await postQuery.SearchAsync(filter, CancellationToken.None);
                Console.WriteLine($"Result count: {result.Count()}");
            }

            Console.WriteLine("End");
        }

        private static (DockerEnvironment, ElasticsearchContainer) CreateEnv()
        {
            var environment = new DockerEnvironmentBuilder()
                .AddElasticsearchContainer("my-elastic")
                .Build();

            var elastic = environment.GetContainer<ElasticsearchContainer>("my-elastic");

            return (environment, elastic);
        }

        private static ConnectionSettings DefaultConnectionSettings(IConnectionPool pool)
        {
            return new ConnectionSettings(pool, new InMemoryConnection())
                .PrettyJson()
                .DefaultIndex("SocialMediaLists".ToLower())
                .DisableDirectStreaming()
                .OnRequestCompleted(response =>
                {
                    // log out the request
                    if (response.RequestBodyInBytes != null)
                    {
                        Console.WriteLine(
                            $"{response.HttpMethod} {response.Uri} \n" +
                            $"{Encoding.UTF8.GetString(response.RequestBodyInBytes)}");
                    }
                    else
                    {
                        Console.WriteLine($"{response.HttpMethod} {response.Uri}");
                    }

                    // log out the response
                    if (response.ResponseBodyInBytes != null)
                    {
                        Console.WriteLine($"Status: {response.HttpStatusCode}\n" +
                                 $"{Encoding.UTF8.GetString(response.ResponseBodyInBytes)}\n" +
                                 $"{new string('-', 30)}\n");
                    }
                    else
                    {
                        Console.WriteLine($"Status: {response.HttpStatusCode}\n" +
                                 $"{new string('-', 30)}\n");
                    }
                });
        }
    }
}