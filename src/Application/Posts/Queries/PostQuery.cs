using SocialMediaLists.Application.Contracts.Posts.Models;
using SocialMediaLists.Application.Contracts.Posts.Queries;
using SocialMediaLists.Application.Contracts.Posts.Repositories;
using SocialMediaLists.Application.Contracts.Posts.Validators;
using SocialMediaLists.Application.Contracts.SocialLists.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SocialMediaLists.Application.Posts.Queries
{
    public class PostQuery : IPostQuery
    {
        private readonly IReadSocialListsRepository _readSocialListsRepository;
        private readonly IReadPostRepository _readPostRepository;
        private readonly IPostSearchRequestValidator _postFilterValidator;

        public PostQuery(IReadPostRepository readPostRepository,
            IReadSocialListsRepository readSocialListsRepository,
            IPostSearchRequestValidator postFilterValidator)
        {
            _readPostRepository = readPostRepository;
            _postFilterValidator = postFilterValidator;
            _readSocialListsRepository = readSocialListsRepository;
        }

        public async Task<IEnumerable<PostSearchResponse>> SearchAsync(PostSearchRequest request,
            CancellationToken cancellationToken)
        {
            await _postFilterValidator.ValidateAndThrowAsync(request, cancellationToken);
            var filter = CreatePostFilter(request);
            var result = await _readPostRepository.SearchAsync(filter, cancellationToken);
            return result.Select(x => new PostSearchResponse
            {
                Date = x.Date,
                Content = x.Content,
                Link = x.Link,
                Network = x.Network,
                Account = x.Author,
                Lists = x.Lists,
            });
        }

        private PostFilter CreatePostFilter(PostSearchRequest request)
        {
            return new PostFilter
            {
                Text = request.Text,
                Authors = request.AccountNames,
                Lists = request.Lists,
                Network = request.Network,
                DateRange = request.DateRange,
                Page = request.Page
            };
        }
    }
}