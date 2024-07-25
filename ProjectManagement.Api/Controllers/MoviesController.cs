using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Data.Entity;
using ProjectManagement.Model.Services.Interface;

namespace ProjectManagement.Api.Controllers;

using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoviesController : ODataController
{
    private readonly IMovieServices _movieServices;

    public MoviesController(IMovieServices movieServices)
    {
        _movieServices = movieServices;
    }


    [EnableQuery]
    [HttpGet]
    public List<Movie>  Get()
    {
         var result = _movieServices.GetAll();
         return result;

    }
    

    [EnableQuery]
    [HttpPost]
    public async Task<ActionResult<Movie>> Post([FromBody] Movie movie)
    {
        try
        {
            await _movieServices.AddMovieAsync(movie);
            return Ok();
        }
        catch (Exception ex)
        {
            // Log the exception as needed.
            return StatusCode(StatusCodes.Status500InternalServerError, "Error adding movie");
        }
    }
    
}