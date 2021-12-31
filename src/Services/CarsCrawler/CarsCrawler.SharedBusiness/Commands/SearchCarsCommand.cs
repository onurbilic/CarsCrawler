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
        public string StockType { get; set; }
        /// <summary>
        /// Car Brands Name but we used brand id for this test case
        /// </summary>
        public string Makes { get; set; }
        /// <summary>
        /// Brand model     
        /// </summary>
        public string Models { get; set; }
        /// <summary>
        /// Car search price limit aka max price
        /// </summary>
        public string Price { get; set; }
        /// <summary>
        /// distance from your search location
        /// </summary>
        public string Distance { get; set; }
        /// <summary>
        /// Zip Code
        /// </summary>
        public string Zip { get; set; }
        /// <summary>
        /// start from specific page
        /// </summary>
        public int PageStart { get; set; }
        /// <summary>
        /// scrap only page count
        /// </summary>
        public int PageCount { get; set; }
    }
    public class SearchCarsCommand : ISearchCarsCommand
    {
        public string StockType { get; set; }
        public string Makes { get; set; }
        public string Models { get; set; }
        public string Price { get; set; }
        public string Distance { get; set; }
        public string Zip { get; set; }
        public int PageStart { get; set; }
        public int PageCount { get; set; }
    }
}