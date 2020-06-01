using Nest;
using SocialMediaLists.Domain.Posts;
using System.Threading;
using System.Threading.Tasks;

namespace SocialMediaLists.Persistence.ElasticSearch.Common.Database
{
    public class IndexManager
    {
        private readonly IElasticClient _elasticClient;
        private readonly string IndexName = nameof(Post).ToLower();

        public IndexManager(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task CreateIndexesAsync(CancellationToken cancellationToken)
        {
            var existsResponse = await _elasticClient.Indices.ExistsAsync(IndexName, ct: cancellationToken);

            if (!existsResponse.Exists)
            {
                await _elasticClient.Indices
                    .CreateAsync(IndexName, index => CreatePostIndex(index), cancellationToken);
            }
        }

        private static CreateIndexDescriptor CreatePostIndex(CreateIndexDescriptor index)
        {
            return index.Map<Post>(mapping =>
            {
                return mapping.AutoMap()
                    .Properties(property => property.Keyword(key => key.Name(post => post.Network)))
                    .Properties(property => property.Keyword(key => key.Name(post => post.Link)))
                    .Properties(property => property.Keyword(key => key.Name(post => post.Lists)))
                    .Properties(property => property.Keyword(key => key.Name(post => post.Author)));
            });
        }

        public void CreateIndexes()
        {
            var existsResponse = _elasticClient.Indices.Exists(IndexName);

            if (!existsResponse.Exists)
            {
                _elasticClient.Indices
                    .Create(IndexName, index => CreatePostIndex(index));
            }
        }
    }
}