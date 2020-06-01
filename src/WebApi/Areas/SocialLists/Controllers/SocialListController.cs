using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMediaLists.Application.Contracts.Common.Validators;
using SocialMediaLists.Application.Contracts.SocialLists.Models;
using SocialMediaLists.Application.Contracts.SocialLists.Queries;
using SocialMediaLists.WebApi.Filters.Exceptions;
using System.Threading;
using System.Threading.Tasks;

namespace SocialMediaLists.WebApi.Areas.SocialLists.Controllers
{
    [Area(nameof(SocialLists))]
    [ApiController]
    [Route("[area]/[controller]")]
    public class SocialListController
    {
        private readonly ISocialListsQuery _socialListsQuery;

        public SocialListController(ISocialListsQuery socialListsQuery)
        {
            _socialListsQuery = socialListsQuery;
        }

        [HttpGet]
        [ProducesResponseType(typeof(SocialListModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IValidationResult), StatusCodes.Status400BadRequest)]
        [TypeFilter(typeof(ValidationExceptionFilter))]
        public Task<SocialListModel> GetAsync([FromQuery] long id,
            CancellationToken cancellationToken)
        {
            return _socialListsQuery.GetByIdAsync(id, cancellationToken);
        }
    }
}