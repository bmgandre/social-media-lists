using FluentAssertions;
using Moq;
using SocialMediaLists.Application.Contracts.Common.Data;
using SocialMediaLists.Application.Contracts.Common.Models;
using SocialMediaLists.Application.Contracts.Common.Validators;
using SocialMediaLists.Application.Contracts.Posts.Models;
using SocialMediaLists.Application.Contracts.SocialLists.Repositories;
using SocialMediaLists.Application.Posts.Validators;
using SocialMediaLists.Domain.SocialLists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SocialMediaLists.Tests.Unit.Application.Posts.Validators
{
    [Binding]
    internal class PostSearchRequestValidatorSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly PostSearchRequestValidator _postFilterValidator;
        private PostSearchRequest _postFilter;
        private List<string> _socialListBackground = new List<string>();

        public PostSearchRequestValidatorSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;

            var mockRepository = new Mock<IReadSocialListsRepository>();
            mockRepository.Setup(x => x.ExistsAsync(It.IsAny<ISpecification<SocialList>>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(_postFilter.Lists.All(x => _socialListBackground.Contains(x))));

            _postFilterValidator = new PostSearchRequestValidator(mockRepository.Object);
        }

        [Given(@"the following lists are registered")]
        public void given_the_following_lists_are_registered(Table table)
        {
            _socialListBackground = table.Rows.SelectMany(x => x.Values).ToList();
        }

        [Given(@"a search post request to validate")]
        public void given_a_search_post_request_to_validate()
        {
            _postFilter = new PostSearchRequest();
        }

        [Given(@"the post filter text is '(.*)'")]
        public void given_the_post_filter_text_is(string text)
        {
            _postFilter.Text = text;
        }

        [Given(@"the post network filter is '(.*)'")]
        public void given_the_post_network_filter_is(string network)
        {
            _postFilter.Network = network;
        }

        [Given(@"the post list filter contains")]
        public void given_the_post_list_filter_contains(Table table)
        {
            var lists = table.Rows.SelectMany(x => x.Values)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();
            _postFilter.Lists = lists;
        }

        [Given(@"the post date filter is \['(.*)', '(.*)']")]
        public void given_the_post_date_filter_is(string begin, string end)
        {
            var dateParser = new Chronic.Parser();
            _postFilter.DateRange = new DateRangeModel
            {
                Begin = string.IsNullOrWhiteSpace(begin) ? default(DateTime?) : dateParser.Parse(begin).ToTime(),
                End = string.IsNullOrWhiteSpace(end) ? default(DateTime?) : dateParser.Parse(end).ToTime()
            };
        }

        [Given(@"the post page filter is \['(.*)', '(.*)']")]
        public void given_the_post_page_filter_is(int? from, int? size)
        {
            _postFilter.Page = new PageModel
            {
                From = from,
                Size = size
            };
        }

        [When(@"the request to search posts with the specified filter is checked")]
        public void when_the_request_to_search_posts_with_the_specified_filter_is_checked()
        {
            try
            {
                _postFilterValidator.ValidateAndThrow(_postFilter);
            }
            catch (Exception ex)
            {
                _scenarioContext.Add("Exception", ex);
            }
        }

        [Then(@"the post filter validation should give no error")]
        public void then_the_post_filter_validation_should_give_no_error()
        {
            _scenarioContext.Keys.Should().NotContain("Exception");
        }

        [Then(@"the post filter validation should give at least one error")]
        public void then_the_post_filter_validation_should_give_at_least_one_error()
        {
            _scenarioContext["Exception"].Should().BeOfType(typeof(ValidationException));
        }

        [Then(@"the post filter validation should give the error '(.*)'")]
        public void then_the_post_filter_validation_should_give_the_error(string error)
        {
            if (!string.IsNullOrWhiteSpace(error))
            {
                var validationException = _scenarioContext["Exception"] as ValidationException;
                validationException.Should().NotBeNull();
                validationException.ValidationResult.Errors
                    .Count(x => x.ErrorMessage.ToLower() == error.ToLower())
                    .Should().BeGreaterOrEqualTo(1);
            }
            else
            {
                _scenarioContext.Keys.Should().NotContain("Exception");
            }
        }
    }
}