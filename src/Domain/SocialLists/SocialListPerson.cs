using SocialMediaLists.Domain.People;

namespace SocialMediaLists.Domain.SocialLists
{
    public class SocialListPerson
    {
        public long SocialListId { get; set; }
        public SocialList SocialList { get; set; }

        public long PersonId { get; set; }
        public Person Person { get; set; }
    }
}