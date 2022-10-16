using System.Globalization;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers;


[ApiController]
[Route("catan")]
[EnableCors("MyPolicy")]
public class CatanController : ControllerBase
{
    private readonly ILogger<CatanController> _logger;
    private static Dictionary<int, DiceGenerator> AllCookies = new Dictionary<int, DiceGenerator>();
    private static JsonSerializerOptions Options = new JsonSerializerOptions
    {
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
        WriteIndented = true
    };

    public CatanController(ILogger<CatanController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<Object> GetMap()
    {
        object locker = new();
        lock (locker)
        {
            //_logger.LogInformation(result);
            return  Ok(JsonSerializer.Serialize(MapGenerator.GenerateMap()));
        }
    }
    
    [HttpGet("dice")]
    public ActionResult<Object> GetDice(int id)
    {
        // var guid = "";
        // if (Request.Cookies.Count == 0)
        // {
        //     guid = Guid.NewGuid().ToString();
        //     HttpContext.Response.Cookies.Append("guid", guid, new Microsoft.AspNetCore.Http.CookieOptions { 
        //         Expires = DateTime.Now.AddDays(1)
        //     } );
        // }
        // else
        // {
        //     guid = Request.Cookies["guid"];
        // }
        
        
        object locker = new();
        lock (locker)
        {
            if (!AllCookies.ContainsKey(id))
                AllCookies[id] = new DiceGenerator();
            
            //System.IO.File.AppendAllText($"log_{id}", result);
            return  Ok(JsonSerializer.Serialize(AllCookies[id].GetDiceResponse(), Options));
        }
    }
}