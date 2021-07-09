using System;
using System.Collections.Generic;

#nullable disable

namespace p0class.Entities
{
    public partial class Order
    {
        public Order()
        {
            LineItems = new HashSet<LineItem>();
        }

        public int OId { get; set; }
        public string OLoc { get; set; }
        public decimal OPrice { get; set; }
        public int OStore { get; set; }
        public int? OCust { get; set; }

        public virtual Customer OCustNavigation { get; set; }
        public virtual StoreFront OStoreNavigation { get; set; }
        public virtual ICollection<LineItem> LineItems { get; set; }
    }
}
