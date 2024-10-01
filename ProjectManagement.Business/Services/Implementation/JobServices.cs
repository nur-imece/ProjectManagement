using MongoDB.Driver;
using ProjectManagement.Data;
using ProjectManagement.Data.Entity;
using ProjectManagement.Model.Services.Interface;

namespace ProjectManagement.Model.Services.Implementation;

public class JobServices: IJobServices
{
    private readonly MflixDbContext _context;
    
    public JobServices(MflixDbContext context)
    {
        _context = context;
    }
    
    public  List<Job> GetAll()
    {
        return _context.Jobs.Find(movie => true).ToList();
    }
    
    public async Task AddJobAsync(Job job)
    {
        await _context.Jobs.InsertOneAsync(job);

    }
}