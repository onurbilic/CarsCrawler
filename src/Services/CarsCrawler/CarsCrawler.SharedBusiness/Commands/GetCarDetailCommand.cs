namespace CarsCrawler.SharedBusiness.Commands
{
    public class GetCarDetailCommand
    {
        public interface IVehicleDetailCommand
        {
            public string VehicleId { get; set; }
        }

        public class VehicleDetailCommand : IVehicleDetailCommand
        {
            public string VehicleId { get; set; }
        }
    }
}