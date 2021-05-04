using System.Collections.Generic;

namespace Blog.Database.Entities
{
    public class User
    {
        public User()
        {
            Posts = new HashSet<Post>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}
