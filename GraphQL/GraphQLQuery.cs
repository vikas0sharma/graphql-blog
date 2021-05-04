using Blog.Database;
using GraphQL.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.GraphQL
{
    public class GraphQLQuery : QueryGraphType<BlogContext>
    {
        public GraphQLQuery(IEfGraphQLService<BlogContext> graphQlService) : base(graphQlService)
        {
            AddQueryField(name: "posts", resolve: ctx => ctx.DbContext.Post);
            AddSingleField(name: "post", resolve: ctx => ctx.DbContext.Post);
            AddQueryField(name: "users", resolve: ctx => ctx.DbContext.User);
            AddSingleField(name: "user", resolve: ctx => ctx.DbContext.User);
        }
    }
}
