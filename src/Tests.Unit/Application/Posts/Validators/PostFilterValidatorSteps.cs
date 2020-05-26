using FluentAssertions;
using SocialMediaLists.Application.Contracts.Common.Models;
using SocialMediaLists.Application.Contracts.Common.Validators;
using SocialMediaLists.Application.Contracts.Posts.Models;
using SocialMediaLists.Application.Posts.Validators;
using System;
using TechTalk.SpecFlow;

namespace SocialMediaLists.Tests.Unit.Application.Posts.Validators
{
    [Binding]
    internal class PostFilterValidatorSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly PostFilterValidator _postFilterValidator;
        private PostFilter _postFilter;

        public PostFilterValidatorSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _postFilterValidator = new PostFilterValidator();
        }

        [Given(@"I have post filter model to be validated")]
        public void given_i_have_post_filter_model_to_be_validated()
        {
            _postFilter = new PostFilter();
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

        [Given(@"the post date filter is \['(.*)', '(.*)']")]
        public void given_the_post_date_filter_is(string begin, string end)
        {
            var dateParser = new Chronic.Parser();
            _postFilter.DateRange = new DateRangeModel
            {
                Begin = dateParser.Parse(begin).ToTime(),
                End = dateParser.Parse(end).ToTime()
            };
        }

        [Given(@"the post page filter is \['(.*)', '(.*)']")]
        public void given_the_post_page_filter_is(int from, int size)
        {
            _postFilter.Page = new PageModel
            {
                From = from,
                Size = size
            };
        }

        [When(@"I validate the post filter")]
        public void when_i_validate_the_post_filter()
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
    }
}