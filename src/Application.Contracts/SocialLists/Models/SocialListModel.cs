using SocialMediaLists.Application.Contracts.People.Models;
using System.Collections.Generic;

namespace SocialMediaLists.Application.Contracts.SocialLists.Models
{
    public class SocialListModel
    {
        public string Name { get; set; }
        public virtual IEnumerable<PersonModel> People { get; set; }
    }
}