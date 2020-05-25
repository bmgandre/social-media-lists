using SocialMediaLists.Application.Contracts.Posts.Models;
using SocialMediaLists.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMediaLists.Application.Contracts.Posts.Repositories
{
    public interface IReadPostRepository
    {
        Task<IEnumerable<Post>> SearchAsync(PostFilter filter);
    }
}