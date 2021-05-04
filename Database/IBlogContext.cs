using Blog.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Blog.Database
{
    public interface IBlogContext
    {
        DbSet<Post> Post { get; set; }
        DbSet<User> User { get; set; }
        DatabaseFacade Database { get; }
    }
}
