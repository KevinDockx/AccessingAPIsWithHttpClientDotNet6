using Microsoft.AspNetCore.Mvc;

namespace Movies.API.Controllers
{
    [Route("api/films")]
    [ApiController]
    public class FilmsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetFilms()
        {
            return RedirectToAction("GetMovies", "Movies");
        }
    }
}
