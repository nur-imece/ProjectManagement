using MongoDB.Driver;
using ProjectManagement.Data;
using ProjectManagement.Data.Entity;
using ProjectManagement.Model.Services.Interface;

namespace ProjectManagement.Model.Services.Implementation
{
    public class MovieServices: IMovieServices
    {
        private readonly MflixDbContext _context;

        public MovieServices(MflixDbContext context)
        {
            _context = context;
        }

        public List<Movie> GetAll()
        {
            return _context.Movies.Find(movie => true).ToList();
        }

        public async Task AddMovieAsync(Movie movie)
        {
            await _context.Movies.InsertOneAsync(movie);
        }
    }
}