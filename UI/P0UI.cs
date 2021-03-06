using System;
using p0class;
using System.Collections.Generic;
using p0class.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;

namespace P0UI
{
    class Program
    {
        public enum ChoosingChoice
        {
            Customer,
            StoreFront,
            Product,
            DefaultState
        }
        static List<int> SearchCustomer(SQLDatastore datastore)
        {
            Console.WriteLine("Enter Customer Name or Part of Name:");

            List<int> result = new List<int>();
            int i = 1;
            string search = Console.ReadLine();
            Console.WriteLine("#,Name,Address,Email");
            foreach (SQLDatastore.CustomerSearchResult cust in datastore.SearchCustomerByName(search))
            {
                Console.WriteLine($"{i},{cust.Name},{cust.Address},{cust.Email}");
                result.Add(cust.Id);
                i += 1;
            }
            return result;
        }

        static List<int> SearchStoreFront(SQLDatastore datastore)
        {
            Console.WriteLine("Enter Store Name or Part Of Name:");

            List<int> result = new List<int>();
            int i = 1;
            string search = Console.ReadLine();
            Console.WriteLine("#,Name,Address");
            foreach (SQLDatastore.StoreSearchResult store in datastore.SearchStoreFrontByName(search))
            {
                Console.WriteLine($"{i},{store.Name},{store.Address}");
                result.Add(store.Id);
                i++;
            }
            return result;
        }

        static int SelectChoice(SQLDatastore datastore, ChoosingChoice subject)
        {
            string subjectStr;
            List<int> choices;
            switch (subject)
            {
                case ChoosingChoice.Customer:
                    subjectStr = "customer";
                    choices = SearchCustomer(datastore);
                    break;
                case ChoosingChoice.StoreFront:
                    subjectStr = "store";
                    choices = SearchStoreFront(datastore);
                    break;
                default:
                    throw new InvalidProgramException("Program flow error: SelectChoice");
            }
            while (true)
            {
                switch (choices.Count)
                {
                    case 0:
                        return -1;
                    case 1:
                        Console.WriteLine($"Selecting your matching {subjectStr}.");
                        return choices[0];
                    default:
                        Console.WriteLine($"Select a {subjectStr} by the number on the left of the listing, or 0 to return.");
                        int selection;
                        bool result = int.TryParse(Console.ReadLine(), out selection);
                        if (result) {
                            if (selection == 0)
                                return -1;
                            if (selection < 1 || selection > choices.Count)
                                Console.WriteLine("Invalid entry, please try again.");
                            else
                                return choices[selection-1];
                        } else
                            Console.WriteLine("Invalid entry, please try again.");
                        break;
                }
            }
            throw new InvalidProgramException("Program flow error: SelectChoice");
        }

        static void AddCustomerUI(SQLDatastore datastore)
        {
            p0class.Customer customer = new p0class.Customer();
            Console.WriteLine("Name?");
            customer.Name = Console.ReadLine();
            Console.WriteLine("Address?");
            customer.Address = Console.ReadLine();
            Console.WriteLine("Email?");
            customer.Email = Console.ReadLine();
            datastore.AddCustomer(customer);
        }

        static void ListLineItems(List<p0class.LineItem> p_items)
        {
            Console.WriteLine("#,Quantity,Name,Price,Description");
            int i = 1;
            foreach(p0class.LineItem item in p_items)
            {
                Console.WriteLine($"{i},{item.Quantity},{item.Prod.Name},{item.Prod.Price},{item.Prod.Description}");
                i += 1;
            }
        }

        static string ChooseNonemptyString(string p_prompt)
        {
            Console.WriteLine(p_prompt);
            string result = Console.ReadLine();
            while (result == "")
            {
                Console.WriteLine("Empty entry not allowed, please try again.");
                result = Console.ReadLine();
            }
            return result;
        }

