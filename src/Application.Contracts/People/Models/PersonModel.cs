using System.Collections.Generic;

namespace SocialMediaLists.Application.Contracts.People.Models
{
    public class PersonModel
    {
        public string Name { get; set; }
        public IEnumerable<SocialAccountModel> Accounts { get; set; }
    }
}