namespace ProjectManagement.Model.Request;

public class BookCreateRequest
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string Category { get; set; } = null!;
    public string Author { get; set; } = null!;
}