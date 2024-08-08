using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MyController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Bu endpoint sadece doğrulanmış kullanıcılar tarafından erişilebilir.");
    }
}