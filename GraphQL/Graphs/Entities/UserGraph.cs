using Blog.Database;
using Blog.Database.Entities;
using GraphQL.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.GraphQL.Graphs.Entities
{
    public class UserGraph : EfObjectGraphType<BlogContext, User>
    {
        public UserGraph(IEfGraphQLService<BlogContext> graphQlService) : base(graphQlService)
        {
            AutoMap();
        }
    }
}
