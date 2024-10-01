using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;
using ProjectManagement.Data.Entity;
namespace ProjectManagement.Data;
public class MflixDbContext : DbContext
{
    
        private readonly IMongoDatabase _database;

        public MflixDbContext(IMongoDatabase database)
        {
            _database = database;
        }

        public IMongoCollection<Movie> Movies => _database.GetCollection<Movie>("Movies");
        public IMongoCollection<Comment> Comments => _database.GetCollection<Comment>("Comments");
        public IMongoCollection<Job> Jobs => _database.GetCollection<Job>("Jobs");
    

}