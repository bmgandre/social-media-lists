using Elasticsearch.Net;
using Nest;
using Newtonsoft.Json;
using SocialMediaLists.Tests.Integration.Persistence.ElasticSearch.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocialMediaLists.Tests.Integration.Persistence.ElasticSearch.Common.Database
{
    internal class MockElasticClient<T>
    {
        public ElasticClient ElasticClient { get; private set; }

        public MockElasticClient(IEnumerable<BaseIndexedEntry<T>> source, string index)
        {
            var response = BuildReponse(source, index);
            var responseBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));

            var connection = new InMemoryConnection(responseBytes, 200);
            var connectionPool = new SingleNodeConnectionPool(new Uri("http://localhost:9200"));

            var connectionSettings = new ConnectionSettings(connectionPool, connection);
            var settings = connectionSettings.DefaultIndex(index);

            ElasticClient = new ElasticClient(settings);
        }

        private static object BuildReponse(IEnumerable<BaseIndexedEntry<T>> source, string index)
        {
            var response = new
            {
                took = 1,
                timed_out = false,
                _shards = new
                {
                    total = 2,
                    successful = 2,
                    failed = 0
                },
                hits = new
                {
                    total = new { value = source.Count() },
                    max_score = 1.0,
                    hits = source.Select(entity => new
                    {
                        _index = index,
                        _type = "_doc",
                        _id = entity.id.ToString(),
                        _score = 1.0,
                        _source = entity.source
                    }).ToArray()
                }
            };
            return response;
        }
    }
}