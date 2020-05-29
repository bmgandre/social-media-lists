using Moq;
using Nest;
using SocialMediaLists.Application.Contracts.Posts.Models;
using SocialMediaLists.Domain.Posts;
using SocialMediaLists.Persistence.ElasticSearch.Posts.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using TechTalk.SpecFlow;

namespace SocialMediaLists.Tests.Unit.Persistence.ElasticSearch.Posts.Repositories
{
    [Binding]
    internal class PostSearchSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly EsReadPostRepository _esReadPostRepository;
        private readonly Mock<IElasticClient> _mockElasticClient;
        private PostSearchRequest _postFilter;

        public PostSearchSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _mockElasticClient = new Mock<IElasticClient>();

            var mockSearchResponse = new Mock<ISearchResponse<Post>>();
            mockSearchResponse.Setup(x => x.Documents).Returns(new List<Post>());

            _mockElasticClient
                .Setup(x => x.SearchAsync<Post>(It.IsAny<SearchRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockSearchResponse.Object);

            _esReadPostRepository = new EsReadPostRepository(_mockElasticClient.Object);
        }

        [Given(@"I have a request for searching posts")]
        [Scope(Tag = "Persistence.ElasticSearch")]
        public void given_i_have_a_request_for_searching_posts()
        {
        }

        [Given(@"no data is provided for filtering the posts")]
        [Scope(Tag = "Persistence.ElasticSearch")]
        public void given_no_data_is_provided_for_filtering_the_posts()
        {
            _postFilter = new PostSearchRequest();
        }

        [When(@"I search the posts")]
        [Scope(Tag = "Persistence.ElasticSearch")]
        public async void when_i_search_the_posts()
        {
            try
            {
                await _esReadPostRepository.SearchAsync(_postFilter, CancellationToken.None);
            }
            catch (Exception ex)
            {
                _scenarioContext.Add("Exception", ex);
            }
        }

        [Then(@"the post repository should be reached")]
        [Scope(Tag = "Persistence.ElasticSearch")]
        public void then_the_post_repository_should_be_reached()
        {
            _mockElasticClient.Verify(mock => mock.SearchAsync<Post>(It.IsAny<SearchRequest>(), It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}