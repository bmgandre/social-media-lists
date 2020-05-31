using FluentAssertions;
using Moq;
using SocialMediaLists.Application.Contracts.Common.Data;
using SocialMediaLists.Application.Contracts.SocialLists.Repositories;
using SocialMediaLists.Application.SocialLists.Queries;
using SocialMediaLists.Domain.People;
using SocialMediaLists.Domain.SocialLists;
using SocialMediaLists.Tests.Unit.Application.SocialLists.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SocialMediaLists.Tests.Unit.Application.SocialLists.Queries
{
    public class SocialListsQuery_GetById
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task Should_find_a_sociallist_by_id(long id)
        {
            var mockRepository = new Mock<IReadSocialListsRepository>();
            mockRepository.Setup(m => m.SearchAsync(It.IsAny<ISpecification<SocialList>>(), It.IsAny<Expression<Func<SocialList, IEnumerable<SocialListPerson>>>>(), It.IsAny<Expression<Func<SocialListPerson, Person>>>(), It.IsAny<CancellationToken>()))
                .Returns<ISpecification<SocialList>, Expression<Func<SocialList, IEnumerable<SocialListPerson>>>, Expression<Func<SocialListPerson, Person>>, CancellationToken>((p, e1, e2, c) =>
                {
                    var id = GetSearchIdFromExpression(p);
                    return Task.FromResult(SocialListsData.GetSeedData().Where(x => x.SocialListId == id));
                });

            var queryPeople = new SocialListsQuery(mockRepository.Object);

            var person = await queryPeople.GetByIdAsync(id, CancellationToken.None);

            person.Should().NotBeNull();
        }

        private static long GetSearchIdFromExpression(ISpecification<SocialList> specification)
        {
            var right = ((BinaryExpression)specification.Predicate.Body).Right;
            var id = Expression.Lambda(right).Compile().DynamicInvoke();
            return (long)id;
        }
    }
}