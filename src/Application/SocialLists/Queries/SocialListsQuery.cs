using SocialMediaLists.Application.Common.Data;
using SocialMediaLists.Application.Contracts.People.Models;
using SocialMediaLists.Application.Contracts.SocialLists.Models;
using SocialMediaLists.Application.Contracts.SocialLists.Queries;
using SocialMediaLists.Application.Contracts.SocialLists.Repositories;
using SocialMediaLists.Application.SocialLists.Specifications;
using SocialMediaLists.Domain.SocialLists;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SocialMediaLists.Application.SocialLists.Queries
{
    public class SocialListsQuery : ISocialListsQuery
    {
        private readonly IReadSocialListsRepository _readSocialListsRepository;

        public SocialListsQuery(IReadSocialListsRepository readSocialListsRepository)
        {
            _readSocialListsRepository = readSocialListsRepository;
        }

        public async Task<SocialListModel> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            var specification = SpecificationBuilder<SocialList>.Create()
                .WithId(id);

            var result = await _readSocialListsRepository.SearchAsync(specification,
                list => list.SocialListPerson,
                sp => sp.Person,
                cancellationToken);
            var socialList = result.First();

            return new SocialListModel
            {
                Name = socialList.Name,
                People = socialList.SocialListPerson.Select(x =>
               {
                   return new PersonModel
                   {
                       Id = x.PersonId,
                       Name = x.Person.Name
                   };
               })
            };
        }
    }
}