using System;
using p0class;
using System.Collections.Generic;

namespace UI
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Customer> customerList = new List<Customer>();

            bool customerRepeat = true;
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
                customerList.Add(customer);
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
