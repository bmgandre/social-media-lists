using SocialMediaLists.Application.Contracts.Posts.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SocialMediaLists.Application.Contracts.Posts.Queries
{
    public interface IPostQuery
    {
        Task<IEnumerable<PostModel>> SearchAsync(PostFilter filter, CancellationToken cancellationToken);
    }
}