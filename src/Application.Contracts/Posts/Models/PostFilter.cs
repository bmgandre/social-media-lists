using SocialMediaLists.Application.Contracts.Common.Models;

namespace SocialMediaLists.Application.Contracts.Posts.Models
{
    public class PostFilter
    {
        public string Text { get; set; }
        public string Network { get; set; }
        public DateRangeModel DateRange { get; set; }
        public PageModel Page { get; set; }
    }
}