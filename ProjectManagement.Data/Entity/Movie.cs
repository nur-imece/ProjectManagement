using MongoDB.Bson;

namespace ProjectManagement.Data.Entity;

public class Movie
{
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
    public string Title { get; set; } = string.Empty;
    public string? Plot { get; set; }
    public IEnumerable<string>? Genres { get; set; }
    public IEnumerable<string>? Cast { get; set; }
    // additional properties as required
}