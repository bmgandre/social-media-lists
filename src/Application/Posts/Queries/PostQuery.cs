using Application.Posts.Queries;
using Nest;
using SocialMediaLists.Application.Contracts.Posts.Models;
using SocialMediaLists.Application.Contracts.Posts.Queries;
using SocialMediaLists.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaLists.Application.Posts.Queries
{
    public class PostQuery : IPostQuery
    {
        private IElasticClient _elasticClient;

        public PostQuery(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task<IEnumerable<PostModel>> SearchAsync(PostFilter filter)
        {
            var query = filter.CreateQuery();
            var searchRequest = new SearchRequest
            {
                From = filter.Page?.From,
                Size = filter.Page?.Size,
                Query = query
            };
            var result = await _elasticClient.SearchAsync<Post>(searchRequest);
            return result.Documents.Select(x => new PostModel
            {
                Date = x.Date,
                Content = x.Content,
                Link = x.Link,
                Network = x.Network
            });
        }
    }
}