using SocialMediaLists.Application.Contracts.Common.Models;
using System.Collections.Generic;

namespace SocialMediaLists.Application.Contracts.Posts.Models
{
    public class SearchPostRequest
    {
        public string Text { get; set; }
        public string Network { get; set; }
        public IEnumerable<string> Lists { get; set; }
        public DateRangeModel DateRange { get; set; }
        public PageModel Page { get; set; }
    }
}