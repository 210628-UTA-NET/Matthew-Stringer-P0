using System;
using p0class;
using System.Collections.Generic;

namespace P0UI
{
    class Program
    {
        static void Main(string[] args)
        {
            bool customerRepeat = true;
            JsonDatastore jsonDatastore = new JsonDatastore();
            List<Customer> customerList = jsonDatastore.LoadAllCustomer();
            while (customerRepeat) {
                Customer customer = new Customer();
                Console.WriteLine("Name?");
                customer.Name = Console.ReadLine();
                Console.WriteLine("Address?");
                customer.Address = Console.ReadLine();
                Console.WriteLine("Email?");
                customer.Email = Console.ReadLine();
                bool orderRepeat = true;
                while (orderRepeat) {
                    Console.WriteLine("Order? (empty line to finish)");
                    string order = Console.ReadLine();
                    if (order == "") {
                        orderRepeat = false;
                    } else {
                        customer.Orders.Add(order);
                    }
                }
                jsonDatastore.AddCustomer(customer);
                customerList = jsonDatastore.LoadAllCustomer();
                Console.WriteLine("Continue? (y/n)");
                customerRepeat = Console.ReadLine() == "y";
            }
            foreach (Customer customer in customerList) {
                Console.WriteLine("Name: " + customer.Name);
                Console.WriteLine("Address: " + customer.Address);
                Console.WriteLine("Email: " + customer.Email);
                Console.WriteLine("Orders:");
                foreach (string item in customer.Orders) {
                    Console.WriteLine(item);
                }
            }
        }
    }
}
