using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ProjectManagement.Model;
using ProjectManagement.Model.Request;
using ProjectManagement.Model.Services;
using ProjectManagement.Model.Respond;
using Xunit;
using System.Threading.Tasks;
using Moq;

namespace TestProject1
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            // Arrange
            // MongoDBSettings örneği oluşturun ve yapılandırın
            var mongoDBSettings = new MongoDBSettings
            {
                ConnectionString = "mongodb://localhost:27017",
                DatabaseName = "TestDatabase",
                BooksCollectionName = "Books"
            };

            // IOptions<MongoDBSettings> nesnesini oluşturun
            var options = Options.Create(mongoDBSettings);

            // BooksService'i başlatın
            var bookService = new BooksService(options);

            // Test verisini hazırlayın
            var createRequest = new BookCreateRequest
            {
                Name = "Test Book",
                Price = 19,
                Category = "Fiction",
                Author = "Test Author"
            };

            // Act
            var response = await bookService.CreateAsync(createRequest);

            // Assert
            Assert.NotNull(response);
            Assert.Equal("Test Book", response.Name);
            Assert.Equal(19, response.Price);
            Assert.Equal("Fiction", response.Category);
            Assert.Equal("Test Author", response.Author);
        }
    }
}