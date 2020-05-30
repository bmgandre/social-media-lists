using Nest;
using SocialMediaLists.Domain.Posts;
using System.Threading;
using System.Threading.Tasks;

namespace SocialMediaLists.Persistence.ElasticSearch.Common.Database
{
    public class IndexManager
    {
        private readonly IElasticClient _elasticClient;

        public IndexManager(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task CreateIndexesAsync(CancellationToken cancellationToken)
        {
            var indexName = nameof(Post).ToLower();
            var existsResponse = await _elasticClient.Indices.ExistsAsync(indexName, ct: cancellationToken);

            if (!existsResponse.Exists)
            {
                await _elasticClient.Indices
                    .CreateAsync(indexName, index =>
                    {
                        return index.Map<Post>(mapping =>
                        {
                            return mapping.AutoMap()
                                .Properties(property => property.Keyword(key => key.Name(post => post.Network)))
                                .Properties(property => property.Keyword(key => key.Name(post => post.Link)));
                        });
                    }, cancellationToken);
            }
        }
    }
}