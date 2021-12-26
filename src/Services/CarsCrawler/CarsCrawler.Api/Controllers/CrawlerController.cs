using System.Net;
using CarsCrawler.Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace CarsCrawler.API.Controllers;

[ApiController]
public class CrawlerController : ControllerBase
{
    [HttpPost]
    [Route("login")]
    [ProducesResponseType((int) HttpStatusCode.NotFound)]
    [ProducesResponseType((int) HttpStatusCode.BadRequest)]
    public IActionResult Login(LoginModel login)
    {
        return Ok();
    }
    
    [HttpPost]
    [Route("search")]
    [ProducesResponseType((int) HttpStatusCode.NotFound)]
    [ProducesResponseType((int) HttpStatusCode.BadRequest)]
    public IActionResult SearchCars(SearchModel searchModel)
    {
        return Ok();
    }
}