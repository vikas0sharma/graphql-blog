using Blog.Database;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Blog.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PostsController : Controller
    {
        private readonly IBlogContext context;

        public PostsController(IBlogContext context)
        {
            this.context = context;
        }
        public IActionResult Get()
        {
            var posts = context.Post.ToArray();
            return Ok(posts);
         }
    }
}
