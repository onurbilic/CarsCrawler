using System;
using System.Threading;
using System.Threading.Tasks;
using CarsCrawler.Domain.Model;
using CarsCrawler.Infrastructure.Repositories.Mongo;
using CarsCrawler.SharedBusiness.Commands;
using MassTransit;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CarsCrawler.Consumers.Producer
{
    public class CarDetailProducer : BackgroundService
    {
        readonly IBus _bus;
        private readonly IMongoRepository<Vehicle> _vehicleRepo;
        private readonly IMongoRepository<VehicleDetail> _vehicleDetailRepo;

        public CarDetailProducer(IBus bus,
            IMongoRepository<Vehicle> vehicleRepo,
            IMongoRepository<VehicleDetail> vehicleDetailRepo)
        {
            _bus = bus;
            _vehicleDetailRepo = vehicleDetailRepo;
            _vehicleRepo = vehicleRepo;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                IEnumerable<Vehicle> vehicles = _vehicleRepo.FilterBy(p => p.Status == 1);

                foreach (var item in vehicles)
                {
                    await _bus.Publish<IVehicleDetailCommand>(new VehicleDetailCommand()
                    {
                        VehicleId = item.carId
                    }, stoppingToken);
                    
                    var update = Builders<Vehicle>.Update.Set("Status", 1);

                    await _vehicleRepo.UpdateOneAsync(f => f.carId == item.carId, update);
                }

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}