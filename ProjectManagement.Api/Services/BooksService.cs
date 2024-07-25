using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ProjectManagement.Model;
using ProjectManagement.API2.Services;
using MongoDB.Bson; // ObjectId için gerekli

namespace ProjectManagement.API2.Services
{
    public class BooksService
    {
        private readonly IMongoCollection<Book> _booksCollection;

        public BooksService(
            IOptions<BookStoreDatabaseSettings> bookStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                bookStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                bookStoreDatabaseSettings.Value.DatabaseName);

            _booksCollection = mongoDatabase.GetCollection<Book>(
                bookStoreDatabaseSettings.Value.BooksCollectionName);
        }

        public async Task<List<Book>> GetAsync() =>
            await _booksCollection.Find(_ => true).ToListAsync();

        public async Task<Book?> GetAsync(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return null; // veya uygun bir hata yönetimi
            }

            return await _booksCollection.Find(x => x.Id == objectId).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Book newBook) =>
            await _booksCollection.InsertOneAsync(newBook);

        public async Task UpdateAsync(string id, Book updatedBook)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return; // veya uygun bir hata yönetimi
            }

            await _booksCollection.ReplaceOneAsync(x => x.Id == objectId, updatedBook);
        }

        public async Task RemoveAsync(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return; // veya uygun bir hata yönetimi
            }

            await _booksCollection.DeleteOneAsync(x => x.Id == objectId);
        }
    }
}