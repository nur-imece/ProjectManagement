using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using ProjectManagement.Data.Entity;
using ProjectManagement.Model.Services.Interface;

namespace ProjectManagement.Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class CommentController : ODataController
{
    private readonly ICommentServices _commentServices;

    public CommentController(ICommentServices commentServices)
    {
        _commentServices = commentServices;
    }

    [EnableQuery]
    [HttpGet]
    public List<Comment> Get()
    {
        var result = _commentServices.GetAll();
        return result;

    }


    [EnableQuery]
    [HttpPost]
    public async Task<ActionResult<Movie>> Post([FromBody] Comment comment)
    {
        try
        {
            await _commentServices.AddCommentAsync(comment);
            return Ok();
        }
        catch (Exception ex)
        {
            // Log the exception as needed.
            return StatusCode(StatusCodes.Status500InternalServerError, "Error adding Comment");
        }
    }
}
