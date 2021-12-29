using CarsCrawler.Domain.Extension;
using CarsCrawler.Domain.SeedWork;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsCrawler.Domain.Model
{
    [BsonCollection("Vehicles")]
    public class Vehicle : IDocument.Document
    {
        public string carId { get; set; }
        public string title { get; set; }
        public string image { get; set; }
        public string stockType { get; set; }
        public string miles { get; set; }
        public string price { get; set; }
        public string reportLink { get; set; }
        public string dealerName { get; set; }
        public string rating { get; set; }
    }
}
