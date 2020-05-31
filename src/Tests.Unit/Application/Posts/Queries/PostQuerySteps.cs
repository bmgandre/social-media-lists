using Moq;
using SocialMediaLists.Application.Contracts.Posts.Models;
using SocialMediaLists.Application.Contracts.Posts.Repositories;
using SocialMediaLists.Application.Contracts.Posts.Validators;
using SocialMediaLists.Application.Contracts.SocialLists.Repositories;
using SocialMediaLists.Application.Posts.Queries;
using SocialMediaLists.Domain.Posts;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SocialMediaLists.Tests.Unit.Application.Posts.Queries
{
    [Binding]
    internal class PostQuerySteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly Mock<IReadPostRepository> _readPostRepository;
        private readonly Mock<IPostSearchRequestValidator> _postFilterValidator;
        private readonly Mock<IReadSocialListsRepository> _readSocialListRepository;
        private readonly PostQuery _postQuery;
        private PostSearchRequest _postFilter;

        public PostQuerySteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;

            _readPostRepository = new Mock<IReadPostRepository>();
            _readPostRepository
                .Setup(x => x.SearchAsync(It.IsAny<PostFilter>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Post>());

            _readSocialListRepository = new Mock<IReadSocialListsRepository>();

            _postFilterValidator = new Mock<IPostSearchRequestValidator>();
            _postFilterValidator
                .Setup(x => x.ValidateAndThrowAsync(It.IsAny<PostSearchRequest>(), It.IsAny<CancellationToken>()));

            _postQuery = new PostQuery(_readPostRepository.Object, _readSocialListRepository.Object, _postFilterValidator.Object);
        }

        [Given(@"I have a request for searching posts")]
        public void given_i_have_a_request_for_searching_posts()
        {
        }

        [Given(@"no data is provided for filtering the posts")]
        public void given_no_data_is_provided_for_filtering_the_posts()
        {
            _postFilter = new PostSearchRequest();
        }

        [When(@"I search the posts")]
        public async Task when_i_search_the_posts()
        {
            try
            {
                await _postQuery.SearchAsync(_postFilter, CancellationToken.None);
            }
            catch (Exception ex)
            {
                _scenarioContext.Add("Exception", ex);
            }
        }

        [Then(@"the post repository should be reached")]
        public void then_the_post_repository_should_be_reached()
        {
            _readPostRepository.Verify(mock => mock.SearchAsync(It.IsAny<PostFilter>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Then(@"the post filter validator should be reached")]
        public void then_the_post_filter_validator_should_be_reached()
        {
            _postFilterValidator.Verify(mock => mock.ValidateAndThrowAsync(It.IsAny<PostSearchRequest>(), It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}