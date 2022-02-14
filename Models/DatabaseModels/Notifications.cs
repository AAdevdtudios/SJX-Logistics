using System;

namespace SjxLogistics.Models.DatabaseModels
{
    public class Notifications
    {
        public int Id { get; set; }
        public string From { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string MessageType { get; set; }

    }
}
