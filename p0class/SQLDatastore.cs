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
        public class CustomerLoadResult
        {

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

        public List<Customer> SearchCustomerByName(string p_name)
        {
            return _context.Customers.Select(
                cust =>
                    new Customer
                    {
                        Name = cust.CName,
                        Address = cust.CAddr,
                        Email = cust.CEmail
                    }
            ).Where(x => x.Name == p_name).ToList();
        }
    }
}