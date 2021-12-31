using CarsCrawler.Domain.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsCrawler.Domain.SeedWork;

namespace CarsCrawler.Domain.Model
{
    [BsonCollection("VehicleDetail")]
    public class VehicleDetail : IDocument.Document
    {
        public string carId { get; set; }
        public string stockType { get; set; }
        public string title { get; set; }
        public string mileage { get; set; }
        public string price { get; set; }
        public BasicInfoItem basicInfo { get; set; }
        public FutureInfoItem futureInfo { get; set; }
        public string sellerName { get; set; }
        public string dealerPhone { get; set; }
        public string rating { get; set; }
        public string dealerAddress { get; set; }
        public string extLink { get; set; }
        public string sellerNotes { get; set; }
    }

    public class BasicInfoItem
    {
        public string key { get; set; }
        public string value { get; set; }
    }

    public class FutureInfoItem
    {
        public string key { get; set; }
        public string value { get; set; }
    }

}
