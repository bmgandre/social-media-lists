using Nest;
using SocialMediaLists.Application.Contracts.Posts.Models;
using SocialMediaLists.Application.Contracts.Posts.Repositories;
using SocialMediaLists.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMediaLists.Persistence.ElasticSearch.Posts.Repositories
{
    public class EsReadPostRepository : IReadPostRepository
    {
        private readonly IElasticClient _elasticClient;

        public EsReadPostRepository(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task<IEnumerable<Post>> SearchAsync(PostFilter filter)
        {
            var query = filter.CreateQuery();
            var searchRequest = new SearchRequest
            {
                From = filter.Page?.From,
                Size = filter.Page?.Size,
                Query = query
            };
            var result = await _elasticClient.SearchAsync<Post>(searchRequest);
            return result.Documents;
        }
    }
}