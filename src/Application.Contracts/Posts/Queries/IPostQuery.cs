using System.Collections.Generic;
using System.Threading.Tasks;
using SocialMediaLists.Application.Contracts.Posts.Models;

namespace SocialMediaLists.Application.Contracts.Posts.Queries
{
    public interface IPostQuery
    {
        Task<IEnumerable<PostModel>> SearchAsync(PostFilter filter);
    }
}