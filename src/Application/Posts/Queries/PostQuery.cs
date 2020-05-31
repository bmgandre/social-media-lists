using SocialMediaLists.Application.Common.Data;
using SocialMediaLists.Application.Contracts.Posts.Models;
using SocialMediaLists.Application.Contracts.Posts.Queries;
using SocialMediaLists.Application.Contracts.Posts.Repositories;
using SocialMediaLists.Application.Contracts.Posts.Validators;
using SocialMediaLists.Application.Contracts.SocialLists.Repositories;
using SocialMediaLists.Application.SocialLists.Specifications;
using SocialMediaLists.Domain.SocialLists;
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
            var filter = await CreatePostFilterAsync(request, cancellationToken);
            var result = await _readPostRepository.SearchAsync(filter, cancellationToken);
            return result.Select(x => new PostSearchResponse
            {
                Date = x.Date,
                Content = x.Content,
                Link = x.Link,
                Network = x.Network
            });
        }

        private async Task<PostFilter> CreatePostFilterAsync(PostSearchRequest request,
            CancellationToken cancellationToken)
        {
            var authors = await LoadAuthorsAsync(request, cancellationToken);

            return new PostFilter
            {
                Text = request.Text,
                Authors = authors,
                Network = request.Network,
                DateRange = request.DateRange,
                Page = request.Page
            };
        }

        private async Task<IEnumerable<string>> LoadAuthorsAsync(PostSearchRequest request,
            CancellationToken cancellationToken)
        {
            var authors = new List<string>();

            if (request.Lists != null && request.Lists.Any())
            {
                var specification = SpecificationBuilder<SocialList>.Create()
                    .WithNames(request.Lists);

                var accounts = await _readSocialListsRepository.SearchAndProjectAsync(specification,
                    s => s.SocialListPerson,
                    sp => sp.Person,
                    a => a.Accounts,
                    s => s.SocialListPerson.SelectMany(list => list.Person.Accounts),
                    cancellationToken);

                var loaded = accounts.Select(account => account.AccountName);
                authors.AddRange(loaded);
            }

            return authors;
        }
    }
}