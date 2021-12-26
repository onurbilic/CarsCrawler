using System.Net;
using CarsCrawler.Domain.Model;
using CarsCrawler.Infrastructure.Caching;
using CarsCrawler.Infrastructure.Repositories.Mongo;
using CarsCrawler.SharedBusiness.Commands;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace CarsCrawler.API.Controllers;

[ApiController]
public class CrawlerController : ControllerBase
{
    private readonly IMongoRepository<SearchModel> _carSearchRepository;
    private readonly IConfiguration _configuration;
    private readonly ICacheService _cacheService;
    private readonly IBus _bus;

    public CrawlerController(           
        IMongoRepository<SearchModel> carSearchRepository,
        IConfiguration configuration,
        ICacheService cacheService, IBus bus)
    {
        _carSearchRepository = carSearchRepository;
        _configuration = configuration;
        _cacheService = cacheService;
        _bus = bus;
    }

    
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
    public async Task SearchCars(SearchModel? searchModel)
    {
        if (searchModel != null)
        {
            await _bus.Publish<ISearchCarsCommand>(searchModel);
        }
    }
}