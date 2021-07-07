using System;
using System.Collections.Generic;

#nullable disable

namespace p0class.Entities
{
    public partial class Product
    {
        public Product()
        {
            LineItems = new HashSet<LineItem>();
        }

        public int PId { get; set; }
        public string PName { get; set; }
        public decimal PPrice { get; set; }
        public string PDesc { get; set; }
        public string PCategory { get; set; }

        public virtual ICollection<LineItem> LineItems { get; set; }
    }
}
