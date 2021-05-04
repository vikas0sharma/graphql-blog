using Blog.Database;
using Blog.GraphQL;
using GraphQL;
using GraphQL.NewtonsoftJson;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Controllers
{
# nullable enable
    [Route("[controller]")]
    [ApiController]
    public class GraphQlController : Controller
    {
        IDocumentExecuter executer;
        private readonly IBlogContext context;
        ISchema schema;
        static DocumentWriter writer = new DocumentWriter(true);

        public GraphQlController(ISchema schema, IDocumentExecuter executer, IBlogContext context)
        {
            this.schema = schema;
            this.executer = executer;
            this.context = context;
        }

        [HttpPost]
        public async Task<ExecutionResult> Post(
            [BindRequired, FromBody] PostBody body,
            CancellationToken cancellation)
        {
            try
            {
                return await Execute(body.Query, body.OperationName, body.Variables, cancellation);
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public class PostBody
        {
            public string? OperationName { get; set; }
            public string Query { get; set; } = null!;
            public JObject? Variables { get; set; }
        }

        [HttpGet]
        public Task Get(
            [FromQuery] string query,
            [FromQuery] string? variables,
            [FromQuery] string? operationName,
            CancellationToken cancellation)
        {
            var jObject = ParseVariables(variables);
            return Execute(query, operationName, jObject, cancellation);
        }

        async Task<ExecutionResult> Execute(string query,
            string operationName,
            JObject variables,
            CancellationToken cancellation)
        {
            ExecutionOptions options = new ExecutionOptions
            {
                Schema = schema,
                Query = query,
                OperationName = operationName,
                Inputs = variables?.ToInputs(),
                UserContext = new GraphQLUserContext(context),
                CancellationToken = cancellation,
#if (DEBUG)
                ThrowOnUnhandledException = true,
                EnableMetrics = true,
#endif
            };
            var executeAsync = await executer.ExecuteAsync(options);
            return new ExecutionResult
            {
                Data = executeAsync.Data,
                Errors = executeAsync.Errors
            };
            //await writer.WriteAsync(Response.Body, executeAsync, cancellation);
        }

        static JObject? ParseVariables(string? variables)
        {
            if (variables == null)
            {
                return null;
            }

            try
            {
                return JObject.Parse(variables);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not parse variables.", exception);
            }
        }
    }
# nullable disable
}