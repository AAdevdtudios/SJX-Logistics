using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SjxLogistics.Models.Request
{
    public class NotificationRequests
    {
        public string OrderCode { get; set; }
        public string RefNo { get; set; }
        public string Message { get; set; }
        public string MessageType { get; set; }

    }
}