        static void StoreFrontInventoryUI(SQLDatastore datastore)
        {
            int userChoice = SelectChoice(datastore, ChoosingChoice.StoreFront);

            if (userChoice != -1)
                ListLineItems(datastore.LoadStoreFrontById(userChoice).Inventory);
        }

        static void CreateOrderUI(SQLDatastore datastore)
        {
            int selectStore = SelectChoice(datastore, ChoosingChoice.StoreFront);
            if (selectStore == -1)
                return;
            int selectCustomer = SelectChoice(datastore, ChoosingChoice.Customer);
            if (selectCustomer == -1)
                return;
            p0class.StoreFront store = datastore.LoadStoreFrontById(selectStore);
            ListLineItems(store.Inventory);
            List<p0class.LineItem> modifiedItems = new List<p0class.LineItem>();
            p0class.Order newOrder = new p0class.Order();
            newOrder.StoreFrontId = selectStore;
            newOrder.CustomerId = selectCustomer;
            newOrder.Location = ChooseNonemptyString("Enter the address for delivery.");
            bool repeat = true;
            while (repeat)
            {
                Console.WriteLine("Item number? 0 to finish");
                int selectedInventory;
                bool result = int.TryParse(Console.ReadLine(), out selectedInventory);
                if (result) {
                    if (selectedInventory == 0)
                        repeat = false;
                    else if (selectedInventory >= 1 && selectedInventory <= store.Inventory.Count)
                    {
                        p0class.LineItem selectedProductInventory = store.Inventory[selectedInventory-1];
                        p0class.LineItem existingOrder = newOrder.LineItems.Where(x => x.Prod.Id == selectedProductInventory.Id).SingleOrDefault();
                        if (existingOrder != null)
                        {
                            // To do: Make it possible to modify an order
                            Console.WriteLine("You've already ordered this item.");
                        } else {
                            Console.WriteLine("Quantity?");
                            int quantitySelection;
                            result = int.TryParse(Console.ReadLine(), out quantitySelection);
                            if (result)
                            {
                                if (quantitySelection < 1 || quantitySelection > selectedProductInventory.Quantity)
                                    Console.WriteLine("Invalid entry. Try again.");
                                else
                                {
                                    newOrder.LineItems.Add(new p0class.LineItem
                                        {
                                            Quantity = quantitySelection,
                                            Prod = selectedProductInventory.Prod
                                        }
                                    );
                                    selectedProductInventory.Quantity -= quantitySelection;
                                    modifiedItems.Add(selectedProductInventory);
                                    newOrder.TotalPrice += quantitySelection * selectedProductInventory.Prod.Price;
                                }
                            }
                        }
                    }
                    else
                        Console.WriteLine("Invalid entry, please try again."); 
                }
            }
            if (newOrder.LineItems.Count > 0)
            {
                ShowOrder(newOrder);
                datastore.SaveOrder(newOrder, modifiedItems);
            }
        }

        static void ShowOrder(p0class.Order p_order)
        {
            Console.WriteLine("Item,Quantity,Item Price,Line Price");
            foreach (p0class.LineItem item in p_order.LineItems)
            {
                Console.WriteLine($"{item.Prod.Name},{item.Quantity},{item.Prod.Price},{item.Quantity * item.Prod.Price}");
            }
            Console.WriteLine($"Total Price: {p_order.TotalPrice}");
        }

        static void OrderHistoryUI(SQLDatastore datastore)
        {
            bool repeat = true;

            Console.WriteLine("Order History:");
            Console.WriteLine("1. By Customer");
            Console.WriteLine("2. By Store Front");
            ChoosingChoice inquiryType = ChoosingChoice.DefaultState;
            while (repeat)
            {
                switch (Console.ReadLine())
                {
                    case "1":
                        inquiryType = ChoosingChoice.Customer;
                        repeat = false;
                        break;
                    case "2":
                        inquiryType = ChoosingChoice.StoreFront;
                        repeat = false;
                        break;
                    default:
                        Console.Write("Invalid entry, try again.");
                        break;                        
                }
            }
            if (inquiryType == ChoosingChoice.DefaultState)
                throw new InvalidProgramException("Program flow error: OrderHistoryUI");
            int userChoice = SelectChoice(datastore, inquiryType);
            List<p0class.Order> orderHistory = datastore.LoadOrderHistory(userChoice, (inquiryType == ChoosingChoice.Customer));

            foreach (p0class.Order order in orderHistory)
            {
                ShowOrder(order);
            }                  
        }

