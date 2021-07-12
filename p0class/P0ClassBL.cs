using System.Collections.Generic;

namespace p0class
{
    public class Customer
    {
        public int Id {set; get;}
        public string Name {set; get;}

        public string Address {set; get;}

        public string Email {set; get;}

        public List<Order> Orders {set; get;}

        public Customer()
        {
            this.Orders = new List<Order>();
        }
    }

    public class Product
    {
        public int Id {set; get;}
        public string Name {set; get;}

        public decimal Price {set; get;}

        public string Description {set; get;}

        public string Category {set; get;}
    }

    public class LineItem
    {
        public int Id {set; get;}
        public Product Prod {set; get;}

        public int Quantity {set; get;}
    }

    public class StoreFront
    {
        public int Id {set; get;}
        public string Name {set; get;}

        public string Address {set; get;}

        public List<LineItem> Inventory {set; get;}

        public List<Order> Orders {set; get;}

        public StoreFront()
        {
            this.Inventory = new List<LineItem>();
            this.Orders = new List<Order>();
        }
    }

    public class Order
    {
        public int Id {set; get;}
        public List<LineItem> LineItems {set; get;}

        public string Location {set; get;}

        public decimal TotalPrice {set; get;}

        public int CustomerId {set; get;}

        public int StoreFrontId {set; get;}

        public Order()
        {
            this.LineItems = new List<LineItem>();
        }
    }
}