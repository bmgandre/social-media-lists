using System;
using System.Collections.Generic;

namespace SocialMediaLists.Domain.Posts
{
    public class Post
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string Network { get; set; }
        public string Link { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public IEnumerable<string> Lists { get; set; }
    }
}