using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SocialMediaLists.Application.Common.Data;
using SocialMediaLists.Application.SocialLists.Specifications;
using SocialMediaLists.Domain.People;
using SocialMediaLists.Domain.SocialLists;
using SocialMediaLists.Persistence.EntityFramework.SocialLists.Repositories;
using SocialMediaLists.Tests.Unit.Persistence.EntityFramework.Common.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SocialMediaLists.Tests.Unit.Persistence.EntityFramework.SocialLists.Repositories
{
    public sealed class ReadSocialListsRepository_Search : IDisposable
    {
        private readonly MockDbContext _mockDbContext = new MockDbContext();
        private readonly DbContext _dbContext;

        public ReadSocialListsRepository_Search()
        {
            _dbContext = _mockDbContext.DbContext;
            SetupData();
        }

        private void SetupData()
        {
            _dbContext.AddRange(GetSeedData());
            _dbContext.SaveChanges();
        }

        private static IEnumerable<SocialListPerson> GetSeedData()
        {
            var people1 = new List<Person>
            {
                new Person
                {
                    Name = "John",
                    Accounts = new List<SocialAccount> { new SocialAccount { AccoutName = "johnlennon", Network= "twitter", PersonId = 1 } }
                },
                new Person
                {
                    Name = "Paul",
                    Accounts = new List<SocialAccount> { new SocialAccount { AccoutName = "paulmccartney", Network= "twitter", PersonId = 2 } }
                },
                new Person
                {
                    Name = "George",
                    Accounts = new List<SocialAccount> { new SocialAccount { AccoutName = "georgeharrison", Network= "twitter", PersonId = 3 } }
                },
                new Person
                {
                    Name = "Ringo",
                    Accounts = new List<SocialAccount> { new SocialAccount{ AccoutName = "ringostar", Network= "twitter", PersonId = 4 } }
                }
            };
            var list1 = new SocialList { Name = "The Beatles" };
            var list2 = new SocialList { Name = "Paul McCartney and Wings" };
            var data = people1
                .Select(x => new SocialListPerson
                {
                    People = x,
                    SocialLists = list1
                })
                .Concat(new List<SocialListPerson> { new SocialListPerson { People = people1[1], SocialLists = list2 } })
                .ToList();
            return data;
        }

        [Theory]
        [InlineData("The Beatles", 4)]
        [InlineData("Paul McCartney and Wings", 1)]
        public async Task Should_find_all_people_by_the_list_name(string name, int expectedListSize)
        {
            var peopleRepository = new ReadSocialListsRepository(_mockDbContext.DbContext);
            var specification = SpecificationBuilder<SocialList>.Create()
                    .WithName(name);

            var result = await peopleRepository.Where(specification, x => x.SocialListPerson, y => y.People)
                .ToListAsync(CancellationToken.None);

            result.Should().HaveCount(1);
            result.First().SocialListPerson.Count.Should().Be(expectedListSize);
        }

        public void Dispose()
        {
            _mockDbContext.Dispose();
            GC.SuppressFinalize(true);
        }
    }
}