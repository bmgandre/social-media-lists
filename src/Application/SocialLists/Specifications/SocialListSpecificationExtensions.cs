using SocialMediaLists.Application.Contracts.Common.Data;
using SocialMediaLists.Domain.SocialLists;
using System.Collections.Generic;
using System.Linq;

namespace SocialMediaLists.Application.SocialLists.Specifications
{
    public static class SocialListSpecificationExtensions
    {
        public static ISpecification<SocialList> WithId(this ISpecification<SocialList> specification,
            long id)
        {
            return specification.And(x => x.SocialListId == id);
        }

        public static ISpecification<SocialList> WithName(this ISpecification<SocialList> specification,
            string name)
        {
            return specification.And(x => x.Name == name);
        }

        public static ISpecification<SocialList> WithNames(this ISpecification<SocialList> specification,
            IEnumerable<string> names)
        {
            return specification.And(x => names.Contains(x.Name));
        }

        public static ISpecification<SocialList> HasPeople(this ISpecification<SocialList> specification)
        {
            return specification.And(x => x.SocialListPerson.Count > 0);
        }
    }
}