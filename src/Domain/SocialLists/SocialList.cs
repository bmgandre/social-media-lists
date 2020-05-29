using System.Collections.Generic;

namespace SocialMediaLists.Domain.SocialLists
{
    public class SocialList
    {
        public long SocialListId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<SocialListPerson> SocialListPerson { get; set; }
    }
}