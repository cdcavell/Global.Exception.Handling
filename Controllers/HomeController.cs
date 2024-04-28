using Global.Exception.Handling.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Global.Exception.Handling.Controllers
{
    public class HomeController(ILogger<HomeController> logger) : ApplicationBaseController<HomeController>(logger)
    {
        [HttpGet("/")]
        [HttpGet("Home")]
        [HttpGet("Home/Index")]
        public IActionResult Index()
        {
            throw new NotFoundException($"Not Found Exception Thrown");
            throw new BadRequestException($"Bad Request Exception Thrown");
            throw new System.Exception($"Exception Thrown");
        }
    }
}
