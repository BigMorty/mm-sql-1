using System;

namespace AzureSQL.ToDo
{

    
    //public record Person([property: JsonPropertyName("name")] string Name, [property: JsonPropertyName("age")] int Age);

    public class ToDoItem
    {
        public Guid Id { get; set; }
        public int? order { get; set; }
        public required string title { get; set; }
        public string? url { get; set; }
        public bool? completed { get; set; }
    }
}