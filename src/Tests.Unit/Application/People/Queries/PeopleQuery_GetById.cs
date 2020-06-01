using FluentAssertions;
using Moq;
using SocialMediaLists.Application.Contracts.Common.Data;
using SocialMediaLists.Application.Contracts.People.Models;
using SocialMediaLists.Application.Contracts.People.Repositories;
using SocialMediaLists.Application.Contracts.People.Validators;
using SocialMediaLists.Application.People.Queries;
using SocialMediaLists.Domain.People;
using SocialMediaLists.Tests.Unit.Application.People.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            mockRepository.Setup(m => m.SearchAsync(It.IsAny<ISpecification<Person>>(), It.IsAny<Expression<Func<Person, ICollection<SocialAccount>>>>(), It.IsAny<CancellationToken>()))
                .Returns<ISpecification<Person>, Expression<Func<Person, ICollection<SocialAccount>>>, CancellationToken>((p, e, c) =>
                {
                    var id = GetSearchIdFromExpression(p);
                    return Task.FromResult(PeopleData.GetSeedData().Where(x => x.PersonId == id));
                });
            var mockValidator = new Mock<IPeopleFindValidators>();
            mockValidator.Setup(m => m.ValidateAndThrowAsync(It.IsAny<PersonFindModel>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(true));

            var queryPeople = new PeopleQuery(mockRepository.Object, mockValidator.Object);

            var person = await queryPeople.GetByIdAsync(id, CancellationToken.None);

            person.Should().NotBeNull();
        }

        private static long GetSearchIdFromExpression(ISpecification<Person> specification)
        {
            var right = ((BinaryExpression)specification.Predicate.Body).Right;
            var id = Expression.Lambda(right).Compile().DynamicInvoke();
            return (long)id;
        }
    }
}