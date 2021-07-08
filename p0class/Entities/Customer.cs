using System;
using System.Collections.Generic;

#nullable disable

namespace p0class.Entities
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public int CId { get; set; }
        public string CName { get; set; }
        public string CAddr { get; set; }
        public string CEmail { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
