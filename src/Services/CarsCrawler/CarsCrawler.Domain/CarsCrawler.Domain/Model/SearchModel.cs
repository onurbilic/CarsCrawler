using CarsCrawler.Domain.Extension;
using CarsCrawler.Domain.SeedWork;

namespace CarsCrawler.Domain.Model
{
    [BsonCollection("CarSearch")]
    public class SearchModel : IDocument.Document
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
}