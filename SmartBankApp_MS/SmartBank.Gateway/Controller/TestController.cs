using Microsoft.AspNetCore.Mvc;

namespace SmartBank.Gateway.Controller
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Gateway Working");
        }
    }
}
