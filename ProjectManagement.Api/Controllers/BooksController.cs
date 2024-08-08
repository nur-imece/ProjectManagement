using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Model;
using ProjectManagement.Model.Request;
using ProjectManagement.Model.Respond;

namespace ProjectManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _booksService;

        public BooksController(IBookService booksService) =>
            _booksService = booksService;

        [HttpGet]
        public async Task<ActionResult<List<BookResponse>>> Get()
        {
            var result = await _booksService.GetAsync();
            if (result == null || !result.Any())
            {
                return NotFound("No books found.");
            }
            return Ok(result);
        }

        [HttpGet("{id:length(24)}", Name = "GetBook")]
        public async Task<ActionResult<BookResponse>> Get(string id)
        {
            var book = await _booksService.GetAsync(id);
            if (book == null)
            {
                return NotFound($"Book with ID {id} not found.");
            }
            return Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> Post(BookCreateRequest newBookRequest)
        {
            if (newBookRequest == null)
            {
                return BadRequest("Book data is null.");
            }

            await _booksService.CreateAsync(newBookRequest);
            return Ok("Book created successfully.");
        }


        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, BookCreateRequest updatedBookRequest)
        {
            if (updatedBookRequest == null)
            {
                return BadRequest("Book data is null.");
            }

            var existingBook = await _booksService.GetAsync(id);
            if (existingBook == null)
            {
                return NotFound($"Book with ID {id} not found.");
            }

            await _booksService.UpdateAsync(id, updatedBookRequest);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existingBook = await _booksService.GetAsync(id);
            if (existingBook == null)
            {
                return NotFound($"Book with ID {id} not found.");
            }

            await _booksService.RemoveAsync(id);
            return NoContent();
        }
    }
}
