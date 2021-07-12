using System.Collections.Generic;
using p0class.Entities;
using System.Linq;

namespace p0class
{
    public class SQLDatastore
    {
        private MattStringer0Context _context;
        public SQLDatastore(MattStringer0Context p_context)
        {
            _context = p_context;
        }
        public class CustomerSearchResult
        {
            public int Id {get; set;}

            public string Name {get; set;}

            public string Address {get; set;}

            public string Email {get; set;}
        }

        public bool AddCustomer(Customer p_cust)
        {
            _context.Customers.Add(new Entities.Customer{
                CName = p_cust.Name,
                CAddr = p_cust.Address,
                CEmail = p_cust.Email
            });

            _context.SaveChanges();
            return true;
        }

        // Returns fully instantiated customers, as there are no nested classes
        public List<CustomerSearchResult> SearchCustomerByName(string p_name)
        {
            List<Customer> data = _context.Customers.Select(
                cust =>
                    new Customer
                    {
                        Id = cust.CId,
                        Name = cust.CName,
                        Address = cust.CAddr,
                        Email = cust.CEmail
                    }
            ).Where(x => x.Name == p_name).ToList();

            List<CustomerSearchResult> result = new List<CustomerSearchResult>();
            foreach(Customer datum in data)
            {
                result.Add(new CustomerSearchResult
                    {
                        Id = datum.Id,
                        Name = datum.Name,
                        Address = datum.Address,
                        Email = datum.Email
                    });
            }
            return result;
        }



        // Returns list of top-level details to avoid instantiating a lot of nested data
        public class StoreSearchResult
        {
            public int Id {set; get;}

            public string Name {set; get;}

            public string Address {set; get;}
        }
        public List<StoreSearchResult> SearchStoreFrontByName(string p_name)
        {
            List<StoreFront> data = _context.StoreFronts.Select(
                store =>
                    new StoreFront
                    {
                        Id = store.SId,
                        Name = store.SName,
                        Address = store.SAddr
                    }
            ).Where(x => x.Name == p_name).ToList();
            List<StoreSearchResult> resultList = new List<StoreSearchResult>();
            foreach (StoreFront datum in data)
            {
                resultList.Add(new StoreSearchResult
                    {
                        Id = datum.Id,
                        Name = datum.Name,
                        Address = datum.Address
                    });
            }
            return resultList;
        }

        private List<LineItem> LoadLineItemsById(int p_id, bool p_order)
        {
            var data = _context.LineItems.Join(
                _context.Products,
                item => item.LProd,
                prod => prod.PId,
                (item, prod) => new
                {
                    Quantity = item.LQuantity,
                    Order = item.LOrder,
                    StoreFront = item.LStorefront,
                    PId = prod.PId,
                    Name = prod.PName,
                    Price = prod.PPrice,
                    Desc = prod.PDesc,
                    Category = prod.PCategory
                }
            ).Where(x => (p_order ? x.Order : x.StoreFront) == p_id).ToList();

            List<LineItem> result = new List<LineItem>();
            foreach (var datum in data)
            {
                LineItem newLine = new LineItem {
                    Id = datum.PId,
                    Quantity = datum.Quantity,
                    Prod = new Product{
                        Id = datum.PId,
                        Name = datum.Name,
                        Price = datum.Price,
                        Description = datum.Desc,
                        Category = datum.Category
                    }
                };
                result.Add(newLine);
            }
            return result;
        }

        public StoreFront LoadStoreFrontById(int p_id)
        {
            StoreFront result = _context.StoreFronts.Select(
                store =>
                    new StoreFront
                    {
                        Id = store.SId,
                        Name = store.SName,
                        Address = store.SAddr
                    }
            ).Where(x => x.Id == p_id).First<StoreFront>();

            result.Orders = _context.Orders.Select(
                order =>
                    new Order
                    {
                        Id = order.OId,
                        Location = order.OLoc,
                        StoreFrontId = order.OStore
                    }
            ).Where(x => x.StoreFrontId == result.Id).ToList();

            foreach(Order order in result.Orders)
            {
                order.LineItems = LoadLineItemsById(order.Id, true);
            }

            result.Inventory = LoadLineItemsById(result.Id, false);
            return result;
        }
    }
}