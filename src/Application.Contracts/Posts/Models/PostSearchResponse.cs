using System;
using System.Collections.Generic;

namespace SocialMediaLists.Application.Contracts.Posts.Models
{
    public class PostSearchResponse
    {
        public DateTime? Date { get; set; }
        public string Network { get; set; }
        public string Link { get; set; }
        public string Content { get; set; }
        public string Account { get; set; }
        public IEnumerable<string> Lists { get; set; }
    }
}