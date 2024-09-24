using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using ProjectManagement.Data.Entity;
using ProjectManagement.Model.Services.Interface;

namespace ProjectManagement.Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class JobController : ODataController
{
    private readonly IJobServices _jobServices;

    public JobController(IJobServices jobServices)
    {
        _jobServices = jobServices;
    }
    [EnableQuery]
    [HttpGet]
    public List<Job>  Get()
    {
        var result = _jobServices.GetAll();
        return result;
    }


    [EnableQuery]
    [HttpPost]
    public async Task<ActionResult<Job>> Post([FromBody] Job job)
    {
        try
        {
            await _jobServices.AddJobAsync(job);
            return Ok();
        }
        catch (Exception ex)
        {
            
            return StatusCode(StatusCodes.Status500InternalServerError, "Error adding job");
        }
    }

}
