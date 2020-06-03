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

            JsonParser<Payment> jsonParser = new JsonParser<Payment>("data/Payments.json");
            jsonParser.LoadFile();
            List<Payment> payments = jsonParser.Deserialize();
        }
    }
}
