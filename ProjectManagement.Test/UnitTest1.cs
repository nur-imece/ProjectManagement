using MongoDB.Driver;
using ProjectManagement.Data;
using ProjectManagement.Data.Entity;
using ProjectManagement.Model.Services.Implementation;
using Moq;

namespace ProjectManagement.Test
{
    [TestFixture]
    public class MovieServiceTests
    {
        [Test]
        public async Task AddMovieAsync_ShouldAddMovieSuccessfully()
        {

            // MongoClient mock'u
            var mockMongoClient = new Mock<IMongoClient>();
            var mockDatabase = new Mock<IMongoDatabase>();
            var mockMovieCollection = new Mock<IMongoCollection<Movie>>();

            mockMongoClient.Setup(client => client.GetDatabase(It.IsAny<string>(), null)).Returns(mockDatabase.Object);
            mockDatabase.Setup(db => db.GetCollection<Movie>(It.IsAny<string>(), null)).Returns(mockMovieCollection.Object);

            // MflixDbContext ve MovieService'i başlatın
            var context = new MflixDbContext(mockDatabase.Object);
            var movieService = new MovieServices(context);

            var movie = new Movie
            {
                Id = "66fab442dc5a27a5b78f6d21",
                Title = "Test Movie",
                Plot = "This is a test movie.",
                Genres = new[] { "Drama" },
                Cast = new[] { "Test Actor" }
            };

            // Act
            await movieService.AddMovieAsync(movie);

            // Assert
            mockMovieCollection.Verify(
                x => x.InsertOneAsync(movie, null, default),
                Times.Once,
                "Movie should be added to the collection."
            );
        }

        [Test]
        public void GetAll_ShouldReturnMoviesSuccessfully()
        {
            // Arrange
            var mockMongoClient = new Mock<IMongoClient>();
            var mockDatabase = new Mock<IMongoDatabase>();
            var mockMovieCollection = new Mock<IMongoCollection<Movie>>();

            // Fake data for movies
            var movieList = new List<Movie>
            {
                new Movie { Id = "1", Title = "Test Movie 1", Plot = "Plot 1" },
                new Movie { Id = "2", Title = "Test Movie 2", Plot = "Plot 2" }
            };

            // Cursor simulation
            var mockCursor = new Mock<IAsyncCursor<Movie>>();
            mockCursor.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
            mockCursor.Setup(c => c.Current).Returns(movieList);

            mockMovieCollection.Setup(x => x.FindSync(It.IsAny<FilterDefinition<Movie>>(), It.IsAny<FindOptions<Movie, Movie>>(), default))
                .Returns(mockCursor.Object);

            mockDatabase.Setup(db => db.GetCollection<Movie>(It.IsAny<string>(), null)).Returns(mockMovieCollection.Object);
            mockMongoClient.Setup(client => client.GetDatabase(It.IsAny<string>(), null)).Returns(mockDatabase.Object);

            // MflixDbContext ve MovieService'i başlatın
            var context = new MflixDbContext(mockDatabase.Object);
            var movieService = new MovieServices(context);

            // Act
            var movies = movieService.GetAll();

            // Assert
            Assert.NotNull(movies);
            Assert.AreEqual(2, movies.Count);
            Assert.AreEqual("Test Movie 1", movies[0].Title);
            Assert.AreEqual("Test Movie 2", movies[1].Title);
        }
    }
}
