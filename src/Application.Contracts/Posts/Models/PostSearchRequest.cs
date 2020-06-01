using SocialMediaLists.Application.Contracts.Common.Models;
using System.Collections.Generic;

namespace SocialMediaLists.Application.Contracts.Posts.Models
{
    public class PostSearchRequest
    {
        public string Content { get; set; }
        public string Network { get; set; }
        public IEnumerable<string> AccountNames { get; set; }
        public IEnumerable<string> Lists { get; set; }
        public DateRangeModel DateRange { get; set; }
        public PageModel Page { get; set; }
    }
}