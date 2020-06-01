using SocialMediaLists.Sample.Data.Posts;
using SocialMediaLists.Sample.Data.SocialLists;

namespace SocialMediaLists.Sample.Data.Common
{
    public sealed class DataSeedCollection
    {
        public SocialListSeedData SocialList { get; private set; }
        public PostsSeedData Posts { get; private set; }

        public DataSeedCollection()
        {
            SocialList = new SocialListSeedData();
            Posts = new PostsSeedData(SocialList);
        }
    }
}