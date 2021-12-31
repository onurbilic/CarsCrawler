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
    }
}
