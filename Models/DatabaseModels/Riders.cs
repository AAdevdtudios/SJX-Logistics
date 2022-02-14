using System.Collections.Generic;

namespace SjxLogistics.Models.DatabaseModels
{
    public class Riders : Users
    {
        public Riders()
        {
            AssignedOrders = new HashSet<Order>();
        }

        public virtual ICollection<Order> AssignedOrders { get; set; }
        public virtual Bikes Bike { get; set; }

    }
}
