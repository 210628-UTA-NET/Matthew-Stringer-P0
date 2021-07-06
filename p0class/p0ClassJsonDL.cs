using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace p0class
{
    public class JsonDatastore : Datastore
    {
        private string _filename = "./datafiles/datastore.json";
        private List<Customer> _customers;

        public bool AddCustomer(Customer p_cust)
        {
            _customers.Add(p_cust);
            string jsonString = JsonSerializer.Serialize<List<Customer>>(_customers);
            try
            {
                File.WriteAllText(_filename, jsonString);
            }
            catch(System.Exception)
            {
                throw new Exception("File path is invalid");
            }
            return true;
        }

        public List<Customer> LoadAllCustomer()
        {
            return new List<Customer>(_customers);
        }

        public Customer LoadCustomer(Customer _pcust)
        {
            throw new System.NotImplementedException();
        }

        public JsonDatastore()
        {
            string jsonString;
            try
            {
                jsonString = File.ReadAllText(_filename);
            }
            catch(System.Exception)
            {
                throw new Exception("File path is invalid");
            }

            _customers = JsonSerializer.Deserialize<List<Customer>>(jsonString);
        }
    }
}