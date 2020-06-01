namespace SocialMediaLists.Domain.People
{
    public class SocialAccount
    {
        public long SocialAccountId { get; set; }
        public string Network { get; set; }
        public string AccountName { get; set; }

        public long PersonId { get; set; }
        public virtual Person Person { get; set; }
    }
}