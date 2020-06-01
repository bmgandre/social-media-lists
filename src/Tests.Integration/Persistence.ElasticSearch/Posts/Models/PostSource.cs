using System;

namespace SocialMediaLists.Tests.Integration.Persistence.ElasticSearch.Posts.Models
{
    internal class PostSource
    {
        public DateTime date { get; set; }
        public string network { get; set; }
        public string author { get; set; }
        public string link { get; set; }
        public string content { get; set; }
    }
}