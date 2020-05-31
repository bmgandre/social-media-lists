using SocialMediaLists.Sample.Data.People;
using SocialMediaLists.Sample.Data.Posts;
using SocialMediaLists.Sample.Data.SocialLists;

namespace SocialMediaLists.Sample.Data.Common
{
    public sealed class DataSeedCollection
    {
        public PeopleSeedData People { get; private set; }
        public SocialListSeedData SocialList { get; private set; }
        public PostsSeedData Posts { get; private set; }

        public DataSeedCollection()
        {
            People = new PeopleSeedData();
            SocialList = new SocialListSeedData();
            Posts = new PostsSeedData(People);
        }
    }
}