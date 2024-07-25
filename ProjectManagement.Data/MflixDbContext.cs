using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.EntityFrameworkCore.Extensions;
using ProjectManagement.Data.Entity;

namespace ProjectManagement.Data;

public class MflixDbContext : DbContext
{
    public MflixDbContext(DbContextOptions<MflixDbContext> options) : base(options) {}


    public DbSet<Movie> Movies { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
    }
}