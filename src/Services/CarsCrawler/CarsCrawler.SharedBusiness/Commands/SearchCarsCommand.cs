using CarsCrawler.Domain.Model;
using MassTransit.Topology;

namespace CarsCrawler.SharedBusiness.Commands
{
    [EntityName(Domain.Model.Consts.SearchCarsCommand)]
    public interface ISearchCarsCommand 
    {
        /// <summary>
        /// used or new car
        /// </summary>
        public int StockType { get; set; }
        /// <summary>
        /// Car Brands Name but we used brand id for this test case
        /// </summary>
        public int Makes { get; set; }
        /// <summary>
        /// Brand model     
        /// </summary>
        public int Models { get; set; }
        /// <summary>
        /// Car search price limit aka max price
        /// </summary>
        public int Price { get; set; }
        /// <summary>
        /// distance from your search location
        /// </summary>
        public int Distance { get; set; }
        /// <summary>
        /// Zip Code
        /// </summary>
        public string Zip { get; set; }
    }
    public class SearchCarsCommand : ISearchCarsCommand
    {
        public int StockType { get; set; }
        public int Makes { get; set; }
        public int Models { get; set; }
        public int Price { get; set; }
        public int Distance { get; set; }
        public string Zip { get; set; }
    }
}