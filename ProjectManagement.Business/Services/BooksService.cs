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



        public async  Task<List<BookResponse>> GetAsync()
        {
            var book = await GetAsync();
            if (book is null)
            {
                return;
            }
            await _booksCollection.Find(_ => true).ToListAsync();

        }

        public async  Task<BookResponse?> GetAsync(string id)
        {
            var book = await GetAsync(id);
            if (book is null)
            {
                return;
            }
            await _booksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }
            

        public async Task CreateAsync(BookCreateRequest newBook)
        {
            var book = await GetAsync(id);
            
            if (book is null)
            {
                return;
            }
            await _booksCollection.InsertOneAsync(newBook);
        }
           

        public async Task UpdateAsync(string id, BookCreateRequest updatedBook)
        {
            var book = await GetAsync(id);

            if (book is null)
            {
                return;
            }
            
            await _booksCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);

        }

        public async Task RemoveAsync(string id)
        {
            var book = await GetAsync(id);

            if (book is null)
            {
                return;
            }

            await _booksCollection.DeleteOneAsync(x => x.Id == id);
            
        }
            
            
    }
}