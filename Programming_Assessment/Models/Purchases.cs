using System;
using System.Collections.Generic;

namespace Programming_Assessment
{
    public class Purchases
    {
        public List<Purchase> PurchasesList { get; set; }
        public Purchases()
        {
            PurchasesList = new List<Purchase>();
        }
    }
    public class Purchase
    {
        public String Customer { get; set; }
        public DateTime Date { get; set; }
        public List<Item> Items { get; set; }

        public Purchase(String iCustomer, DateTime iDate, List<Item> iItems)
        {
            this.Customer = iCustomer;
            this.Date = iDate;
            this.Items = iItems;
        }
        public Purchase()
        {
            Items = new List<Item>();
        }
    }
    public class Item
    {
        public String ItemNumber { get; set; }
    }

}
