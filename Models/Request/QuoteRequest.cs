using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SjxLogistics.Models.Request
{
    public class QuoteRequest
    {
        public double sLag { get; set; }
        public double sLat { get; set; }
        public double eLag { get; set; }
        public double eLat { get; set; }
        public int weight { get; set; }
    }
}
