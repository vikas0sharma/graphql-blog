using Blog.Database;
using Blog.Database.Entities;
using Blog.GraphQL;
using Blog.GraphQL.Graphs.Entities;
using GraphQL;
using GraphQL.EntityFramework;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using GraphQL.Types;
using GraphQL.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<IBlogContext, BlogContext>(options =>
            options.UseNpgsql(Configuration.GetSection("Database:ConnectionString").Value));
            // GraphQL
            var s = services.BuildServiceProvider();
            var db = s.GetService<IBlogContext>();
            
            GraphTypeTypeRegistry.Register<Post, PostGraph>();
            GraphTypeTypeRegistry.Register<User, UserGraph>();
            EfGraphQLConventions.RegisterInContainer(services, (context) => {
                return (context as GraphQLUserContext)?.Context;

            }, model: (db as BlogContext).Model);
            EfGraphQLConventions.RegisterConnectionTypesInContainer(services);
            services.AddSingleton<IDocumentExecuter, EfDocumentExecuter>();
            services.AddSingleton<ISchema, GraphQLSchema>();
            //services.AddSingleton<GraphQLQuery>();
            // Import also all the other created Graphs
            foreach (Type type in GetGraphQlTypes())
            {
                services.AddScoped(type);
            }

            services.AddMvc(option => option.EnableEndpointRouting = false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            bool executeMigration = Convert.ToBoolean(Configuration.GetSection("Database:ExecuteEntityMigration").Value);
            if (executeMigration)
            {
                IBlogContext dbContext = app.ApplicationServices.GetService<IBlogContext>();
                dbContext.Database.Migrate();
                dbContext.User.Add(new User
                {
                    Id = 1,
                    Name = "Vikas Sharma",
                    Email = "mailbox.viksharma@gmail.com",
                    Website = "vik-sharma.medium.com"
                });
                ((DbContext)dbContext).SaveChanges();
                dbContext.Post.Add(new Post
                {
                    Body = "This is my first Herokuapp Postgres Db app",
                    Title = "GraphQL+ EF + Postgres",
                    Id = 1,
                    UserId = 1

                });
                ((DbContext)dbContext).SaveChanges();
            }
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            // Graph QL
            app.UseGraphQLPlayground();
            //app.UseGraphQLGraphiQL();
            app.UseMvc();

        }
        static IEnumerable<Type> GetGraphQlTypes()
        {
            return typeof(Startup).Assembly
                .GetTypes()
                .Where(x => !x.IsAbstract &&
                            (typeof(IObjectGraphType).IsAssignableFrom(x) ||
                             typeof(IInputObjectGraphType).IsAssignableFrom(x) ||
                             typeof(ScalarGraphType).IsAssignableFrom(x)));
        }
    }
}
