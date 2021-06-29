using System;
using p0class;

namespace UI
{
    class Program
    {
        static void Main(string[] args)
        {
            Customer customer = new Customer();
            Console.WriteLine("Name?");
            customer.Name = Console.ReadLine();
            Console.WriteLine("Address?");
            customer.Address = Console.ReadLine();
            Console.WriteLine("Email?");
            customer.Email = Console.ReadLine();
            bool repeat = true;
            while (repeat) {
                Console.WriteLine("Order? (empty line to finish)");
                string order = Console.ReadLine();
                if (order == "") {
                    repeat = false;
                } else {
                    customer.Orders.Add(order);
                }
            }
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