        static void ListProducts(List<p0class.Product> p_prodList)
        {
            Console.WriteLine("#,Name,Price,Description,Category");
            int i = 1;
            foreach (p0class.Product prod in p_prodList)
            {
                Console.WriteLine($"{i},{prod.Name},{prod.Price},{prod.Description},{prod.Category}");
                i += 1;
            }
        }

       static void SelectProduct(SQLDatastore datastore, p0class.StoreFront store, List<p0class.Product> prodList)
        {
            bool repeat = true;

            while (repeat)
            {
                Console.WriteLine("Enter product number.");
                int selection;
                if (int.TryParse(Console.ReadLine(), out selection) &&
                    selection >= 1 && selection <= prodList.Count)
                {
                    Console.WriteLine("Select quantity.");
                    int quantity;
                    if (int.TryParse(Console.ReadLine(), out quantity) && quantity >= 1)
                    {
                        datastore.UpdateOrAddInventory(store.Id, prodList[selection-1].Id, quantity);
                        store.Inventory = datastore.LoadLineItemsById(store.Id, false);
                        repeat = false;
                    }
                    else
                    {
                        Console.WriteLine("Invalid Entry, try again.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Entry, try again.");
                }
            }
        }

        static void ReplenishInventoryUI(SQLDatastore datastore)
        {
            int userChoice = SelectChoice(datastore, ChoosingChoice.StoreFront);
            if (userChoice == -1)
                return;

            p0class.StoreFront store = datastore.LoadStoreFrontById(userChoice);
            List<p0class.Product> prodList = datastore.GetAllProducts();
            bool repeat = true;
            while(repeat)
            {
                Console.WriteLine("0. Done");
                Console.WriteLine("1. List Current Inventory");
                Console.WriteLine("2. List Products");
                Console.WriteLine("3. Select Product");
                switch (Console.ReadLine())
                {
                    case "0":
                        repeat = false;
                        break;

                    case "1":
                        ListLineItems(store.Inventory);
                        break;

                    case "2":
                        ListProducts(prodList);
                        break;

                    case "3":
                        SelectProduct(datastore, store, prodList);
                        break;

                    default:
                        Console.WriteLine("Invalid Entry, try again.");
                        break;
                }
            }
            
        }

        static void MainMenu(SQLDatastore datastore)
        {
            bool looping = true;

            while (looping)
            {
                Console.WriteLine("Select:");
                Console.WriteLine("0: Exit");
                Console.WriteLine("1. Add Customer");
                Console.WriteLine("2. Search Customer");
                Console.WriteLine("3. Search Store Fronts");
                Console.WriteLine("4. List Store Inventory");
                Console.WriteLine("5. Place Order");
                Console.WriteLine("6. View Order History");
                Console.WriteLine("7. Replenish Inventory");
                switch(Console.ReadLine())
                {
                    case "0":
                        looping = false;
                        break;
                    case "1":
                        AddCustomerUI(datastore);
                        break;
                    case "2":
                        SearchCustomer(datastore);
                        break;
                    case "3":
                        SearchStoreFront(datastore);
                        break;
                    case "4":
                        StoreFrontInventoryUI(datastore);
                        break;
                    case "5":
                        CreateOrderUI(datastore);
                        break;
                    case "6":
                        OrderHistoryUI(datastore);
                        break;
                    case "7":
                        ReplenishInventoryUI(datastore);
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
