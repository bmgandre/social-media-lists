using SocialMediaLists.Sample.Data.People;
using SocialMediaLists.Sample.Data.Posts;
using SocialMediaLists.Sample.Data.SocialLists;

namespace SocialMediaLists.Sample.Data.Common
{
    public sealed class DataSeedCollection
    {
        public PostsSeedData Posts { get; private set; } = new PostsSeedData();
        public PeopleSeedData People { get; private set; } = new PeopleSeedData();
        public SocialListSeedData SocialList { get; private set; } = new SocialListSeedData();
    }
}