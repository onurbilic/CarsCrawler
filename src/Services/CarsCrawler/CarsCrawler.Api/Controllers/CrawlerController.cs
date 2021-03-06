using System.Net;
using CarsCrawler.Domain.Model;
using CarsCrawler.Infrastructure.Caching;
using CarsCrawler.Infrastructure.Repositories.Mongo;
using CarsCrawler.SharedBusiness.Commands;
using MassTransit;
using MassTransit.Monitoring.Health;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CarsCrawler.API.Controllers
{

    [ApiController]
    public class CrawlerController : ControllerBase
    {
        private readonly IMongoRepository<Vehicle> _vehicleRepository;

        private readonly IConfiguration _configuration;

        // private readonly ICacheService _cacheService;
        private readonly IBus _bus;

        public CrawlerController(
            IMongoRepository<Vehicle> vehicleRepository,
            IConfiguration configuration,
            IBus bus)
        {
            _vehicleRepository = vehicleRepository;
            _configuration = configuration;
            // _cacheService = cacheService;
            _bus = bus;
        }


        [HttpPost]
        [Route("login")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult Login(LoginModel login)
        {
            return Ok();
        }

        [HttpPost]
        [Route("search")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task SearchCars(SearchModel searchModel)
        {
            //TODO : we can use mediatr pattern and CQRS pattern to post and put commands. But deadline is tight for the test case
            try
            {
                await _bus.Publish<ISearchCarsCommand>(new
                {
                    Distance = searchModel.Distance,
                    Price = searchModel.Price,
                    Zip = searchModel.Zip,
                    Models = searchModel.Models,
                    Makes = searchModel.Makes,
                    StockType = searchModel.StockType,
                    PageStart = searchModel.PageStart,
                    PageCount = searchModel.PageCount
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        [Route("getcars")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public List<Vehicle> Get()
        {
            return _vehicleRepository.AsQueryable().ToList();
        }
    }
}