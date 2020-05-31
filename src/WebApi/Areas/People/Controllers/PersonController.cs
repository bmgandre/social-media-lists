using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMediaLists.Application.Contracts.Common.Validators;
using SocialMediaLists.Application.Contracts.People.Models;
using SocialMediaLists.Application.Contracts.People.Queries;
using SocialMediaLists.WebApi.Filters.Exceptions;
using System.Threading;
using System.Threading.Tasks;

namespace SocialMediaLists.WebApi.Areas.People.Controllers
{
    [Area(nameof(People))]
    [ApiController]
    [Route("[area]/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IPeopleQuery _peopleQuery;

        public PersonController(IPeopleQuery peopleQuery)
        {
            _peopleQuery = peopleQuery;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PersonModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IValidationResult), StatusCodes.Status400BadRequest)]
        [TypeFilter(typeof(ValidationExceptionFilter))]
        public Task<PersonModel> GetAsync([FromQuery] long id,
            CancellationToken cancellationToken)
        {
            return _peopleQuery.GetByIdAsync(id, cancellationToken);
        }
    }
}