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
 
        [Function("HttpExample")]
        public OutputType Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] 
            HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        var message = "Welcome to Azure Functions!";
         
        // Return a response to both HTTP trigger and Azure SQL output binding.
        return new OutputType
        {
            ToDoItem = new ToDoItem
            {
                Id = System.Guid.NewGuid(),
                title = message,
                completed = false,
                url = ""
            },
            HttpResponse = new OkObjectResult("Welcome to Azure Functions!")
        };
}
    }

    public class OutputType
    {
        [SqlOutput("dbo.ToDo", connectionStringSetting: "SqlConnectionString")]
        public required ToDoItem ToDoItem { get; set; }
        [HttpResult]
        public required IActionResult HttpResponse { get; set; }
    }
}