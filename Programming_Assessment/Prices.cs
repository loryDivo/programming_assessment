using System.Collections.Generic;
using System.Xml.Serialization;

namespace Programming_Assessment
{
    [XmlRoot(ElementName = "ItemPricesRoot")]
    public class ItemPricesRoot
    {
        [XmlElement(ElementName = "ItemPricesList")]
        public ItemPrices ItemPrices { get; set; }
    }
    [XmlRoot(ElementName = "ItemPricesList")]
    public class ItemPrices
    {
        [XmlElement(ElementName = "ItemPrice")]
        public HashSet<ItemPrice> ItemPriceSet { get; set; }
    }
    [XmlRoot(ElementName = "ItemPrice")]
    public class ItemPrice
    {
        [XmlElement(ElementName = "Item")]
        public string Item { get; set; }
        [XmlElement(ElementName = "Price")]
        public float Price { get; set; }
    }
}
