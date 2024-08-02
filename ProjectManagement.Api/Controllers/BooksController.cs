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
        public async Task<ActionResult<BookResponse>> Get()

        {
            var result= await _booksService.GetAsync();
            return Ok(result);
            }

        [HttpGet("{id:length(24)}", Name = "GetBook")]
        public async Task<ActionResult<BookResponse>> Get(string id)
        {
            await _booksService.GetAsync(id);
            
                return Ok();
        }

         
        [HttpPost]
        public async Task<IActionResult> Post(BookCreateRequest newBookRequest)
        {

             await _booksService.CreateAsync(newBookRequest);
             return Ok();


        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, BookCreateRequest updatedBookRequest)
        {
            await _booksService.UpdateAsync(id, updatedBookRequest);

            return Ok();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _booksService.RemoveAsync(id);

            return NoContent();
        }
    }
}
