using System;
using p0class;
using System.Collections.Generic;
using p0class.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace P0UI
{
    class Program
    {
        static void MainMenu(SQLDatastore datastore)
        {
            bool looping = true;

            while (looping)
            {
                Console.WriteLine("Select:");
                Console.WriteLine("0: Exit");
                Console.WriteLine("1. Add Customer");
                Console.WriteLine("2. Search Customer");
                switch(Console.ReadLine())
                {
                    case "0":
                        looping = false;
                        break;
                    case "1":
                        p0class.Customer customer = new p0class.Customer();
                        Console.WriteLine("Name?");
                        customer.Name = Console.ReadLine();
                        Console.WriteLine("Address?");
                        customer.Address = Console.ReadLine();
                        Console.WriteLine("Email?");
                        customer.Email = Console.ReadLine();
                        datastore.AddCustomer(customer);
                        break;
                    case "2":
                        Console.WriteLine("Enter Customer Name:");
                        foreach (p0class.Customer cust in datastore.SearchCustomerByName(Console.ReadLine()))
                        {
                            Console.WriteLine($"{cust.Name}, {cust.Address}, {cust.Email}");
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid Entry, try again.");
                        break;
                }
            }
        }
        static void Main(string[] args)
        {
            //Get the configuration from our appsetting.json file
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsetting.json")
                .Build();

            //Grabs our connectionString from our appsetting.json
            string connectionString = configuration.GetConnectionString("DB0");

            DbContextOptions<MattStringer0Context> options = new DbContextOptionsBuilder<MattStringer0Context>()
                .UseSqlServer(connectionString)
                .Options;

            SQLDatastore repo = new SQLDatastore(new MattStringer0Context(options));
            MainMenu(repo);
        }
    }
}
