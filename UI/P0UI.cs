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
        public enum MenuState
        {
            MainMenu,
            StoreFrontMenu,
            ViewInventory,
            PlaceOrder,
            OrderHistory,
            Replenish,
            Exit

        }

        static List<int> SearchCustomer(SQLDatastore datastore)
        {
            Console.WriteLine("Enter Customer Name:");

            List<int> result = new List<int>();

            foreach (SQLDatastore.CustomerSearchResult cust in datastore.SearchCustomerByName(Console.ReadLine()))
            {
                Console.WriteLine($"{cust.Name}, {cust.Address}, {cust.Email}");
                result.Add(cust.Id);
            }
            return result;
        }

        static List<int> SearchStoreFront(SQLDatastore datastore)
        {
            Console.WriteLine("Enter Store Name:");

            List<int> result = new List<int>();

            foreach (SQLDatastore.StoreSearchResult store in datastore.SearchStoreFrontByName(Console.ReadLine()))
            {
                Console.WriteLine($"{store.Name}, {store.Address}");
                result.Add(store.Id);
            }
            return result;
        }

        static MenuState MainMenu(SQLDatastore datastore)
        {
            bool looping = true;

            while (looping)
            {
                Console.WriteLine("Select:");
                Console.WriteLine("0: Exit");
                Console.WriteLine("1. Add Customer");
                Console.WriteLine("2. Search Customer");
                Console.WriteLine("3. Search Store Fronts");
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
                        SearchCustomer(datastore);
                        break;
                    case "3":
                        SearchStoreFront(datastore);
                        break;
                    default:
                        Console.WriteLine("Invalid Entry, try again.");
                        break;
                }
            }
            return MenuState.Exit;
        }

        static void MenuStateMachine(SQLDatastore datastore)
        {
            MenuState state = MenuState.MainMenu;

            while (state != MenuState.Exit)
            {
                switch(state)
                {
                    case MenuState.MainMenu:
                        state = MainMenu(datastore);
                        break;
                    case MenuState.Exit:
                        return;
                    default:
                        throw new InvalidProgramException("Invalid Menu State");
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
            MenuStateMachine(repo);
        }
    }
}
