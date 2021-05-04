using Blog.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.GraphQL
{
    public class GraphQLUserContext: Dictionary<string, object>
    {
        public BlogContext Context { get; set; }
        public GraphQLUserContext(IBlogContext blogContext)
        {
            Context = blogContext as BlogContext;
        }
    }
}
