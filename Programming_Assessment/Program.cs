using System;
namespace Programming_Assessment
{
    static class Program
    {
        public static void Main(string[] args)
        {
            XmlParser<ItemPricesRoot> xmlParser = new XmlParser<ItemPricesRoot>("data\\Prices.xml");
            xmlParser.LoadFile();
            ItemPricesRoot itemPricesRoot = xmlParser.Deserialize("ItemPricesRoot");
            for (int i = 0; i < itemPricesRoot.ItemPricesList.ItemPrice.Length; i++)
            {
                Console.WriteLine(itemPricesRoot.ItemPricesList.ItemPrice[i].Item);
            }
        }
    }
}
