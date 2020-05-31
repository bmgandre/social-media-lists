using SocialMediaLists.Application.Contracts.Common.Data;
using SocialMediaLists.Domain.SocialLists;

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
    }
}