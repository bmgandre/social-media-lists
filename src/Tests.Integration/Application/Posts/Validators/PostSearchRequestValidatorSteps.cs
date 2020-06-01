using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SocialMediaLists.Application.Contracts.Common.Validators;
using SocialMediaLists.Application.Contracts.Posts.Models;
using SocialMediaLists.Application.Posts.Validators;
using SocialMediaLists.Domain.People;
using SocialMediaLists.Domain.SocialLists;
using SocialMediaLists.Persistence.EntityFramework.People.Repositories;
using SocialMediaLists.Persistence.EntityFramework.SocialLists.Repositories;
using SocialMediaLists.Tests.Integration.Persistence.EntityFramework.Common.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace SocialMediaLists.Tests.Integration.Application.Posts.Validators
{
    [Binding]
    internal class PostSearchRequestValidatorSteps : IDisposable
    {
        private readonly MockDbContext _mockDbContext = new MockDbContext();
        private readonly ScenarioContext _scenarioContext;
        private readonly PostSearchRequestValidator _postFilterValidator;
        private readonly DbContext _dbContext;
        private PostSearchRequest _postFilter;

        public PostSearchRequestValidatorSteps(ScenarioContext scenarioContext)
        {
            _dbContext = _mockDbContext.DbContext;
            _scenarioContext = scenarioContext;

            var readSocialListsRepository = new ReadSocialListsRepository(_dbContext);
            var readPeopleRepository = new ReadPeopleRepository(_dbContext);

            _postFilterValidator = new PostSearchRequestValidator(readSocialListsRepository, readPeopleRepository);
        }

        public void Dispose()
        {
            _mockDbContext.Dispose();
            GC.SuppressFinalize(true);
        }

        [Given(@"the following lists are registered")]
        public async Task given_the_following_lists_are_registered(Table table)
        {
            var socialListBackground = table.CreateSet<SocialList>();
            var socialLists = socialListBackground.Select(list =>
            {
                list.SocialListPerson = new List<SocialListPerson>
                {
                    new SocialListPerson
                    {
                        Person = new Person { Name = "Sample" }
                    }
                };

                return list;
            });
            await _dbContext.AddRangeAsync(socialLists);
            await _dbContext.SaveChangesAsync();
        }

        [Given(@"the following lists are registered with some members")]
        public async Task given_the_following_lists_are_registered_with_some_members(Table table)
        {
            var socialLists = table.Rows.Select(entry =>
            {
                return new SocialList
                {
                    Name = entry[0],
                    SocialListPerson = Enumerable
                        .Range(0, int.Parse(entry[1]))
                        .Select(item =>
                        {
                            return new SocialListPerson
                            {
                                Person = new Person { Name = $"Person {item}" }
                            };
                        }).ToList()
                };
            });
            await _dbContext.AddRangeAsync(socialLists);
            await _dbContext.SaveChangesAsync();
        }

        [Given(@"a search post request to validate")]
        public void given_a_search_post_request_to_validate()
        {
            _postFilter = new PostSearchRequest();
        }

        [Given(@"the post list filter contains")]
        public void given_the_post_list_filter_contains(Table table)
        {
            var lists = table.Rows.SelectMany(x => x.Values)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();
            _postFilter.Lists = lists;
        }

        [When(@"the request to search posts with the specified filter is checked")]
        public async Task when_the_request_to_search_posts_with_the_specified_filter_is_checked()
        {
            try
            {
                await _postFilterValidator.ValidateAndThrowAsync(_postFilter, CancellationToken.None);
            }
            catch (Exception ex)
            {
                _scenarioContext.Add("Exception", ex);
            }
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