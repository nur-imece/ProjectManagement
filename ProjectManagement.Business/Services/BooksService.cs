using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ProjectManagement.Model.Request;
using ProjectManagement.Model.Respond;

namespace ProjectManagement.Model.Services
{
    public class BooksService : IBookService
    {
        private readonly IMongoCollection<Book> _booksCollection;

        public BooksService(IOptions<MongoDBSettings> mongoDbSettings)
        {
            var mongoClient = new MongoClient(
                mongoDbSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                mongoDbSettings.Value.DatabaseName);

            _booksCollection = mongoDatabase.GetCollection<Book>(
                mongoDbSettings.Value.BooksCollectionName);
        }

        public async Task<List<BookResponse>> GetAsync()
        {
            var result = await _booksCollection.Find(_ => true).ToListAsync();
            return result.Select(book => new BookResponse
            { 
                Name = book.BookName,
                Author = book.Author,
                Category = book.Category,
                Price = book.Price
               
            }).ToList();
        }

        public async Task<BookResponse?> GetAsync(string id)
        {
            var book = await _booksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (book is null)
            {
                return null;
            }
            return new BookResponse
            {
                Name = book.BookName,
                Author = book.Author,
                Category = book.Category,
                Price = book.Price
            };
        }

        public async Task CreateAsync(BookCreateRequest newBook)
        {
            var book = new Book
            {
                BookName = newBook.Name,
                Author = newBook.Author,
                Category = newBook.Category,
                Price = newBook.Price,
            };
            await _booksCollection.InsertOneAsync(book);
        }

        public async Task UpdateAsync(string id, BookCreateRequest updatedBook)
        {
            var book = new Book
            {
                // Map BookCreateRequest to Book here
                Id = id,
                BookName = updatedBook.Name,
                Author = updatedBook.Author,
                Category = updatedBook.Category,
                Price = updatedBook.Price,
            };
            await _booksCollection.ReplaceOneAsync(x => x.Id == id, book);
        }

        public async Task RemoveAsync(string id)
        {
            await _booksCollection.DeleteOneAsync(x => x.Id == id);
        }
    }
}
