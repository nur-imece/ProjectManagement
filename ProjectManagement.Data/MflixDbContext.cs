using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.EntityFrameworkCore.Extensions;
using ProjectManagement.Data.Entity;
namespace ProjectManagement.Data;
public class MflixDbContext : DbContext
{
    public MflixDbContext(DbContextOptions<MflixDbContext> options) : base(options)
    {
    }
    
    public DbSet<Job> Jobs { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Comment> Comments { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var jobEntity = modelBuilder.Entity<Job>();
        jobEntity.ToCollection("jobs");
        jobEntity.Property(x => x.Id)
            .HasElementName("_id")
            .HasConversion<ObjectId>();
        jobEntity.Property(x => x.Name).HasElementName("name");
        jobEntity.Property(x => x.Status).HasElementName("status");
        jobEntity.Property(x => x.StartDate).HasElementName("startDate");
    

    base.OnModelCreating(modelBuilder);
        var movieEntity = modelBuilder.Entity<Movie>();
        movieEntity.ToCollection("movies");
        movieEntity.Property(x => x.Id)
            .HasElementName("_id")
            .HasConversion<ObjectId>();
        movieEntity.Property(x => x.Title).HasElementName("title");
        movieEntity.Property(x => x.Plot).HasElementName("plot");
        movieEntity.Property(x => x.Genres).HasElementName("genres");
        movieEntity.Property(x => x.Cast).HasElementName("cast");

        

        var commentEntity = modelBuilder.Entity<Comment>();
        commentEntity.ToCollection("comments");
        commentEntity.Property(x => x.Id)
            .HasElementName("_id")
            .HasConversion<ObjectId>();
        commentEntity.Property(x => x.Description).HasElementName("description");
        commentEntity.Property(x => x.Commentedby).HasElementName("commentedby");
        commentEntity.Property(x => x.Commentdate).HasElementName("commentdate");
        
    }
}