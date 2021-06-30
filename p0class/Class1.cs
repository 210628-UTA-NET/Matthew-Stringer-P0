using System.Collections.Generic;

namespace p0class
{
    public class Customer
    {
        public string Name {set; get;}

        public string Address {set; get;}

        public string Email {set; get;}

        public List<string> Orders {get;}

        public Customer()
        {
            Orders = new List<string>();
        }
    }
}