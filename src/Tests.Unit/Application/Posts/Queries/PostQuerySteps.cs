using SocialMediaLists.Application.Contracts.Posts.Models;
using TechTalk.SpecFlow;
using Moq;
using System;
using FluentAssertions;
using SocialMediaLists.Application.Posts.Queries;
using Nest;
using SocialMediaLists.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace SocialMediaLists.Tests.Unit.Application.Posts.Queries.UnitTests
{
    [Binding]
    public class PostQueryTests
    {
        private ScenarioContext _scenarioContext;
        private PostFilter _postFilter;
        private Mock<PostQuery> _mockPostQuery;
        private Mock<IElasticClient> _mockElasticClient;

        public PostQueryTests(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _mockElasticClient = new Mock<IElasticClient>();

            var mockSearchResponse = new Mock<ISearchResponse<Post>>();
            mockSearchResponse.Setup(x => x.Documents).Returns(new List<Post>());

            _mockElasticClient
                .Setup(x => x.SearchAsync<Post>(It.IsAny<SearchRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockSearchResponse.Object);

            _mockPostQuery = new Mock<PostQuery>(_mockElasticClient.Object);
        }

        [Given(@"I have a request for searching posts")]
        public void given_i_have_a_request_for_searching_posts()
        {
        }

        [Given(@"no data is provided for filtering the posts")]
        public void given_no_data_is_provided_for_filtering_the_posts()
        {
            _postFilter = new PostFilter();
        }

        [When(@"I search the posts")]
        public async void when_i_search_the_posts()
        {
            try
            {
                await _mockPostQuery.Object.SearchAsync(_postFilter);
            }
            catch (Exception ex)
            {
                _scenarioContext.Add("Exception", ex);
            }
        }

        [Then(@"the post repository should be reached")]
        public void then_the_post_repository_should_be_reached()
        {
            _mockElasticClient.Verify(mock => mock.SearchAsync<Post>(It.IsAny<SearchRequest>(), It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}