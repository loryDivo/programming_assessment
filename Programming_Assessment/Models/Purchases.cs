using System;
using System.Collections.Generic;
using System.Text;

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
