namespace SocialMediaLists.WebApi.Settings
{
    public class SocialMediaListSettings
    {
        public const string SectionName = "SocialMediaList";

        public ElasticSearchSettings ElasticSearch { get; set; }
    }
}