using MongoDB.Driver;
using ProjectManagement.Data;
using ProjectManagement.Data.Entity;
using ProjectManagement.Model.Services.Interface;

namespace ProjectManagement.Model.Services.Implementation;

public class CommentServices: ICommentServices
{
    private readonly MflixDbContext _context;
    
    public CommentServices(MflixDbContext context)
    {
        _context = context;
    }
    
    public  List<Comment> GetAll()
    {
        return _context.Comments.Find(movie => true).ToList();

    }
    
    public async Task AddCommentAsync(Comment comment)
    {
        await _context.Comments.InsertOneAsync(comment);

    }
}