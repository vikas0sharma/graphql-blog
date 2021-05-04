using Blog.Database;
using Blog.Database.Entities;
using GraphQL;
using GraphQL.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.GraphQL.Graphs.Entities
{
    [GraphQLMetadata("posts")]
    public class PostGraph : EfObjectGraphType<BlogContext, Post>
    {
        public PostGraph(IEfGraphQLService<BlogContext> graphQlService) : base(graphQlService)
        {
            AutoMap();
        }
    }
}
