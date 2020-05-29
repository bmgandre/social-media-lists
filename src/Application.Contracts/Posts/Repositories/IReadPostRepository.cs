using SocialMediaLists.Application.Contracts.Posts.Models;
using SocialMediaLists.Domain.Posts;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SocialMediaLists.Application.Contracts.Posts.Repositories
{
    public interface IReadPostRepository
    {
        Task<IEnumerable<Post>> SearchAsync(PostSearchRequest filter, CancellationToken cancellationToken);
    }
}