using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        /// <summary>Home page</summary>
        /// <returns>View</returns>
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult HomePage()
        {
            return Redirect("ui/playground");
        }
    }
}
