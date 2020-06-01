using FluentAssertions;
using Moq;
using SocialMediaLists.Application.Contracts.Common.Validators;
using SocialMediaLists.Application.Contracts.People.Models;
using SocialMediaLists.Application.Contracts.People.Repositories;
using SocialMediaLists.Application.People.Validators;
using SocialMediaLists.Tests.Unit.Application.People.Data;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SocialMediaLists.Tests.Unit.Application.People.Validators
{
    public class PeopleFindValidator_Validate
    {
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(100)]
        [InlineData(1000)]
        public async Task Should_not_find_a_unregisted_person_by_id(long id)
        {
            var mockRepository = new Mock<IReadPeopleRepository>();
            mockRepository.Setup(x => x.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
                .Returns<object[], CancellationToken>((o, c) =>
                {
                    return Task.FromResult(PeopleData.GetSeedData().FirstOrDefault(x => x.PersonId == (long)o[0]));
                });

            var validator = new PeopleFindValidators(mockRepository.Object);
            Func<Task> act = async () => await validator.ValidateAndThrowAsync(new PersonFindModel { PersonId = id }, CancellationToken.None);

            act.Should().Throw<ValidationException>();
        }
    }
}