using System;

namespace SjxLogistics.Models.DatabaseModels
{
    public class Bikes
    {
        public int id { get; set; }
        public string RidersName { get; set; }
        public string BikeNo { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
