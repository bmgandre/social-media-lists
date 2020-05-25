using SocialMediaLists.Application.Contracts.Posts.Models;
using SocialMediaLists.Application.Contracts.Posts.Queries;
using SocialMediaLists.Application.Contracts.Posts.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaLists.Application.Posts.Queries
{
    public class PostQuery : IPostQuery
    {
        private readonly IReadPostRepository _readPostRepository;
        public PostQuery(IReadPostRepository readPostRepository)
        {
            _readPostRepository = readPostRepository;
        }

        public async Task<IEnumerable<PostModel>> SearchAsync(PostFilter filter)
        {
            var result = await _readPostRepository.SearchAsync(filter);
            return result.Select(x => new PostModel
            {
                Date = x.Date,
                Content = x.Content,
                Link = x.Link,
                Network = x.Network
            });
        }
    }
}