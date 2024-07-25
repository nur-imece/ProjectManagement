using MongoDB.Bson;

namespace ProjectManagement.Data.Entity;

public class Author
{
    
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
    
    public string Name { get; set; }

    public DateTime Birthdate { get; set; }

    public string Nationality { get; set; }

}