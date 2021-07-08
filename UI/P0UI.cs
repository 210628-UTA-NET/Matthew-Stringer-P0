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
        static void Main(string[] args)
        {
            bool customerRepeat = true;

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
            while (customerRepeat) {
                p0class.Customer customer = new p0class.Customer();
                Console.WriteLine("Name?");
                customer.Name = Console.ReadLine();
                Console.WriteLine("Address?");
                customer.Address = Console.ReadLine();
                Console.WriteLine("Email?");
                customer.Email = Console.ReadLine();
                repo.AddCustomer(customer);
                Console.WriteLine("Continue? (y/n)");
                customerRepeat = Console.ReadLine() == "y";
            }
        }
    }
}
