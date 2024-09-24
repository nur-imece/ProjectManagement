using MongoDB.Bson;

namespace ProjectManagement.Data.Entity;

public class Job
{
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
    public string Name { get; set; }
    public string? Status { get; set; }
    public DateTime StartDate { get; set; }
}