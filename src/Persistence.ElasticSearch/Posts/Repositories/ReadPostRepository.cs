using Nest;
using SocialMediaLists.Application.Contracts.Posts.Models;
using SocialMediaLists.Application.Contracts.Posts.Repositories;
using SocialMediaLists.Domain.Posts;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SocialMediaLists.Persistence.ElasticSearch.Posts.Repositories
{
    public class ReadPostRepository : IReadPostRepository
    {
        private readonly IElasticClient _elasticClient;

        public ReadPostRepository(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task<IEnumerable<Post>> SearchAsync(PostFilter filter,
            CancellationToken cancellationToken)
        {
            var query = filter.CreateQuery();
            var searchRequest = new SearchRequest
            {
                From = filter.Page?.From ?? 0,
                Size = filter.Page?.Size ?? 100,
                Query = query
            };

            var result = await _elasticClient.SearchAsync<Post>(searchRequest, cancellationToken);
            return result.Documents;
        }
    }
}