using System;
using System.Collections.Generic;

#nullable disable

namespace p0class.Entities
{
    public partial class LineItem
    {
        public int LId { get; set; }
        public int LProd { get; set; }
        public int? LOrder { get; set; }
        public int? LStorefront { get; set; }
        public int LQuantity { get; set; }

        public virtual Order LOrderNavigation { get; set; }
        public virtual Product LProdNavigation { get; set; }
        public virtual StoreFront LStorefrontNavigation { get; set; }
    }
}
