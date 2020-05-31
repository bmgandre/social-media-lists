using FluentAssertions;
using Moq;
using SocialMediaLists.Application.Contracts.People.Models;
using SocialMediaLists.Application.Contracts.People.Repositories;
using SocialMediaLists.Application.Contracts.People.Validators;
using SocialMediaLists.Application.People.Queries;
using SocialMediaLists.Tests.Unit.Application.People.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SocialMediaLists.Tests.Unit.Application.People.Queries
{
    public class PeopleQuery_GetById
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public async Task Should_find_a_person_by_id(long id)
        {
            var mockRepository = new Mock<IReadPeopleRepository>();
            mockRepository.Setup(x => x.FindAsync(It.IsAny<CancellationToken>(), It.IsAny<object[]>()))
                .Returns<CancellationToken, object[]>((c, o) =>
                {
                    return Task.FromResult(PeopleData.GetSeedData().FirstOrDefault(x => x.PersonId == (long)o[0]));
                });
            var mockValidator = new Mock<IPeopleFindValidators>();
            mockValidator.Setup(m => m.ValidateAndThrowAsync(It.IsAny<PersonFindModel>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(true));

            var queryPeople = new PeopleQuery(mockRepository.Object, mockValidator.Object);

            var person = await queryPeople.GetByIdAsync(id, CancellationToken.None);

            person.Should().NotBeNull();
        }
    }
}