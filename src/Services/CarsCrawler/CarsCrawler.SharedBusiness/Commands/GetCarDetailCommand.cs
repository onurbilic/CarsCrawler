using MassTransit.Topology;

namespace CarsCrawler.SharedBusiness.Commands
{
    [EntityName(Domain.Model.Consts.GetCarDetailCommand)]
    public interface IVehicleDetailCommand
    {
        public string VehicleId { get; set; }
    }

    public class VehicleDetailCommand : IVehicleDetailCommand
    {
        public string VehicleId { get; set; }
    }
}