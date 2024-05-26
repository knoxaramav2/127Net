using Microsoft.AspNetCore.Mvc;

namespace HomeNet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ComponentController(ILogger<ComponentController> logger) 
        : Controller
    {
        private readonly ILogger<ComponentController> _logger = logger;

        [HttpGet]
        public IEnumerable<string> Get(string? item)
        {
            return [item ?? "DEF", "Hello", "World"];
        }
    }
}
