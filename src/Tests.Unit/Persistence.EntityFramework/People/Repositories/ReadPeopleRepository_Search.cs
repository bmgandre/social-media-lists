using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SocialMediaLists.Application.Common.Data;
using SocialMediaLists.Application.People.Specifications;
using SocialMediaLists.Domain.People;
using SocialMediaLists.Persistence.EntityFramework.People.Repositories;
using SocialMediaLists.Tests.Unit.Persistence.EntityFramework.Common.Database;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SocialMediaLists.Tests.Unit.Persistence.EntityFramework.People.Repositories
{
    public sealed class ReadPeopleRepository_Search : IDisposable
    {
        private readonly MockDbContext _mockDbContext = new MockDbContext();
        private readonly DbContext _dbContext;

        public ReadPeopleRepository_Search()
        {
            _dbContext = _mockDbContext.DbContext;
            SetUpData();
        }

        private void SetUpData()
        {
            _dbContext.AddRange(GetSeedData());
            _dbContext.SaveChanges();
        }

        private static IEnumerable<Person> GetSeedData()
        {
            return new[]
            {
                new Person
                {
                    Name = "John",
                    Accounts = new [] { new SocialAccount { AccoutName = "johnlennon", Network= "twitter", PersonId = 1 } }
                },
                new Person
                {
                    Name = "Paul",
                    Accounts = new [] { new SocialAccount { AccoutName = "paulmccartney", Network= "twitter", PersonId = 2 } }
                },
                new Person
                {
                    Name = "George",
                    Accounts = new [] { new SocialAccount { AccoutName = "georgeharrison", Network= "twitter", PersonId = 3 } }
                },
                new Person
                {
                    Name = "Ringo",
                    Accounts = new [] { new SocialAccount{ AccoutName = "ringostar", Network= "twitter", PersonId = 4 } }
                }
            };
        }

        [Theory]
        [InlineData("John")]
        [InlineData("Paul")]
        [InlineData("George")]
        [InlineData("Ringo")]
        public async Task Should_find_a_person_by_the_name(string name)
        {
            var peopleRepository = new ReadPeopleRepository(_mockDbContext.DbContext);
            var specification = SpecificationBuilder<Person>.Create()
                .AndWithName(name);

            var result = await peopleRepository.SearchAsync(specification, CancellationToken.None);

            result.Should().HaveCount(1);
        }

        public void Dispose()
        {
            _mockDbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}