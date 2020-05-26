using Application.Contracts.Common.Models;
using Elasticsearch.Net;
using Nest;
using SocialMediaLists.Application.Contracts.Posts.Models;
using SocialMediaLists.Application.Posts.Queries;
using SocialMediaLists.Domain;
using SocialMediaLists.Persistence.ElasticSearch.Posts.Repositories;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestEnvironment.Docker;
using TestEnvironment.Docker.Containers.Elasticsearch;

namespace Sample.ConsoleApplication
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine("Start");

            var client = GetElasticClient();
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

            var postRepository = new EsReadPostRepository(client);
            var postQuery = new PostQuery(postRepository);
            var filter = new PostFilter
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

        private static ElasticClient GetElasticClient()
        {
            var pool = new SingleNodeConnectionPool(new Uri("http://localhost:9200"));
            var connectionSettings = DefaultConnectionSettings(pool);
            return new ElasticClient(connectionSettings);
        }

        private static ConnectionSettings DefaultConnectionSettings(IConnectionPool pool)
        {
            return new ConnectionSettings(pool)
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