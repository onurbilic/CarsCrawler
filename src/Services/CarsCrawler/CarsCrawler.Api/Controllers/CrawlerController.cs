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
        private readonly IMongoRepository<SearchModel> _carSearchRepository;

        private readonly IConfiguration _configuration;

        // private readonly ICacheService _cacheService;
        private readonly IBus _bus;

        public CrawlerController(
            IMongoRepository<SearchModel> carSearchRepository,
            IConfiguration configuration,
            IBus bus)
        {
            _carSearchRepository = carSearchRepository;
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
                    Model = searchModel.Models,
                    Makes = searchModel.Makes,
                    StockType = searchModel.StockType
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
        public IActionResult Get()
        {
            return Ok();
        }
        //public HealthResult CheckHealth()
        //{
        //    IEndpointHealth endpointHealth;
        //    var endpointHealthResult = endpointHealth.CheckHealth();

        //    var data = new Dictionary<string, object> { ["Endpoints"] = endpointHealthResult.Data };

        //    return _healthy && endpointHealthResult.Status == BusHealthStatus.Healthy
        //        ? HealthResult.Healthy("Ready", data)
        //        : HealthResult.Unhealthy($"Not ready: {_failureMessage}", data: data);
        //}

    }
}