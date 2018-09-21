using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Movies.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    public class UsersController : Controller
    {
        [Route("{userId}/movies/{movieId}/top-ranked")]
        [HttpGet]
        public Task<IActionResult> GetTopRanked(Guid userId, Guid movieId)
        {
            throw new NotImplementedException();
        }

        [Route("{userId}/movies/{movieId}/rating")]
        [HttpPost]
        public Task<IActionResult> AddRating(Guid userId, Guid movieId)
        {
            throw new NotImplementedException();
        }
    }
}
