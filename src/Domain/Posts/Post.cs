using System;

namespace SocialMediaLists.Domain.Posts
{
    public class Post
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string Network { get; set; }
        public string Link { get; set; }
        public string Content { get; set; }
    }
}