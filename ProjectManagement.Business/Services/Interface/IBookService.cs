using ProjectManagement.Model.Request;
using ProjectManagement.Model.Respond;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagement.Model
{
    public interface IBookService
    {
        Task<List<BookResponse>> GetAsync();
        Task<BookResponse?> GetAsync(string id);
        Task CreateAsync(BookCreateRequest newBook);
        Task UpdateAsync(string id, BookCreateRequest updatedBook);
        Task RemoveAsync(string id);
    }
}