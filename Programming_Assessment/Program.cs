using System;
using System.Collections.Generic;

namespace Programming_Assessment
{
    static class Program
    {
        public static void Main(string[] args)
        {
            XmlParser<ItemPricesRoot> xmlParser = new XmlParser<ItemPricesRoot>("data/Prices.xml");
            xmlParser.LoadFile();
            ItemPricesRoot itemPricesRoot = xmlParser.Deserialize("ItemPricesRoot");

            JsonParser<Payments> jsonParser = new JsonParser<Payments>("data/Payments.json");
            jsonParser.LoadFile();
            List<Payments> payments = jsonParser.Deserialize();

            PurchasesDatParser purchasesDatParser = new PurchasesDatParser("data/Purchases.dat");
            purchasesDatParser.LoadFile();
            Purchases purchases = purchasesDatParser.Deserialize();

            
        }
    }
}
