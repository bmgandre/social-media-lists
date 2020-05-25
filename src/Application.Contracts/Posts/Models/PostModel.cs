using System;

namespace SocialMediaLists.Application.Contracts.Posts.Models
{
    public class PostModel
    {
        public DateTime? Date { get; set; }
        public string Network { get; set; }
        public string Link { get; set; }
        public string Content { get; set; }
    }
}