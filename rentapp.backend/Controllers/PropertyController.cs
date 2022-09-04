using Microsoft.AspNetCore.Mvc;

namespace rentapp.backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PropertyController : ControllerBase
    {
        [HttpGet]
        [Route("get")]
        public IEnumerable<string> Get()
        {
            return new string[] { "Property 1", "Property 2" };
        }
    }
}
