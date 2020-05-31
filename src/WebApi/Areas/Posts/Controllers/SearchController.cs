using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMediaLists.Application.Contracts.Common.Validators;
using SocialMediaLists.Application.Contracts.Posts.Models;
using SocialMediaLists.Application.Contracts.Posts.Queries;
using SocialMediaLists.WebApi.Filters.Exceptions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SocialMediaLists.WebApi.Areas.Posts.Controllers
{
    [Area(nameof(Posts))]
    [ApiController]
    [Route("[area]/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly IPostQuery _postQuery;

        public SearchController(IPostQuery postQuery)
        {
            _postQuery = postQuery;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PostSearchResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IValidationResult), StatusCodes.Status400BadRequest)]
        [TypeFilter(typeof(ValidationExceptionFilter))]
        public Task<IEnumerable<PostSearchResponse>> SearchAsync([FromQuery] PostSearchRequest request,
            CancellationToken cancellationToken)
        {
            return _postQuery.SearchAsync(request, cancellationToken);
        }
    }
}