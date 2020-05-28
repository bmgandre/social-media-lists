using SocialMediaLists.Application.Contracts.Common.Data;
using SocialMediaLists.Domain.People;

namespace SocialMediaLists.Application.People.Specifications
{
    internal static class PersonSpecification
    {
        public static ISpecification<Person> AndWithName(this ISpecification<Person> specification, string name)
        {
            return specification.And(x => x.Name == name);
        }
    }
}