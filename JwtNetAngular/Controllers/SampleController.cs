using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JwtNetAngular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        [HttpGet("rottaProtetta")]
        [Authorize]
        public IActionResult rottaProtetta()
        {
            return Ok("Il token è valido");
        }

        [HttpGet("free")]
        public IActionResult free()
        {
            return Ok("Hai sempre accesso a questa rotta");
        }
    }
}
