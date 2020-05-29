using SocialMediaLists.Application.Contracts.Posts.Models;
using SocialMediaLists.Application.Contracts.Posts.Queries;
using SocialMediaLists.Application.Contracts.Posts.Repositories;
using SocialMediaLists.Application.Contracts.Posts.Validators;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SocialMediaLists.Application.Posts.Queries
{
    public class PostQuery : IPostQuery
    {
        private readonly IPostSearchRequestValidator _postFilterValidator;
        private readonly IReadPostRepository _readPostRepository;

        public PostQuery(IReadPostRepository readPostRepository,
            IPostSearchRequestValidator postFilterValidator)
        {
            _readPostRepository = readPostRepository;
            _postFilterValidator = postFilterValidator;
        }

        public async Task<IEnumerable<PostSearchResponse>> SearchAsync(PostSearchRequest filter,
            CancellationToken cancellationToken)
        {
            _postFilterValidator.ValidateAndThrow(filter);
            var result = await _readPostRepository.SearchAsync(filter, cancellationToken);
            return result.Select(x => new PostSearchResponse
            {
                Date = x.Date,
                Content = x.Content,
                Link = x.Link,
                Network = x.Network
            });
        }
    }
}