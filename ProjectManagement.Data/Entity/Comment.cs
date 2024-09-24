using MongoDB.Bson;

namespace ProjectManagement.Data.Entity;

public class Comment
{
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
    public string Description { get; set; }
    public string? Commentedby { get; set; }
    public DateTime Commentdate { get; set; }
    
    // additional properties as required
}