using SocialMediaLists.Application.Contracts.Common.Data;
using SocialMediaLists.Domain.People;
using System.Linq;

namespace SocialMediaLists.Application.People.Specifications
{
    internal static class PersonSpecificationExtensions
    {
        public static ISpecification<Person> WithId(this ISpecification<Person> specification, long id)
        {
            return specification.And(x => x.PersonId == id);
        }

        public static ISpecification<Person> WithName(this ISpecification<Person> specification, string name)
        {
            return specification.And(x => x.Name == name);
        }

        public static ISpecification<Person> WithAccountName(this ISpecification<Person> specification, string accountName)
        {
            return specification.And(x => x.Accounts.Any(account => account.AccountName == accountName));
        }
    }
}