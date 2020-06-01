using System;

namespace SocialMediaLists.Sample.ConsoleApplication.Models
{
    internal class FlatPostSearchResponseModel
    {
        public DateTime? Date { get; set; }
        public string Network { get; set; }
        public string Link { get; set; }
        public string Content { get; set; }
        public string Account { get; set; }
        public string Lists { get; set; }
    }
}