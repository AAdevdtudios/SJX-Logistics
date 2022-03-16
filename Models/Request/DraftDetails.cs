using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SjxLogistics.Models.Request
{
    public class DraftDetails
    {
        public int id { get; set; }
        public string StartAddress { get; set; }
        public string EndAddress { get; set; }
        public int weight { get; set; }
        public string Categories { get; set; }
        public int price { get; set; }
    }
}
