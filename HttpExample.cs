using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;
using AzureSQL.ToDo;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace My.Function
{
    public class HttpExample
    {
        private readonly ILogger<HttpExample> _logger;

        public HttpExample(ILogger<HttpExample> logger)
        {
            _logger = logger;
        }

        // [Function("HttpExample")]
        // public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        // {
        //     _logger.LogInformation("C# HTTP trigger function processed a request.");
        //     return new OkObjectResult("Welcome Mike to Azure Functions!");
        // }
 
        [Function("httpget")]
        public static OutputType Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] 
            HttpRequestData req, 
            FunctionContext executionContext)
    {
        var logger = executionContext.GetLogger("HttpExample");
        logger.LogInformation("C# HTTP trigger function processed a request.");

        var message = "Welcome to Azure Functions!";

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        response.WriteStringAsync(message);

        // Return a response to both HTTP trigger and Azure SQL output binding.
        return new OutputType()
        {
            ToDoItem = new ToDoItem
            {
                Id = System.Guid.NewGuid(),
                title = message,
                completed = false,
                url = ""
            },
            HttpResponse = response
        };
}
    }

    public class OutputType
    {
        [SqlOutput("dbo.ToDo", connectionStringSetting: "SqlConnectionString")]
        public required ToDoItem ToDoItem { get; set; }
        [HttpResult]
        public required HttpResponseData HttpResponse { get; set; }
    }
}