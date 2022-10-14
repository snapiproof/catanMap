using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers;

[ApiController]
[Route("[controller]")]
public class CatanController : ControllerBase
{
    private readonly ILogger<CatanController> _logger;

    public CatanController(ILogger<CatanController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetGeneration")]
    public JsonContent Get()
    {
        return JsonContent.Create("");
    }
}